using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pong_Ball : MonoBehaviour
{
    [SerializeField] private GameObject[] paddles;
    [SerializeField] private TextMeshProUGUI scorePaddleLeftText;
    [SerializeField] private TextMeshProUGUI scorePaddleRightText;
    [SerializeField] private float ballSpeed = 5f;

    private Rigidbody2D rb;
    private Vector3 lastVelocity;
    private int scorePaddleLeft = 0;
    private int scorePaddleRight = 0;

    // Start is called before the first frame update
    void Start()
    {
        int firstBounceSide = Random.value <= 0.5f ? -1 : 1;
        rb = GetComponent<Rigidbody2D>();
        BounceBall(firstBounceSide);
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
        
        if (transform.position.x > 9f)
        {
            scorePaddleLeft++; 
            scorePaddleLeftText.text = $"{scorePaddleLeft}";
            StartCoroutine(ResetBall(-1));
        }
        else if (transform.position.x < -9f)
        {
            scorePaddleRight++;
            scorePaddleRightText.text = $"{scorePaddleRight}";
            StartCoroutine(ResetBall(1));
        }
    }

    private IEnumerator ResetBall(int bounceSide)
    {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        yield return new WaitForSeconds(2);
        BounceBall(bounceSide);
    }

    private void BounceBall(int bounceSide)
    {
        rb.velocity = new Vector2(bounceSide * ballSpeed, -bounceSide * ballSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = new Vector2(direction.x + Mathf.Sign(direction.x) * Random.Range(0.15f, 0.45f), direction.y) * ballSpeed;
    }
}
