
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
    public int RowsSpawned
    {
        set { rowsSpawned = value; }
        get { return rowsSpawned; }
    }

    [SerializeField]
    private Text lvlCountText;

    private List<Block> blocksSpawned = new List<Block>();

    private void OnEnable()
    {
        SpawnRowOfBlocks();
    }

    public IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(0.05f);

        bool win = true;
        foreach (Block block in blocksSpawned)
        {
            if (block != null)
            {
                win = false;
                break;
            }
        }

        if (win)
        {
            GameController.instance.SaveCheckPoint(rowsSpawned);
        }
    }

    internal void RemoveBlocks(int checkPoint)
    {
        
        foreach (Block block in blocksSpawned)
        {
            if (block != null)
            {
                Destroy(block.gameObject);
            }
        }
        blocksSpawned = new List<Block>();
        rowsSpawned = checkPoint;
        SpawnRowOfBlocks();
    }

    internal void SpawnRowOfBlocks()
    {
        List<Block> tempBlocs = new List<Block>();
        lvlCountText.text = rowsSpawned.ToString();
        bool isLose = false;
        foreach (Block block in blocksSpawned)
        {
            if (block != null)
            {
                tempBlocs.Add(block);
                block.transform.position += Vector3.down * distanceBetweenBlock;

                if (block.transform.position.y < -3)
                {
                    isLose = true;
                }
            }
        }
        blocksSpawned = tempBlocs;

        if (isLose)
        {
            GameController.instance.Lose();
            return;
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
