using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    public float moveSpeed;
    Rigidbody2D rigidBody;

    [HideInInspector]
    public float lastHorizontalValue;
    [HideInInspector]
    public float lastVerticalValue;

    [HideInInspector]
    public Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();

    }
    void FixedUpdate()
    {
        Move();

    }
    void InputManagement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;

        if (movementDirection.x != 0)
        {
            lastHorizontalValue = movementDirection.x;
        }
        if (movementDirection.y != 0)
        {
            lastVerticalValue = movementDirection.y;
        }

    }
    void Move()
    {
        rigidBody.velocity = new Vector2(movementDirection.x * moveSpeed, movementDirection.y * moveSpeed);

    }
}
