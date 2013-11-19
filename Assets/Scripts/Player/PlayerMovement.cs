using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour  
{
    Rigidbody body;
   public float speed = 3;
   public float maxSpeed = 7;
   private bool canJump = true;
   private bool grounded = true;
   private float jumpHeight = 2f;
   private float groundAngle = 0;
	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(1,1,1);
    }
	
	
	// Update is called once per frame
	public void FixedUpdate()
{
    
    {
        if (Input.GetAxis("Horizontal") > 0 && body.velocity.x < maxSpeed)
            body.AddForce(speed, 4, 0);
        if (Input.GetAxis("Horizontal") < 0 && body.velocity.x > -maxSpeed)
            body.AddForce(-speed, 4, 0);

        if ((grounded || (body.velocity.y == 0 && Input.GetAxis("Horizontal") == 0)) && Input.GetButtonDown("Jump")) body.velocity = new Vector3(body.velocity.x, Mathf.Sqrt(2 * jumpHeight * 9.8f), body.velocity.z);
    }

    Debug.Log(grounded + " - " + groundAngle);
    grounded = false;
    groundAngle = 0;
        

}
    public void OnCollisionStay(Collision collision)
    {
        Vector3 contact = collision.contacts[0].normal;
        contact.y = 0;
        groundAngle = Mathf.Max(groundAngle, Vector3.Angle(contact.normalized, collision.contacts[0].normal));
        grounded = groundAngle >= 30f;
    }
}
