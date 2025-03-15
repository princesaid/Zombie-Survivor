using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStat : MonoBehaviour
{
    public ZombieScriptableObject zombieData;

    //current stats
    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;

    void Awake()
    {
        currentDamage = zombieData.Damage;
        currentHealth = zombieData.MaxHealth;
        currentMoveSpeed = zombieData.MoveSpeed;

    }

    public void TakeDamage(float takenDamage)
    {
        currentHealth -= takenDamage;
        if (currentHealth <= 0)
        {
            Kill();
        }


    }
    public void Kill()
    {
        Destroy(gameObject);

    }

    private void Oy2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

}
