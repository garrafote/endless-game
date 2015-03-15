using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    GameObject[] blocks;
    List<Block> movingBlocks;

    float distance = 0;
    float targetDistance = 20;
    private GameObject lastBlock;
    int nextBlockID;

    void Awake()
    {
        nextBlockID = -1;
        movingBlocks = new List<Block>();

        var resBlocks = Resources.LoadAll<GameObject>("Blocks");

        // group and order blocks by level
        // select block arrays
        blocks = resBlocks.OrderBy(g => g.name).ToArray();

        SpawnBlock(1,0);
        SpawnBlock(1,20);
    }

    void SpawnBlock(int blockIndex, float position = 40)
    {
        var go = GameObject.Instantiate(blocks[blockIndex], new Vector3(position, 2f, 0), Quaternion.identity) as GameObject;

        var block = go.GetComponent<Block>();

        lastBlock = go;

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

        distance += deltaDistance;
        if( distance > targetDistance && nextBlockID >= 0)
        {
			if (movingBlocks.Count != 0)
			{
				SpawnBlock(nextBlockID, lastBlock.transform.position.x + 20.0f);
			}
			else
			{
				SpawnBlock(nextBlockID, 0.0f);
			}
			targetDistance += 20;

            nextBlockID = -1;
        }
    }

    void Update()
    {
		if (nextBlockID == -1)
		{
			UIManager.Instance.canUse = true;
			for (int i = 0; i < blocks.Length; i++)
			{
				if (Input.GetKeyDown(i.ToString()))
				{
					if (UIManager.Instance.Abilities[(i - 1)].UseAbility())
					{
						nextBlockID = i;

					}
				}
			}
		}
		else
		{
			UIManager.Instance.canUse = false;
		}
    }
}
