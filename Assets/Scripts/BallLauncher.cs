
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
    private Image speedUpButton, ballsReturnButton;

    private GameObject startDragPositionShow;
    private LaunchPreview launchPreview;
    private BlockSpawner blockSpawner;
    private List<Ball> balls = new List<Ball>();

    private Vector3 startDragPosition;
    private Vector3 endDragPosition;
    private Vector2 originLoseCheck = new Vector2(-2.8f, -3.5f);

    private int ballsReady;
    private int ballsLaunched;
    private WaitForSeconds waitForLaunchBall = new WaitForSeconds(0.05f);
    private bool isBallsLaunched = true;
    private bool isBallsReturned = true;
    private bool isDragged = true;
    private bool isSpeedUp;
    private Quaternion defaultRotation;

    internal void ReturnBall()
    {
        ballsReady++;

        if (isBallsLaunched && BallsReturn.needSavePosition == false)
        {
            transform.position = BallsReturn.ballPosition;
        }
        if (ballsReady == ballsLaunched)
        {
            StopAllCoroutines();
            ballsLaunched = 0;
            ballsReturnButton.gameObject.SetActive(false);
            blockSpawner.SpawnRowOfBlocks();

            isBallsReturned = true;
            BallsReturn.needSavePosition = true;
        }
    }

    public void ReturnBalls()
    {
        StopAllCoroutines();
        isBallsLaunched = true;
        transform.rotation = defaultRotation;
        transform.position = BallsReturn.ballPosition;
        foreach (Ball ball in balls)
        {
            if (ball.isActive)
                ball.GetComponent<Rigidbody2D>().velocity = transform.position - ball.transform.position + Vector3.down;
            else
                ball.endPosition = transform.position;
        }
    }

    public void SpeedUp()
    {
        if (isSpeedUp)
        {
            Time.timeScale = 1f;
            speedUpButton.color = Color.green;
        }
        else
        {
            Time.timeScale = 10f;
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

    public void SetBalls(int count)
    {
        if (balls.Count < count)
        {
            for (int i = 0; i < count - balls.Count; i++)
            {
                CreateBall();
            }
        }
        else 
        {
            foreach (Ball ball in balls.GetRange(0, balls.Count - count))
            {
                Destroy(ball.gameObject);
            }
            balls.RemoveRange(0, balls.Count - count);
        }
    }

    private void Awake()
    {
        launchPreview = GetComponent<LaunchPreview>();
        blockSpawner = FindObjectOfType<BlockSpawner>();

        startDragPositionShow = GameObject.Find("StartDragPosition");
        startDragPositionShow.SetActive(false);

        defaultRotation = transform.rotation;

        ballsReturnButton.gameObject.SetActive(false);

        ballsLaunched = 0;
    }

    private void Update()
    {
        if (isBallsReturned)
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
        isDragged = true;
        
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
            if (Vector3.Angle(direction, Vector3.up) > 80)
            {
                direction.x /= Mathf.Abs(direction.x);
                direction.y = 0.15f;
            }

            isBallsLaunched = false;
            isBallsReturned = false;
            direction.Normalize();
            CreateBall();

            ballsReady = 0;
            StartCoroutine(SetActive(ballsReturnButton.gameObject, 3f));
            foreach (var ball in balls)
            {
                ball.transform.position = transform.position;
                ball.isActive = true;
                ball.GetComponent<Rigidbody2D>().AddForce(direction);

                ballsLaunched++;

                yield return waitForLaunchBall;
            }
            isBallsLaunched = true;
            transform.rotation = defaultRotation;
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
            if (Vector3.Angle(direction, Vector3.up) > 80)
            {
                direction.x /= Mathf.Abs(direction.x);
                direction.y = 0.15f;
            }

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
        isDragged = false;
        startDragPositionShow.SetActive(true);
        startDragPositionShow.transform.position = worldPosition;

        startDragPosition = worldPosition;
        launchPreview.SetStartPoint(transform.position);
    }

    private bool CanLaunch(Vector3 direction)
    {
        float angle = Vector3.Angle(direction, Vector3.up);
        bool isCorrectLength = direction.magnitude > 1f;
        bool isCorrectAngle = angle < 120;
        
        return isCorrectAngle && isCorrectLength && isBallsReturned && isDragged == false;
    }
}
