using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public float destroyAfterSecondsl;

    //current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected float currentPierce;

    void Awake()
    {
        currentCooldownDuration = weaponData.CooldownDuration;
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentPierce = weaponData.Pierce;
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecondsl);

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            ZombieStat zombie = collision.GetComponent<ZombieStat>();
            zombie.TakeDamage(currentDamage);
        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(currentDamage);

            }
        }

    }


}
