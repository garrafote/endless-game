using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public float Move(float distance)
    {
        var pos = transform.position;
        pos.x -= distance;

        transform.position = pos;

        return pos.x;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(20, 20, 1));
    }


}
