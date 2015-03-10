using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    GameObject[][] blocks;
    List<Block> movingBlocks;

    float distance = 0;
    float targetDistance = 20;



    void Awake()
    {
        movingBlocks = new List<Block>();

        var resBlocks = Resources.LoadAll<GameObject>("Blocks");

        // group and order blocks by level
        // select block arrays
        blocks = resBlocks.GroupBy((System.Func<GameObject, string>)GroupByLevel).OrderBy(g => g.Key).Select(g => g.ToArray()).ToArray();

        SpawnBlock(0);
        SpawnBlock(20);
        SpawnBlock(40);
    }

    string GroupByLevel(GameObject t)
    {
        const string pattern = @"Block (?<level>\d+) \d+";
        
        var match = Regex.Match(t.name, pattern);

        if (!match.Success)
        {
            return "99999";
        }

        Debug.Log(match.Groups["level"].Value);

        return match.Groups["level"].Value;
    }

    void SpawnBlock(float position = 40)
    {
        var levelBlocks = blocks[0];
        var index = Random.Range(0, levelBlocks.Length);

        var go = GameObject.Instantiate(levelBlocks[index], new Vector3(position, 0, 0), Quaternion.identity) as GameObject;

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

        distance += deltaDistance;

        if (distance > targetDistance)
        {
            SpawnBlock();
            targetDistance += 20;
        }
    }
}
