using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    
    [Header("Movement")]
    
    public float runSpeed = 8;
    private float xMove;
    public float maxSpeed = 5f;
    
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


    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded) jump = true;


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
        
         
//        Vector2 size = collider.size;
//        Vector2 centerPoint = new Vector2( collider.offset.x, collider.offset.y);
//        
//        
//        float top = collider.offset.y + (collider.size.y / 2f);
        float btm = collider.offset.y - (collider.size.y / 2f);
        float left = collider.offset.x - (collider.size.x / 2f);
        float right = collider.offset.x + (collider.size.x /2f);
//         
//        Vector3 topLeft = transform.TransformPoint (new Vector3( left, top, 0f));
//        Vector3 topRight = transform.TransformPoint (new Vector3( right, top, 0f));
        Vector3 btmLeft = transform.TransformPoint (new Vector3( left, btm, 0f));
        Vector3 btmRight = transform.TransformPoint (new Vector3( right, btm, 0f));
//        
//        Debug.DrawLine(topLeft,btmRight);
//        Debug.DrawLine(topRight,btmLeft);
//        Debug.DrawLine(btmLeft,btmRight);
//        
//        grounded = Physics2D.OverlapArea(btmLeft, btmRight, groundLayers);
//
//        print(Physics2D.OverlapArea(btmLeft, btmRight, groundLayers).gameObject.name);
        
        RaycastHit2D hit = Physics2D.Raycast(btmLeft, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, 0.1f, 0));
        
        RaycastHit2D hit2 = Physics2D.Raycast(btmLeft, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(btmLeft, btmLeft - new Vector3(0, 0.1f, 0));

        RaycastHit2D hit3 = Physics2D.Raycast(btmRight, -Vector2.up, 0.1f, groundLayers);
        Debug.DrawLine(btmRight, btmRight - new Vector3(0, 0.1f, 0));


        
        grounded = (hit.collider != null | hit2.collider != null | hit3.collider != null);
       
        
        float h;

        //store Right Hor input
        h = Input.GetAxis("Horizontal");

        //store wish move Right Hor
        float xMove = h * runSpeed;

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
    }


    private void Flip()
    {
        facingRight = !facingRight;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}