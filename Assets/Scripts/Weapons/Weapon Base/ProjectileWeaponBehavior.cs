using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileWeaponBehavior : MonoBehaviour
{

    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    //current valuse 
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }





    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);


    }

    // Update is called once per frame
    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float directionX = direction.x;
        float directionY = direction.y;

        Vector3 scale = transform.localScale;

        Vector3 rotation = transform.rotation.eulerAngles;

        if (directionX < 0 && directionY == 0) // left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (directionX == 0 && directionY < 0) // down
        {
            scale.y = scale.y * -1;

        }
        else if (directionX == 0 && directionY > 0) // up
        {
            scale.x = scale.x * -1;
        }
        else if (directionX > 0 && directionY > 0) // right up
        {
            rotation.z = 0f;
        }
        else if (directionX > 0 && directionY < 0) // right down
        {
            rotation.z = -90f;

        }
        else if (directionX < 0 && directionY > 0) // left up
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY < 0) // left down
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;

        }


        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);



    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            ZombieStat zombie = collision.GetComponent<ZombieStat>();
            zombie.TakeDamage(currentDamage);
            ReducePierce();

        }
        else if (collision.CompareTag("Prop"))
        {
            if (collision.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
    }
    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }

    }

}
