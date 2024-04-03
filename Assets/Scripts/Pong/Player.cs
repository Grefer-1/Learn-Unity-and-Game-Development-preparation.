using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
        }
    }
}
