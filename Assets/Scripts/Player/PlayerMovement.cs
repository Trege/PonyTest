using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour  
{
    Rigidbody body;
    [NonSerialized]
    public float speed = 0;
    public float maxSpeed = 0;
    public float horizontalDrag = 0.1f;
    private bool grounded = true;
    private bool jumping = false;
    private float jumpHeight = 2f;
    private float jumpDelay = 0;
    private float groundAngle = 0;
    private Vector3 lastNormal = Vector3.zero;
    private AnimationHandler animationHandler;
    private Transform trans;
    public Renderer spriteRenderer;
    private Material spriteMat;
    private Transform spriteTrans;
    private EntityStats stats = new EntityStats();
    // Use this for initialization
	void Start ()
    {
        trans = transform;
        body = rigidbody;
        animationHandler = spriteRenderer.GetComponent<AnimationHandler>();
        spriteMat = spriteRenderer.material;
        spriteTrans = spriteRenderer.transform;
        stats.agi = 9;
        speed = stats.agi * 5;
        maxSpeed = stats.agi;
    }

    public float targetSpriteAngle = 90;
    private float currentSpriteAngle = 90;
    private float tmp = 0;
    private RaycastHit hit;
    public void Update()
    {
        if (!grounded)
        {
            if (currentSpriteAngle != 90 && Physics.Raycast(trans.position, Vector3.down, out hit, 100))
            {
                if (Mathf.Abs(GetNormalAngle(hit.normal) - 90f) <= 0.1f)
                    targetSpriteAngle = 90;
            }
        }
        else
        {
            if (lastNormal.x < 0)
                targetSpriteAngle = 180 - groundAngle;
            else
                targetSpriteAngle = groundAngle;
        }

        currentSpriteAngle = Mathf.SmoothDampAngle(currentSpriteAngle, targetSpriteAngle, ref tmp, 0.08f);
        spriteTrans.transform.eulerAngles = new Vector3(currentSpriteAngle, 270, 90);
    }

	public void FixedUpdate()
    {
        
        body.drag = 0f;
        int sign = 0;
        if (Input.GetAxis("Horizontal") > 0)
            sign = 1;
        else if (Input.GetAxis("Horizontal") < 0)
            sign = -1;
        if (((jumping || groundAngle >= 30) && lastNormal.y > 0.1f) && Mathf.Abs(body.velocity.x) < maxSpeed)
            body.AddForce(sign * speed, 0, 0);

        if (sign != 0)
        {
            //Apply horizontal friction
            body.velocity = new Vector3(body.velocity.x * (1 - horizontalDrag), body.velocity.y, body.velocity.z * (1 - horizontalDrag));

            spriteMat.mainTextureScale = new Vector2(sign, 1);
            if (grounded)
                animationHandler.state = PlayerStates.Walk;
        }
        else
        {
            if (grounded)
                body.velocity = new Vector3(body.velocity.x * 0.5f, grounded ? 0 : body.velocity.y, body.velocity.z * 0.5f);
            //Prevent sliding
            if (Mathf.Abs(body.velocity.x) > 0 && Mathf.Abs(body.velocity.x) < 1)
                body.drag = 100f;
            
            if (grounded)
                animationHandler.state = PlayerStates.Stand;
        }

        if (jumpDelay > 0)
            jumpDelay -= Time.fixedDeltaTime;
        else
            jumpDelay = 0;
        if ((grounded || (body.velocity.y == 0 && Input.GetAxis("Horizontal") == 0)) && Input.GetButtonDown("Jump"))
        {
            body.velocity = new Vector3(body.velocity.x, Mathf.Sqrt(2 * jumpHeight * 12f), body.velocity.z);
            animationHandler.state = PlayerStates.Jump;
            jumpDelay = 0.1f;
            jumping = true;
            //Remove the next line if you don't want jumping to automatically reset your rotation
            
        }
    
        grounded = false;
        groundAngle = 0;
    }
    public void OnCollisionStay(Collision collision)
    {
        lastNormal = collision.contacts[0].normal;
        if (collision.contacts[0].normal.y > 0.1f)
        {
            groundAngle = Mathf.Max(groundAngle, GetNormalAngle(collision.contacts[0].normal));
            grounded = groundAngle >= 30f && jumpDelay == 0;
            if (jumpDelay == 0)
                jumping = false;
        }
    }

    private float GetNormalAngle(Vector3 normal)
    {
        return Vector3.Angle(new Vector3(normal.x, 0, normal.z).normalized, normal);
    }
}
