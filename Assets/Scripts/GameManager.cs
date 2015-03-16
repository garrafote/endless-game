using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    GameObject[] blocks;
    List<Block> movingBlocks;

    [SerializeField]
    GameObject tokenPrefab;

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

        SpawnBlock(1,0, false);
        SpawnBlock(1,20, false);
    }

    void SpawnBlock(int blockIndex, float position = 40, bool addToken = true)
    {
        var go = GameObject.Instantiate(blocks[blockIndex], new Vector3(position, 2f, 0), Quaternion.identity) as GameObject;

        var block = go.GetComponent<Block>();

        lastBlock = go;

        if (addToken && Random.Range(0.0f, 1.0f) < 1f)
        {
            AddToken(go);
        }

        movingBlocks.Add(block);
    }

    void AddToken(GameObject go)
    {
        var tokenList = go.transform.FindChild("Token");

        var index = Random.Range(0, tokenList.childCount);
        var tokenContainer = tokenList.GetChild(index);

        var token = (GameObject)Instantiate(tokenPrefab, tokenContainer.position + Vector3.up * .2f, Quaternion.identity);
        token.transform.parent = tokenContainer;
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
					int val = i;
					if(i == 0)
					{
						val = 9;
					}
					else
					{
						val = i - 1;
					}

					if (UIManager.Instance.Abilities[val].UseAbility())
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
