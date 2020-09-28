
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Block blockPrefab;
    private int playWidth = 7;
    private float distanceBetweenBlock = 0.8f;
    private int rowsSpawned = 1;

    private List<Block> blocksSpawned = new List<Block>();

    private void OnEnable()
    {
        SpawnRowOfBlocks();
    }

    internal void SpawnRowOfBlocks()
    {
        foreach (Block block in blocksSpawned)
        {
            if (block != null)
            {
                block.transform.position += Vector3.down * distanceBetweenBlock;
            }
        }

        for (int i = 0; i < playWidth; i++)
        {
            if (Random.Range(0, 100) < 31)
            {
                Block block = Instantiate(blockPrefab, GetPosition(i), Quaternion.identity);
                int hits = rowsSpawned;

                block.SetHits(hits);

                blocksSpawned.Add(block);
            }
        }
        rowsSpawned++;
    }

    private Vector3 GetPosition(int i)
    {
        Vector3 position = transform.position;
        position += Vector3.right * i * distanceBetweenBlock;

        return position;
    }
}
