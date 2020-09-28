
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour
{
    [SerializeField]
    private Ball ballPrefab;

    [SerializeField]
    private Text ballsCount, ballsRedyCount;

    [SerializeField]
    private Image speedUpButton, ballsReturnButton;
    private Text speedUpButtonText;

    private GameObject startDragPositionShow;
    private LaunchPreview launchPreview;
    private BlockSpawner blockSpawner;
    private List<Ball> balls = new List<Ball>();

    private Vector3 startDragPosition;
    private Vector3 endDragPosition;

    private int ballsReady;
    private WaitForSeconds waitForLaunchBall = new WaitForSeconds(0.05f);
    private bool ballsLaunched = true;
    private bool ballsReturned = true;
    private bool dragged = true;
    private bool isSpeedUp;
    private Quaternion defaultRotation;

    internal void ReturnBall()
    {
        ballsReady++;
        ballsRedyCount.text = ballsReady.ToString();
        ballsCount.text = balls.Count.ToString();
        if (ballsLaunched && BallsReturn.needSavePosition == false)
        {
            transform.position = BallsReturn.ballPosition;
        }
        if (ballsReady == balls.Count)
        {
            StopAllCoroutines();
            ballsReturnButton.gameObject.SetActive(false);
            blockSpawner.SpawnRowOfBlocks();
            transform.position = BallsReturn.ballPosition;

            ballsReturned = true;
            BallsReturn.needSavePosition = true;
        }
    }

    public void ReturnBalls()
    {
        foreach (Ball ball in balls)
        {
            if (ball.isActive)
                ball.GetComponent<Rigidbody2D>().velocity = transform.position - ball.transform.position + Vector3.down;
        }
    }

    public void SpeedUp()
    {
        if (isSpeedUp)
        {
            Time.timeScale = 1f;
            speedUpButtonText.gameObject.SetActive(false);
            speedUpButton.color = Color.green;
        }
        else
        {
            speedUpButtonText.gameObject.SetActive(true);
            Time.timeScale = 2f;
            speedUpButton.color = Color.red;
        }

        isSpeedUp = !isSpeedUp;
    }

    private void CreateBall()
    {
        Ball ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        ball.isActive = false;
        balls.Add(ball);
    }

    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
        blockSpawner = FindObjectOfType<BlockSpawner>();

        startDragPositionShow = GameObject.Find("StartDragPosition");
        startDragPositionShow.SetActive(false);

        defaultRotation = transform.rotation;

        ballsReturnButton.gameObject.SetActive(false);

        speedUpButtonText = speedUpButton.GetComponentInChildren<Text>();
        speedUpButtonText.gameObject.SetActive(false);
    }



    private void Update()
    {
        if (ballsReturned)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10f;

            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(worldPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueDrag(worldPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }

    private void EndDrag()
    {
        launchPreview.Hide();
        startDragPositionShow.SetActive(false);

        StartCoroutine(LaunchBalls());
        dragged = true;
    }

    private IEnumerator SetActive(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(true);
    }

    private IEnumerator LaunchBalls()
    {

        Vector3 direction = startDragPosition - endDragPosition;
        if (CanLaunch(direction))
        {

            ballsLaunched = false;
            ballsReturned = false;
            direction.Normalize();
            CreateBall();

            ballsReady = 0;
            ballsRedyCount.text = ballsReady.ToString();

            foreach (var ball in balls)
            {
                ball.transform.position = transform.position;
                ball.isActive = true;
                ball.GetComponent<Rigidbody2D>().AddForce(direction);

                yield return waitForLaunchBall;
            }
            ballsLaunched = true;

            transform.rotation = defaultRotation;
            StartCoroutine(SetActive(ballsReturnButton.gameObject, 3f));

        }
        else transform.rotation = defaultRotation;

    }

    private void ContinueDrag(Vector3 worldPosition)
    {
        endDragPosition = worldPosition;
        Vector3 direction = startDragPosition - endDragPosition;

        transform.rotation = Quaternion.Euler(0f, 0f, Vector3.Angle(Vector3.right, direction) - 180);

        if (CanLaunch(direction))
        {
            direction.Normalize();
            launchPreview.SetEndPoint(direction);
        }
        else
        {
            launchPreview.Hide();
        }
    }

    private void StartDrag(Vector3 worldPosition)
    {
        dragged = false;
        startDragPositionShow.SetActive(true);
        startDragPositionShow.transform.position = worldPosition;

        startDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

    private bool CanLaunch(Vector3 direction)
    {
        float angle = Vector3.Angle(direction, Vector3.up);
        bool correctLength = direction.magnitude > 1f;
        bool correctAngle = angle < 83;
        return correctAngle && correctLength && ballsReturned && dragged == false;
    }
}
