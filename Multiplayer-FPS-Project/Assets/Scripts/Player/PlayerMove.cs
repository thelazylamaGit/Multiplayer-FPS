using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform cameraPos;

    [Header("Misc")]
    private float playerHeight = 2f;
    [SerializeField]
    private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    private float moveMultiplier = 10f;
    [SerializeField] private float airMultiplier = 0.4f;
    private float moveX;
    private float moveY;
    private Vector3 moveDir;

    [Header("Drag")]
    private float groundDrag = 6f;
    private float airDrag = 2f;



    [Header("Jumping")]
    public float jumpForce = 5f;
    [Header("Ground Detection")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheck;
    private bool isGrounded;
    private float groundDistance = 0.4f;

    private Vector3 slopeMoveDir;
    RaycastHit slopeHit;
    private bool OnSlope()
    {
        //Checks if on a slope
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (isLocalPlayer)
        {
            playerCamera = Instantiate(playerCamera, transform.position, Quaternion.identity);
            playerCamera.GetComponent<MoveCamera>().cameraPos = cameraPos;
            GetComponent<PlayerLook>().cameraHolder = playerCamera.transform;
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    void Update()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        PlayerInput();
        ControlDrag();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        //Uses ProjectOnPlane to find correct vector for moving on slope
        slopeMoveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void PlayerInput()
    {
        //Gets Keyboard Input
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        //Uses Orientation Transform to find move direction
        moveDir = orientation.forward * moveY + orientation.right * moveX;
    }

    void MovePlayer()
    {
        //Moves Normally
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDir.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        // Moves On Slope
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDir.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        //Moves In Air
        else if (!isGrounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        //Resets Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        // Adds Jump Force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    //Changes Rigidbody Drag depending on if grounded
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
}
