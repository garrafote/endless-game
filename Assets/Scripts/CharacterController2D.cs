using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {

    private Vector2 jumpForce = new Vector2(0, 25);

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
        
        // TODO: get collider to enable double jump

        var pos = (Vector2)transform.position;

        var hit = Physics2D.OverlapArea(
            pointA: pos + new Vector2(-0.5f, -0.5f),
            pointB: pos + new Vector2( 0.5f,  0.0f),
            layerMask: (1 << LayerMask.NameToLayer("Platform")));

        // it is grounded if is touching any platform
        var isGrounded = hit != null;

        var jump = Input.GetButtonDown("Jump");

        var vel = rigidbody2D.velocity;
        vel.x = 0;

        if (jump)
        {
            //clear vertical velocity
            vel.y = 0;
        }

        rigidbody2D.velocity = vel;

        if (jump) 
        {
            // apply force
            rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
        }

    }

    void FixedUpdate() {
	
    }
}
