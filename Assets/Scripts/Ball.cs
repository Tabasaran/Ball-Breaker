using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    [SerializeField]
    private float moveSpeed = 10f;
    public bool isActive;
    public Vector3 endPosition;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        endPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isActive)
            rigidBody2D.velocity = rigidBody2D.velocity.normalized * moveSpeed;
        else if (Vector3.Distance(transform.position, endPosition) > 0.2f)
            rigidBody2D.velocity = (endPosition - transform.position).normalized * moveSpeed;
        else if (Vector3.Distance(transform.position, endPosition) < 0.2f)
        { 
            rigidBody2D.velocity = rigidBody2D.velocity = Vector2.zero;
            transform.position = endPosition;
        }
        else
            rigidBody2D.velocity = Vector2.zero;
    }
}
