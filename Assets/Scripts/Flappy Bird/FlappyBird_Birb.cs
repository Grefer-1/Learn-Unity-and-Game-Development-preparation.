using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FlappyBird_Birb : MonoBehaviour
{
    [SerializeField] private float upForce = 350f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!FlappyBird_GameManager.Instance.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Base" || collision.transform.name == "Mouth")
            FlappyBird_GameManager.Instance.SetGameOver();
    }

}
