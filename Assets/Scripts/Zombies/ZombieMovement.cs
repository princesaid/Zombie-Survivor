using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    ZombieStat zombie;
    Transform player;

    public bool hasLegs = false;
    public bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<ZombieStat>();
        player = FindObjectOfType<PlayerMovement>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasLegs)
        {
            Vector2 direction = player.transform.position - transform.position;
            //transform.localScale;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);

            }

        }
        if (move == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombie.currentMoveSpeed * Time.deltaTime);
        }
        else
        {
            return;
        }

    }
}
