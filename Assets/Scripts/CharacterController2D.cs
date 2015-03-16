using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {

    private Vector2 jumpForce = new Vector2(0, 25);
    private bool hasExtraJump;

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
        
        var pos = (Vector2)transform.position;

        // check death state
        if (pos.y < -10)
        {
            Destroy(gameObject);
            // TODO: death screen
            Application.LoadLevel(Application.loadedLevel);
            return;
        }

        var hit = Physics2D.OverlapArea(
            pointA: pos + new Vector2(-0.45f, -0.5f),
            pointB: pos + new Vector2( 0.45f,  0.0f),
            layerMask: (1 << LayerMask.NameToLayer("Platform")));

        // it is grounded if is touching any platform
        var isGrounded = hit != null;
        
        // reeset extra jump whenever player hits the ground
        if (isGrounded) hasExtraJump = true;

        var jumpPressed = Input.GetButtonDown("Jump");
        var jump = jumpPressed && (isGrounded || hasExtraJump);

        var vel = rigidbody2D.velocity;
        vel.x = 0;

        // clear vertical velocity if player is about to jump
        if (jump) vel.y = 0;

        rigidbody2D.velocity = vel;

        if (jump) 
        {
            // apply force
            rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
            
            // burn extra jump if was not grounded
            if (!isGrounded) hasExtraJump = false;
        }

    }

    void FixedUpdate() {
	
    }
}
