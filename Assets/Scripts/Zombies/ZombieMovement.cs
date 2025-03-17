using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    ZombieStat zombie;
    Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<ZombieStat>();
        player = FindObjectOfType<PlayerMovement>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, zombie.currentMoveSpeed * Time.deltaTime);
        
    }
}
