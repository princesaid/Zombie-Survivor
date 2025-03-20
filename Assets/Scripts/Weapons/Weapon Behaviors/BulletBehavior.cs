using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : ProjectileWeaponBehavior
{
    private Camera mainCamera;
    protected Vector3 mousePosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


    }

    // Update is called once per frame
    protected override void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

    }
}
