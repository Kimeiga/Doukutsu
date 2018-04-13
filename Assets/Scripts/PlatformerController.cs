using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private SpriteAnimator spriteAnimator;
    
    
    [Header("Movement")]
    
    public float runSpeed = 8;
    public float maxSpeed = 5f;
    public float xMove;
    
    [HideInInspector] public bool facingRight;
    
    [Space(10)]
    
    [Header("Jumping")]
    
    [HideInInspector] public bool jump;
    public float fallMultiplier = 2.5f;
    public float jumpForce = 1000f;
    public float lowJumpMultiplier = 2f;

    [Space(10)]
    
    [Header("Grounding")]
    
//    public Transform topLeft;
//    public Transform bottomRight;
    public float groundedDistance = 1;
    public bool grounded;
    public LayerMask groundLayers;


    public bool duck;

    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteAnimator = transform.GetChild(0).GetComponent<SpriteAnimator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded) jump = true;

        duck = Input.GetButton("Duck");

        if (Input.GetAxisRaw("Horizontal") == -1 && !facingRight)
            Flip();
        else if (Input.GetAxisRaw("Horizontal") == 1 && facingRight)
            Flip();

        //mario jump
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //check if grounded
        IsGrounded();

        //store Right Hor input
        float h = Input.GetAxis("Horizontal");

        //store wish move Right Hor
        xMove = h * runSpeed;

        //move Right Horly
        rb.velocity += new Vector2(xMove - rb.velocity.x, 0);

        //jump
        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        //limit speed by maxSpeed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        if (duck)
        {
            collider.size = new Vector2(collider.size.x, 0.5f);
            collider.offset = new Vector2(collider.offset.x, 0.25f);
        }
        else
        {
            collider.size = new Vector2(collider.size.x, 1f);
            collider.offset = new Vector2(collider.offset.x, 0.5f);
        }
        
    }

    //sets grounded to whether we're grounded
    private void IsGrounded()
    {
        //get extents
        float btm = collider.offset.y - (collider.size.y / 2f);
        float left = collider.offset.x - (collider.size.x / 2f);
        float right = collider.offset.x + (collider.size.x /2f);
        
        //get corners
        Vector3 btmMid = transform.TransformPoint (new Vector3( 0, btm, 0f));
        Vector3 btmLeft = transform.TransformPoint (new Vector3( left, btm, 0f));
        Vector3 btmRight = transform.TransformPoint (new Vector3( right, btm, 0f));

        //raycast from bottom midpoint down 
        RaycastHit2D hit = Physics2D.Raycast(btmMid, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(btmMid, btmMid - new Vector3(0, 0.1f, 0));
        
        //raycast from bottom left corner down
        RaycastHit2D hit2 = Physics2D.Raycast(btmLeft, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(btmLeft, btmLeft - new Vector3(0, 0.1f, 0));

        //raycast from bottom right corner down
        RaycastHit2D hit3 = Physics2D.Raycast(btmRight, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(btmRight, btmRight - new Vector3(0, 0.1f, 0));

        //if one of those intersects with something, we are grounded
        grounded = (hit.collider != null | hit2.collider != null | hit3.collider != null);
    }


    private void Flip()
    {
        facingRight = !facingRight;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}