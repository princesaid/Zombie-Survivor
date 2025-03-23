using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ZombieStat : MonoBehaviour
{
    public ZombieScriptableObject zombieData;

    //current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;
    Transform player;

    ZombieMovement zombieMovement;


    void Awake()
    {
        currentDamage = zombieData.Damage;
        currentHealth = zombieData.MaxHealth;
        currentMoveSpeed = zombieData.MoveSpeed;

        zombieMovement = GetComponent<ZombieMovement>();

    }
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;


    }

    public void TakeDamage(float takenDamage)
    {
        currentHealth -= takenDamage;
        if (currentHealth <= 0)
        {
            Kill();
        }


    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnZombie();

        }
    }
    public void Kill()
    {
        Destroy(gameObject);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Zombie Collided");
        if (collision.gameObject.CompareTag("Player"))
        {
            zombieMovement.move = false;
            //Debug.Log("Player takes damage");
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            zombieMovement.move = true;            
        }
        
    }
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        ZombieSpawner zombieSpawner = FindObjectOfType<ZombieSpawner>();
        zombieSpawner.OnZombieKilled();
    }

    void ReturnZombie()
    {
        ZombieSpawner zombieSpawner = FindObjectOfType<ZombieSpawner>();
        transform.position = player.position + zombieSpawner.relativeSpawnPoints[UnityEngine.Random.Range(0, zombieSpawner.relativeSpawnPoints.Count)].position;

    }

}
