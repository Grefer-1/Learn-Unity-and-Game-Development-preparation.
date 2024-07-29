using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakout_Ball : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private Vector3 lastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    public void ApplyForce() => rb.velocity = new Vector2(0f, -1f * speed);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int side = 1;
        if (collision.transform.position.x - transform.position.x > 0f && collision.transform.name == "Paddle")
            side = -1;
        else
            side = 1;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = new Vector2(side * Mathf.Sign(direction.x), Mathf.Sign(direction.y)) * speed;
    }
}
