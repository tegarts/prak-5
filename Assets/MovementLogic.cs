using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    private Rigidbody rb;
    public float walkSpeed = 0.1f, runSpeed = 0.5f, jumpPower = 10f, fallSpeed = 0.1f, airMultiplier = 10f, hitPoints = 100f;
    private Transform playerOrientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool isGrounded = true, aerialBoost = true, AimMode= false, TPSMode = true;
    public Animator anim;
    public CameraLogic camLogic;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerOrientation = this.GetComponent<Transform>();
    }

    private void Update()
    {
        Movement();
        Jump();
        ShootLogic();
        AimModeAdjuster();

        if (Input.GetKey(KeyCode.F))
        {
            PlayerGetHit(100f);
        }
    }

    private void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

        if (isGrounded && moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * runSpeed * 10f, ForceMode.Force);
            }
            else
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);
            }
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
            
        }
        else if (!isGrounded)
        {
            rb.AddForce(Vector3.down * fallSpeed * rb.mass, ForceMode.Force);
            if (aerialBoost)
            {
                rb.AddForce(moveDirection.normalized * walkSpeed * 10f * airMultiplier, ForceMode.Impulse);
                aerialBoost = false;
            }
          
        }
    }

    public void GroundedChanger()
    {
        isGrounded = true;
        aerialBoost = true;
        anim.SetBool("Jump", false);
    }

    public void AimModeAdjuster()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (AimMode)
            {
                TPSMode = true;
                AimMode = false;
                anim.SetBool("AimMode", false);
            }
            else if (TPSMode)
            {
                TPSMode = false;
                AimMode = true;
                anim.SetBool("AimMode", true);
            }
            camLogic.CameraModeChanger(TPSMode, AimMode);
        }
    }

    private void ShootLogic()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (moveDirection.normalized != Vector3.zero)
            {
                anim.SetBool("WalkShoot", true);
                anim.SetBool("IdleShoot", false);
            }
            else
            {
                anim.SetBool("IdleShoot", true);
                anim.SetBool("WalkShoot", false);
            }
        }
        else
        {
            anim.SetBool("WalkShoot", false);
            anim.SetBool("IdleShoot", false);
        }
    }

    public void PlayerGetHit(float damage)
    {
        Debug.Log("Player Receive Dama - " + damage);
        hitPoints -= damage;

        if (hitPoints == 0f)
        {
            anim.SetBool("Death", true);
        }
    }
}
