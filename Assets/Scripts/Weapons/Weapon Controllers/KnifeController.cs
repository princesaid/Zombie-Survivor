using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        //Old Behavior
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(playerMovement.lastMovedVector);

        

    }





}
