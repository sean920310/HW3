using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class BallManager : MonoBehaviour, IPunObservable
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

    public BallManager()
    {
        Instance = this;
    }

    private void Start()
    {
        //Instance = this;

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
        if(pv) pv.RPC("RpcUpdateBallState", RpcTarget.Others, (int)state);
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
            if (pv)
                pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, (Vector3)ServeDirection.normalized * ServeForce);
            else
                rb.AddForce( ServeDirection.normalized * ServeForce, ForceMode.Impulse);
        }
        else
        {
            ServeDirection = new Vector2(-Mathf.Abs(ServeDirection.x), Mathf.Abs(ServeDirection.y));
            if (pv)
                pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, (Vector3)ServeDirection.normalized * ServeForce);
            else
                rb.AddForce( ServeDirection.normalized * ServeForce, ForceMode.Impulse);

        }

        if (pv)
        {
            pv.RPC("RpcNormalHit", RpcTarget.All);
            pv.RPC("RpcOnHit", RpcTarget.All);
        }
        else
        {
            trailRenderer.startColor = NormalTrailColor;
            trailRenderer.enabled = true;
            HitSound.Play();
            HitParticle.Play();
        }

    }

    public void SetPosition(Vector3 pos)
    {
        if (pv)
            pv.RPC("RpcSetPosition", RpcTarget.MasterClient, pos);
        else
            transform.position = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        if (pv)
            pv.RPC("RpcSetRotation", RpcTarget.MasterClient, rot);
        else
            transform.rotation = rot;
    }

    public void SetVelocity(Vector3 vel)
    {
        if (pv)
            pv.RPC("RpcSetVelocity", RpcTarget.MasterClient, vel);
        else
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
            if(pv) pv.RPC("RpcResetVel", RpcTarget.All);

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
                    else if (racketManager.transform.root.name == "Player2")
                        p2StatesPanel.ShowMessageRight("Defence!!!");

                    playerInfo.Info.defenceCount++;

                    if (pv)
                        pv.RPC("RpcDefenceHit", RpcTarget.All, PhotonNetwork.IsMasterClient);
                    else
                    {
                        trailRenderer.startColor = DefenseTrailColor;
                        DefenseVFX.Play();
                        HitSound.Play();
                    }

                }
                else
                {
                    SwitchState(BallStates.NormalHit);

                    if (pv)
                        pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, racketManager.transform.up.normalized * racketManager.swinDownForce);
                    else
                        rb.AddForce(racketManager.transform.up.normalized * racketManager.swinDownForce, ForceMode.Impulse);

                    if (pv)
                        pv.RPC("RpcNormalHit", RpcTarget.All);
                    else
                    {
                        trailRenderer.startColor = NormalTrailColor;
                        HitSound.Play();
                    }
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
                    else if (racketManager.transform.root.name == "Player2")
                        p2StatesPanel.ShowMessageRight("Smash!!!");

                    playerInfo.Info.smashCount++;

                    if (pv)
                        pv.RPC("RpcSmashHit", RpcTarget.All, PhotonNetwork.IsMasterClient);
                    else
                    {
                        PowerHitParticle.Play();
                        trailRenderer.startColor = SmashTrailColor;
                        SmashSound.Play();
                    }
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
                        else if (racketManager.transform.root.name == "Player2")
                            p2StatesPanel.ShowMessageRight("Defence!!!");

                        playerInfo.Info.defenceCount++;

                        if (pv)
                            pv.RPC("RpcDefenceHit", RpcTarget.All, PhotonNetwork.IsMasterClient);
                        else
                        {
                            trailRenderer.startColor = DefenseTrailColor;
                            DefenseVFX.Play();
                            HitSound.Play();
                        }

                    }
                    else
                    {
                        SwitchState(BallStates.NormalHit);

                        if (pv)
                            pv.RPC("RpcBallAddForce", RpcTarget.MasterClient, -racketManager.transform.up.normalized * racketManager.hitForce);
                        else
                            rb.AddForce(-racketManager.transform.up.normalized * racketManager.hitForce, ForceMode.Impulse);

                        if (pv)
                            pv.RPC("RpcNormalHit", RpcTarget.All);
                        else
                        {
                            trailRenderer.startColor = NormalTrailColor;
                            HitSound.Play();
                        }
                    }
                }
            }

            racketManager.boxColliderDisable();

            if (pv)
                pv.RPC("RpcOnHit", RpcTarget.All);
            else
            {
                trailRenderer.enabled = true;
                HitParticle.Play();
            }
        }
    }

    // Hit Floor
    private void OnCollisionEnter(Collision collision)
    {
        if (pv && !PhotonNetwork.IsMasterClient) return;
        if(ballStates != BallStates.Serving && collision.transform.tag == "Ground")
        {
            SwitchState(BallStates.Serving);

            if (collision.gameObject.name == "Player2Floor")
            {
                if (pv)
                    PhotonManager.Instance.P1GetPoint();
                else
                    GameManager.instance.p1GetPoint();
            }
            else if (collision.gameObject.name == "Player1Floor")
            {
                if (pv)
                    PhotonManager.Instance.P2GetPoint();
                else
                    GameManager.instance.p2GetPoint();
            }

            if (pv)
                pv.RPC("RpcBallOnTouchGround", RpcTarget.All);
            else
                OnTouchGround();
        }
    }

    void OnTouchGround()
    {
        trailRenderer.startColor = NormalTrailColor;
        trailRenderer.enabled = false;
        HittingFloorSound.Play();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            rb.position = (Vector3)stream.ReceiveNext();
            rb.rotation = (Quaternion)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            rb.position += rb.velocity * lag;
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

    [PunRPC]
    void RpcBallOnTouchGround(PhotonMessageInfo info)
    {
        OnTouchGround();
    }

    [PunRPC]
    void RpcSetPosition(Vector3 pos, PhotonMessageInfo info)
    {
        transform.position = pos;
    }

    [PunRPC]
    void RpcSetRotation(Quaternion rot, PhotonMessageInfo info)
    {
        transform.rotation = rot;
    }

    [PunRPC]
    void RpcSetVelocity(Vector3 vel, PhotonMessageInfo info)
    {
        rb.velocity = vel;
    }

    [PunRPC]
    void RpcNormalHit(PhotonMessageInfo info)
    {
        trailRenderer.startColor = NormalTrailColor;
        HitSound.Play();
    }

    [PunRPC]
    void RpcSmashHit(bool player1, PhotonMessageInfo info)
    {
        if (player1)
            p1StatesPanel.ShowMessageLeft("Smash!!!");
        else
            p2StatesPanel.ShowMessageRight("Smash!!!"); 
        PowerHitParticle.Play();
        trailRenderer.startColor = SmashTrailColor;
        SmashSound.Play();
    }

    [PunRPC]
    void RpcDefenceHit(bool player1, PhotonMessageInfo info)
    {
        if (player1)
            p1StatesPanel.ShowMessageLeft("Defence!!!");
        else
            p2StatesPanel.ShowMessageRight("Defence!!!"); 

        trailRenderer.startColor = DefenseTrailColor;
        DefenseVFX.Play();
        HitSound.Play();
    }

    [PunRPC]
    void RpcOnHit(PhotonMessageInfo info)
    {
        trailRenderer.enabled = true;
        HitParticle.Play();
    }
    #endregion
}
