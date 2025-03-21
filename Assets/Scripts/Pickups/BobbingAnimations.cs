using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimations : MonoBehaviour
{
    public float frequecy;
    public float magnitude;
    public Vector3 direction;
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequecy) * magnitude;

    }
}
