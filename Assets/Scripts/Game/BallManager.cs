using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class BallManager : MonoBehaviour
{
    static public BallManager Instance;

    public enum BallStates
    {
        Serving,
        NormalHit,
        Smash,
        Defense,
        TouchFloor
    }

    public Rigidbody rb;
    [SerializeField] Transform centerOfMass;

    [SerializeField] public StatesPanel p1StatesPanel;
    [SerializeField] public StatesPanel p2StatesPanel;

    [SerializeField] public Transform centerBorder;

    // Visual Effect
    [SerializeField] ParticleSystem HitParticle;
    [SerializeField] ParticleSystem PowerHitParticle;

    [SerializeField] VisualEffect DefenseVFX;

    TrailRenderer trailRenderer;
    [SerializeField] Color SmashTrailColor;
    [SerializeField] Color DefenseTrailColor;
    [SerializeField] Color NormalTrailColor;

    // Audio
    [SerializeField] AudioSource HitSound;
    [SerializeField] AudioSource SmashSound;
    [SerializeField] AudioSource HittingFloorSound;

    // Serve State will set to true in GameManager when some one is about to serve.
    [ReadOnly] [SerializeField] private BallStates ballStates = BallStates.Serving;

    public bool BallInLeftSide { get; private set; }

    private PhotonView pv;

    private void Start()
    {
        Instance = this;

        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        rb.centerOfMass = centerOfMass.position;

        Physics.IgnoreLayerCollision(7, 8, true); // Border
        Physics.IgnoreLayerCollision(7, 9, true); // Player

        ballStates = BallStates.Serving;
    }

    private void FixedUpdate()
    {
        if(ballStates == BallStates.Serving)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        BallInLeftSide = (transform.position.x < centerBorder.transform.position.x);

        transform.rotation = Quaternion.Euler(0, 0, Quaternion.FromToRotation(Vector3.right, rb.velocity).eulerAngles.z);
    }

    public void SwitchState(BallStates state)
    {
        ballStates = state;
        pv.RPC("RpcUpdateBallState", RpcTarget.Others, (int)state);
    }

    public bool IsInState(BallStates states)
    {
        return ballStates == states;
    }

    public void Serve(bool faceRight, Vector2 ServeDirection, float ServeForce)
    {
        SwitchState(BallStates.NormalHit);

        rb.velocity = Vector3.zero;

        if (faceRight)
        {
            ServeDirection = new Vector2(Mathf.Abs(ServeDirection.x), Mathf.Abs(ServeDirection.y));
            rb.AddForce( ServeDirection.normalized * ServeForce, ForceMode.Impulse);
        }
        else
        {
            ServeDirection = new Vector2(-Mathf.Abs(ServeDirection.x), Mathf.Abs(ServeDirection.y));
            rb.AddForce( ServeDirection.normalized * ServeForce, ForceMode.Impulse);

        }


        trailRenderer.enabled = true;
        HitParticle.Play();

        HitSound.Play();

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }


    // Hit Racket
    private void OnTriggerEnter(Collider other)
    {
        RacketManager racketManager = other.transform.GetComponent<RacketManager>();
        PlayerInformationManager playerInfo = other.transform.root.GetComponent<PlayerInformationManager>();
        if (racketManager != null)
        {
            if (other.transform.root.GetComponent<PhotonView>() && !other.transform.root.GetComponent<PhotonView>().IsMine) return;
            rb.velocity = Vector3.zero;
            pv.RPC("RpcResetVel", RpcTarget.All);

            if (racketManager.isSwinDown)
            {
                playerInfo.Info.underhandCount++;

                // If the previous state was a smash, then this hit should be a defensive shot.
                if (ballStates == BallStates.Smash)
                {
                    SwitchState(BallStates.Defense);

                    if(pv)
                        pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, racketManager.transform.up.normalized * racketManager.defenceHitForce);
                    else
                        rb.AddForce(racketManager.transform.up.normalized * racketManager.defenceHitForce, ForceMode.Impulse);

                    if (racketManager.transform.root.name == "Player1")
                        p1StatesPanel.ShowMessageLeft("Defence!!!");
                    else
                        p2StatesPanel.ShowMessageRight("Defence!!!");

                    playerInfo.Info.defenceCount++;

                    trailRenderer.startColor = DefenseTrailColor;
                    DefenseVFX.Play();

                }
                else
                {
                    SwitchState(BallStates.NormalHit);

                    if (pv)
                        pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, racketManager.transform.up.normalized * racketManager.swinDownForce);
                    else
                        rb.AddForce(racketManager.transform.up.normalized * racketManager.swinDownForce, ForceMode.Impulse);

                    trailRenderer.startColor = NormalTrailColor;
                    HitSound.Play();
                }
            }
            else
            {
                // Power Hit
                Vector3 hittingAngle = Quaternion.FromToRotation(Vector3.right, -racketManager.transform.up).eulerAngles;
                if ((360 >= hittingAngle.z && hittingAngle.z >= 170 || 10 >= hittingAngle.z && hittingAngle.z >= 0) && 
                    !racketManager.transform.root.GetComponent<PlayerMovement>().onGround)
                {
                    SwitchState(BallStates.Smash);

                    if (pv)
                        pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, (-racketManager.transform.up.normalized) * racketManager.powerHitForce);
                    else
                        rb.AddForce((-racketManager.transform.up.normalized) * racketManager.powerHitForce, ForceMode.Impulse);

                    if (racketManager.transform.root.name == "Player1")
                        p1StatesPanel.ShowMessageLeft("Smash!!!");
                    else
                        p2StatesPanel.ShowMessageRight("Smash!!!");

                    playerInfo.Info.smashCount++;

                    PowerHitParticle.Play();
                    trailRenderer.startColor = SmashTrailColor;
                    SmashSound.Play();
                }
                else
                {
                    playerInfo.Info.overhandCount++;

                    // If the previous state was a smash, then this hit should be a defensive shot.
                    if (ballStates == BallStates.Smash)
                    {
                        SwitchState(BallStates.Defense);

                        if (pv)
                            pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, -racketManager.transform.up.normalized * racketManager.defenceHitForce);
                        else
                            rb.AddForce(-racketManager.transform.up.normalized * racketManager.defenceHitForce, ForceMode.Impulse);

                        if (racketManager.transform.root.name == "Player1")
                            p1StatesPanel.ShowMessageLeft("Defence!!!");
                        else
                            p2StatesPanel.ShowMessageRight("Defence!!!");

                        playerInfo.Info.defenceCount++;

                        trailRenderer.startColor = DefenseTrailColor;
                        DefenseVFX.Play();

                    }
                    else
                    {
                        SwitchState(BallStates.NormalHit);

                        if (pv)
                            pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, -racketManager.transform.up.normalized * racketManager.hitForce);
                        else
                            rb.AddForce(-racketManager.transform.up.normalized * racketManager.hitForce, ForceMode.Impulse);

                        trailRenderer.startColor = NormalTrailColor;
                        HitSound.Play();
                    }
                }
            }

            racketManager.boxColliderDisable();

            trailRenderer.enabled = true;
            HitParticle.Play();
        }
    }

    // Hit Floor
    private void OnCollisionEnter(Collision collision)
    {
        if(ballStates != BallStates.Serving && collision.transform.tag == "Ground")
        {
            SwitchState(BallStates.Serving);

            if (collision.gameObject.name == "Player2Floor")
            {
                GameManager.instance.p1GetPoint();
            }
            else if (collision.gameObject.name == "Player1Floor")
            {
                GameManager.instance.p2GetPoint();
            }

            trailRenderer.startColor = NormalTrailColor;
            trailRenderer.enabled = false;
            HittingFloorSound.Play();
        }
    }

    #region PunRPC

    [PunRPC]
    void RpcUpdateBallState(int state, PhotonMessageInfo info)
    {
        ballStates = (BallStates)state;
    }

    [PunRPC]
    void RpcResetVel(PhotonMessageInfo info)
    {
        rb.velocity = Vector3.zero;
    }

    [PunRPC]
    void RpcBallAddForce(Vector3 force, PhotonMessageInfo info)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    #endregion
}
