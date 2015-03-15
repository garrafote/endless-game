using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    GameObject[] blocks;
    List<Block> movingBlocks;

    float distance = 0;
    float targetDistance = 20;



    void Awake()
    {
        movingBlocks = new List<Block>();

        var resBlocks = Resources.LoadAll<GameObject>("Blocks");

        // group and order blocks by level
        // select block arrays
        blocks = resBlocks.OrderBy(g => g.name).ToArray();

        SpawnBlock(1,0);
        SpawnBlock(1,20);
        SpawnBlock(1,40);
    }

    void SpawnBlock(int blockIndex, float position = 40)
    {
        var go = GameObject.Instantiate(blocks[blockIndex], new Vector3(position, 0, 0), Quaternion.identity) as GameObject;

        var block = go.GetComponent<Block>();

        movingBlocks.Add(block);
    }

    void FixedUpdate()
    {
        Block blockToRemove = null;

        var deltaDistance = Time.fixedDeltaTime * 10;

        foreach (var block in movingBlocks)
        {
            var blockPos = block.Move(deltaDistance);
            if (blockPos < -30)
            {
                blockToRemove = block;
            }
        }

        if (blockToRemove != null)
        {
            movingBlocks.Remove(blockToRemove);
            Destroy(blockToRemove.gameObject);
        }

        Event e = Event.current;
        if(e != null)
            Debug.Log((int)e.character);

    }


}
