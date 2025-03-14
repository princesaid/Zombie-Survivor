using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehavior : MonoBehaviour
{
    public float destroyAfterSecondsl;

    // Start is called before the first frame update
     protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecondsl)        ;

    }


}
