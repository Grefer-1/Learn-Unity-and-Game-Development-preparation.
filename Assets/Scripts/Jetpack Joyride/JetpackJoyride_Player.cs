using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackJoyride_Player : MonoBehaviour
{
    [SerializeField] private float force = 350f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            rb.AddForce(Vector2.up * force, ForceMode2D.Force);

        if (transform.position.x > -7f)
            transform.position = new Vector3(-7f, 0, 0);

        if (transform.position.x < -10.7f)
            JetpackJoyride_GameManager.Instance.SetGameOver();
    }
}
