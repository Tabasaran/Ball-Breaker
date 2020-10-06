using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private BallLauncher ballLauncher;
    [SerializeField]
    private BlockSpawner blockSpawner;

    [SerializeField]
    private GameObject winEffect;
    [SerializeField]
    private Text checkpointText;

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
        StartCoroutine(Win(lvl));
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

    private IEnumerator Win(int lvl)
    {
        
        winEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        checkpointText.text = "checkpoint\n" + lvl;
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        checkpointText.gameObject.SetActive(false);
    }
}
