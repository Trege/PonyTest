using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour  
{
    Rigidbody body;
    public float speed = 1;
    public float maxSpeed = 2;
    public float horizontalDrag = 0.1f;
    private Vector3 dragVec = new Vector3(1, 0, 1);
    private bool canJump = true;
    private bool grounded = true;
    private float jumpHeight = 2f;
    private float groundAngle = 0;
    private AnimationHandler animation;
    private Transform trans;
    public Renderer spriteRenderer;
    private Material spriteMat;
    private EntityStats stats;
    // Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        animation = spriteRenderer.GetComponent<AnimationHandler>();
        spriteMat = spriteRenderer.material;
        body.AddForce(1,1,1);
        speed = stats.agi*40;
        maxSpeed = stats.agi*99;
    }
	
	
	// Update is called once per frame
	public void FixedUpdate()
    {
        int sign = 0;
        if (Input.GetAxis("Horizontal") > 0)
            sign = 1;
        else if (Input.GetAxis("Horizontal") < 0)
            sign = -1;
        if (Mathf.Abs(body.velocity.x) < maxSpeed)
            body.AddForce(sign * speed, 2, 0);

        //Apply horizontal friction
        if (sign == 0)
            body.velocity = new Vector3(body.velocity.x * (1 - horizontalDrag), body.velocity.y, body.velocity.z * (1 - horizontalDrag));

        if (sign != 0)
        {
            spriteMat.mainTextureScale = new Vector2(sign, 1);
            if (grounded)
                animation.state = PlayerStates.Walk;
        }
        else if (grounded)
            animation.state = PlayerStates.Stand;

        if ((grounded || (body.velocity.y == 0 && Input.GetAxis("Horizontal") == 0)) && Input.GetButtonDown("Jump"))
        {
            body.velocity = new Vector3(body.velocity.x, Mathf.Sqrt(2 * jumpHeight * 12f), body.velocity.z);
            animation.state = PlayerStates.Jump;
        }
    
        grounded = false;
        groundAngle = 0;
    }
    public void OnCollisionStay(Collision collision)
    {
        Vector3 contact = collision.contacts[0].normal;
        contact.y = 0;
        groundAngle = Mathf.Max(groundAngle, Vector3.Angle(contact.normalized, collision.contacts[0].normal));
        grounded = groundAngle >= 30f && body.velocity.y <= 0.015f + maxSpeed * Mathf.Cos(Mathf.Deg2Rad * groundAngle);
    }
}
