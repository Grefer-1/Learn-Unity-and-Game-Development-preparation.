using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird_Tunnel : MonoBehaviour
{
    public event EventHandler OnScored;
    public bool hasScored { get; private set; }

    private float speed = 2.5f;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.x < -9f)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.position.x > transform.position.x)
            OnScored?.Invoke(this, EventArgs.Empty);
    }

    public void Scored()
    {
        hasScored = true;
    }
}
