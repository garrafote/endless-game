using UnityEngine;
using System.Collections.Generic;

public abstract class ITarget : MonoBehaviour
{
    public abstract float pos { get; }
}

public class Follower : ITarget {

    public ITarget target;

    public float frameDelay = 20;
    float frameCount = 0;

    public override float pos
    {
        get
        {
            return memory.Peek();
        }
    }

    Queue<float> memory;

    void Awake()
    {
        memory = new Queue<float>();
        for (int i = 0; i < frameDelay; i++)
        {
            memory.Enqueue(transform.position.y);
        }
    }

    void FixedUpdate()
    {
        var position = transform.position;
        position.y = target.pos;

        if (frameCount < frameDelay)
        {
            position.x -= Time.fixedDeltaTime * 10;
            frameCount += 1;
        }

        transform.position = position;

        memory.Dequeue();
        memory.Enqueue(position.y);
    }

}
