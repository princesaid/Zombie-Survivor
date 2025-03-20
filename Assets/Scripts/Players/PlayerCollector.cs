using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public float pullSpeed;

    PlayerStats player;
    CircleCollider2D playerCollector;
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        playerCollector.radius = player.CurrentMagnet;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            rigidbody.AddForce(forceDirection * pullSpeed);
            collectible.Collect();
            //Destroy(collision.gameObject);
        }
    }
}
