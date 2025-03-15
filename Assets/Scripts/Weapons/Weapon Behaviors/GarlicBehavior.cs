using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehavior : MeleeWeaponBehavior
{
    List<GameObject> markedEnemies;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie") && !markedEnemies.Contains(collision.gameObject))
        {
            ZombieStat zombie = collision.GetComponent<ZombieStat>();
            zombie.TakeDamage(currentDamage);
            markedEnemies.Add(collision.gameObject);

        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(collision.gameObject))
            {
                breakable.TakeDamage(currentDamage);
                markedEnemies.Add(collision.gameObject);
                
            }
        }




    }



}
