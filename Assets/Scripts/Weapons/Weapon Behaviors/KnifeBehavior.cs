using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBehavior : ProjectileWeaponBehavior
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();



    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // if (Input.GetMouseButtonDown(0))
        // {
            
        // }

        //OldBehavior
        transform.position += direction * currentSpeed *Time.deltaTime;

    }
}
