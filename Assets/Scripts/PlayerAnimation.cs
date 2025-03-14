using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimation : MonoBehaviour
{

    Animator animator;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.movementDirection.x != 0 || playerMovement.movementDirection.y != 0)
        {
            animator.SetBool("Move", true);

            SpriteDirection();

            


        }
        else
        {
            animator.SetBool("Move", false);
            
        }
        

    }

    void SpriteDirection()
    {
        if (playerMovement.lastHorizontalValue < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

    }
}
