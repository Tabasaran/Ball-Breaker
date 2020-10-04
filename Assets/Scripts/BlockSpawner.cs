
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Block[] blockPrefabs;
    private int playWidth = 7;
    private float distanceBetweenBlock = 0.8f;
    private int rowsSpawned = 1;


    [SerializeField]
    private Text lvlCountText;

    private List<Block> blocksSpawned = new List<Block>();

    private void OnEnable()
    {
        SpawnRowOfBlocks();
    }

    internal void SpawnRowOfBlocks()
    {
        lvlCountText.text = rowsSpawned.ToString();
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
                Block block = Instantiate(RandomBlockPrefab(), GetPosition(i), Quaternion.identity);
                int hits = rowsSpawned;

                block.SetHits(hits);

                blocksSpawned.Add(block);
            }
        }
        rowsSpawned++;
        

    }

    private Block RandomBlockPrefab()
    {
        return blockPrefabs[Random.Range(0, blockPrefabs.Length)];
    }

    private Vector3 GetPosition(int i)
    {
        Vector3 position = transform.position;
        position += Vector3.right * i * distanceBetweenBlock;

        return position;
    }
}
