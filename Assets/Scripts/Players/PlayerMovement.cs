using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    //public float moveSpeed;


    [HideInInspector]
    public float lastHorizontalValue;
    [HideInInspector]
    public float lastVerticalValue;

    [HideInInspector]
    public Vector2 movementDirection;

    [HideInInspector]
    public Vector2 lastMovedVector;

    // Refernces
    Rigidbody2D rigidBody;

    PlayerStats player;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rigidBody = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f);

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
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movementDirection = new Vector2(moveX, moveY).normalized;

        if (movementDirection.x != 0)
        {
            lastHorizontalValue = movementDirection.x;
            lastMovedVector = new Vector2(lastHorizontalValue, 0f);
        }
        if (movementDirection.y != 0)
        {
            lastVerticalValue = movementDirection.y;
            lastMovedVector = new Vector2(0f, lastVerticalValue);
        }
        if (movementDirection.x != 0f && movementDirection.y != 0f)
        {
            lastMovedVector = new Vector2(lastHorizontalValue, lastVerticalValue);


        }

    }
  
    void Move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        rigidBody.velocity = new Vector2(movementDirection.x * player.CurrentMoveSpeed, movementDirection.y * player.CurrentMoveSpeed);

    }
}
