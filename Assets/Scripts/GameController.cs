using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private BallLauncher ballLauncher;
    [SerializeField]
    private BlockSpawner blockSpawner;
    public static GameController instance;
    private int checkPoint = 1;
    [SerializeField]
    private GameObject[] blockDestroyEffect;
    private int blockDestroyEffectCount = 0;

    private void Awake()
    {
        instance = this;
    }

    public void CheckWin()
    {
        StartCoroutine(blockSpawner.CheckWin());
    }
    public void SaveCheckPoint(int lvl)
    {
        checkPoint = lvl;
        ballLauncher.ReturnBalls();
    }
    public void Lose()
    {
        ballLauncher.SetBalls(checkPoint - 1);
        blockSpawner.RemoveBlocks(checkPoint);
        checkPoint = 1;
    }
    public void PlayBlockDestroyedEffect(Vector3 position)
    {
        blockDestroyEffect[blockDestroyEffectCount % blockDestroyEffect.Length].transform.position = position;
        blockDestroyEffect[blockDestroyEffectCount % blockDestroyEffect.Length].SetActive(true);
        blockDestroyEffectCount++;
    }
}
