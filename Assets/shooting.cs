using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{

    private Camera mainCamera;
    protected Vector3 mousePosition;
    protected virtual void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

       // Destroy(gameObject, destroyAfterSeconds);



    }

    // Update is called once per frame
    void Update()
    {

    }
}
