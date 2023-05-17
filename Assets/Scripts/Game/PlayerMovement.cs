using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [ReadOnly] public Rigidbody rb;
    [ReadOnly] public Animator animator;
    [SerializeField] Transform LeftHand;
    [SerializeField] AudioSource SwooshSound;

    // Player Parameter
    // Move
    [Header("Movement")]
    [SerializeField] float movementSpeed = 4.9f;
    [SerializeField] float airMovementSpeed = 2.4f;

    // Jump
    [Header("Jump")]
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] Transform GroundChk;
    [SerializeField] LayerMask WhatIsGround;
    [ReadOnly] public bool onGround = true;
    [SerializeField] float normalGravityScale = 1.0f;
    public bool enableFallGravityScale;
    [DrawIf("enableFallGravityScale", true, ComparisonType.Equals)]
    [SerializeField] float fallGravityScale = 1.5f;


    // Swin
    [Header("Swin")]
    [SerializeField] RacketManager racket;
    [SerializeField] public bool CanSwin { get; private set; } = false;

    [SerializeField] public bool PrepareServe { get; private set; } = false;
    bool facingRight = false;

    // Input Flag
    [ReadOnly] public float moveInputFlag = 0.0f;
    [ReadOnly] public bool jumpInputFlag = false;
    [ReadOnly] public bool swinUpInputFlag = false;
    [ReadOnly] public bool swinDownInputFlag = false;

    //Pun
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (enableFallGravityScale)
        {
            rb.useGravity = false;
        }
        facingRight = (transform.rotation.y == 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(GroundChk.position, GroundChk.position + Vector3.down * 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!pv || pv.IsMine)
        {
            // Check On Ground
            onGround = Physics.Raycast(GroundChk.position, Vector3.down, 0.02f, WhatIsGround);
            animator.SetBool("OnGround", onGround);

            // Jump
            // Serving Can't Jump
            if (!PrepareServe && jumpInputFlag && onGround)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpInputFlag = false;
            }

            // Serve
            if (PrepareServe)
            {
                // Set Serve Animation
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ServePrepare"))
                {
                    animator.SetBool("ServePrepare", true);
                }

                BallManager.Instance.SetPosition(LeftHand.position);
                BallManager.Instance.SetRotation(LeftHand.rotation);
                BallManager.Instance.SetVelocity(Vector3.zero);

                if (swinUpInputFlag && CanSwin)
                {
                    racket.boxColliderDisable();

                    SwooshSound.Play();
                    animator.SetTrigger("LongServe");
                    if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "LongServe");

                    BallManager.Instance.Serve(facingRight, racket.LongServeDirection, racket.LongServeForce);

                    PrepareServe = false;
                    swinUpInputFlag = false;
                    animator.SetBool("ServePrepare", false);

                    GameManager.instance.EndServe();
                    if(PhotonManager.Instance)
                        PhotonManager.Instance.EndServe();
                }
                else if (swinDownInputFlag && CanSwin)
                {
                    racket.boxColliderDisable();

                    SwooshSound.Play();
                    animator.SetTrigger("ShortServe");
                    if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "ShortServe");

                    BallManager.Instance.Serve(facingRight, racket.ShortServeDirection, racket.ShortServeForce);

                    PrepareServe = false;
                    swinUpInputFlag = false;
                    animator.SetBool("ServePrepare", false);

                    GameManager.instance.EndServe();
                    if(PhotonManager.Instance)
                        PhotonManager.Instance.EndServe();
                }
                return;
            }

            // Swing
            if (swinUpInputFlag && CanSwin)
            {
                swinUpInputFlag = false;

                SwooshSound.Play();
                animator.SetTrigger("SwingUp");
                if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "SwingUp");
                racket.swinUp();
            }
            if (swinDownInputFlag && CanSwin)
            {
                swinDownInputFlag = false;
                SwooshSound.Play();

                // SwinDown Type Detection: Front Ball SwingDownFront, vice versa.
                if (facingRight)
                {
                    if (BallManager.Instance.transform.position.x - transform.position.x <= 0.2f)
                    {
                        animator.SetTrigger("SwingDownBack");
                        if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "SwingDownBack");
                    }
                    else
                    {
                        animator.SetTrigger("SwingDownFront");
                        if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "SwingDownFront");
                    }
                }
                else
                {
                    if (BallManager.Instance.transform.position.x - transform.position.x >= 0.2f)
                    {
                        animator.SetTrigger("SwingDownBack");
                        if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "SwingDownBack");
                    }
                    else
                    {
                        animator.SetTrigger("SwingDownFront");
                        if (pv) pv.RPC("RpcAnimTrigger", RpcTarget.Others, "SwingDownFront");
                    }
                }
                racket.swinDown();
            }

        }
    }

    private void FixedUpdate()
    {
        if (!pv || pv.IsMine)
        {
            // Fall Gravity
            if (enableFallGravityScale)
            {
                Vector3 gravity = gravity = -9.81f * normalGravityScale * Vector3.up;
                if (!onGround && rb.velocity.y <= 0)
                {
                    gravity = -9.81f * fallGravityScale * Vector3.up;
                }
                rb.AddForce(gravity, ForceMode.Acceleration);
            }

            // Movement
            float movementX = moveInputFlag;

            if (Mathf.Abs(movementX) > 0f)
                animator.SetBool("Move", true);
            else
                animator.SetBool("Move", false);


            if (onGround)
                rb.velocity = new Vector3(movementX * movementSpeed, rb.velocity.y, 0);
            else
                rb.velocity = new Vector3(movementX * airMovementSpeed, rb.velocity.y, 0);
        }
    }

    // This SetRacketColliderOff is for animation event
    public void SetRacketColliderOff()
    {
        racket.boxColliderDisable();
    }

    public void SetRacketTrailOn()
    {
        racket.setTrailOn();
    }

    public void SetRacketTrailOff()
    {
        racket.setTrailOff();
    }

    public void SetPlayerServe(bool serve)
    {
        PrepareServe = serve;
    }

    public void swinDisable()
    {
        CanSwin = false;
    }

    public void swinEnable()
    {
        CanSwin = true;
    }

    public void ResetSwinInputFlag()
    {
        swinUpInputFlag = false;
        swinDownInputFlag = false;
    }
    public void ResetInputFlag()
    {
        moveInputFlag = 0.0f;
        jumpInputFlag = false;
        swinUpInputFlag = false;
        swinDownInputFlag = false;
    }
    public void ResetAllAnimatorTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }

    #region Input Handler
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            moveInputFlag = context.ReadValue<float>();

        if (context.canceled)
            moveInputFlag = 0f;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            jumpInputFlag = true;

        if (context.canceled)
            jumpInputFlag = false;
    }
    public void OnSwinUp(InputAction.CallbackContext context)
    {
        if (context.started)
            swinUpInputFlag = true;

        if (context.canceled)
            swinUpInputFlag = false;
    }
    public void OnSwinDown(InputAction.CallbackContext context)
    {
        if (context.started)
            swinDownInputFlag = true;

        if (context.canceled)
            swinDownInputFlag = false;
    }
    #endregion


    #region PunRPC

    [PunRPC]
    void RpcAnimTrigger(string param, PhotonMessageInfo info)
    {
        animator.SetTrigger(param);
    }

    #endregion


    #region Mobile Input Handler

    public void OnMove(Vector2 value)
    {
        moveInputFlag = value.normalized.x;
    }
    public void OnJump(bool value)
    {
        jumpInputFlag = value;
    }
    public void OnSwinUp(bool value)
    {
        swinUpInputFlag = value;
    }
    public void OnSwinDown(bool value)
    {
        swinDownInputFlag = value;
    }

    #endregion

}
