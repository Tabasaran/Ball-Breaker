using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallsReturn : MonoBehaviour
{
    private BallLauncher ballLauncher;
    public static Vector3 ballPosition;
    public static bool needSavePosition = true;
    private void Awake()
    {
        ballLauncher = FindObjectOfType<BallLauncher>();
        ballPosition = ballLauncher.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (needSavePosition)
            {
                needSavePosition = false;
                ballPosition.x = collision.transform.position.x;
            }
            ballLauncher.ReturnBall();
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ball.isActive = false;
            ball.endPosition = ballPosition;
        }
    }
}
