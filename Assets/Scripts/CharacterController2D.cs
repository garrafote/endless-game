using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : ITarget {

    private Vector2 jumpForce = new Vector2(0, 25);

    Queue<float> memory;

    public override float pos
    {
        get
        {
            return memory.Peek();
        }
    }

	// Use this for initialization
	void Awake () {
        memory = new Queue<float>();

        for (int i = 0; i < 20; i++)
        {
            memory.Enqueue(transform.position.y);
        }


	}
	
	// Update is called once per frame
	void Update () {
        
        // TODO: get collider to enable double jump
        var col = Physics2D.BoxCast((Vector2)transform.position + new Vector2(0, -0.5f), new Vector2(1, 0.1f), 0, -Vector2.up, 0.01f, Physics2D.IgnoreRaycastLayer);

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
        memory.Dequeue();
        memory.Enqueue(transform.position.y);
	}
}
