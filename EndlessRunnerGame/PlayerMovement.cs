using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [Header("Move settings")]
    [SerializeField] private float moveSpeed;
    private float runMultiplier = 1f;
    [SerializeField] private float maxHorizontalDistance;

    [Header("Jump settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;

    private bool inAirAnim;

    [Header("Debug")]
    [SerializeField] private bool grounded;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        StartRunning();

        grounded = true;
    }

    public void StartRunning()
    {
        anim.SetBool("isRunning", true);
    }

    
    void Update()
    {
        GroundCheck();

        if (grounded && !LevelManager.levelManager.stunned)
        {
            if(Input.GetAxis("Horizontal") != 0)
            {
                MoveHorizontal();
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }            
        }

    }

    void MoveHorizontal()
    {
        // Not possible to move mid air
        if (inAirAnim)
        {
            return;
        }

        float xMovement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(xMovement, 0f, 0f) * moveSpeed * runMultiplier * Time.deltaTime;

        // Add speed by pressing shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            runMultiplier = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runMultiplier = 1f;
        }

        // Clamp horizontal movement to the play area
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -maxHorizontalDistance, maxHorizontalDistance), transform.position.y, transform.position.z);
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        anim.SetTrigger("jump");
        inAirAnim = true;
    }

    void GroundCheck()
    {
        // Raycast down to the ground layer
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);

        if(grounded && inAirAnim)
        {
            inAirAnim = false;
            anim.SetTrigger("landed");
        }
    }

}
