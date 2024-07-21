using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvader_Bullet : MonoBehaviour
{
    private int side;

    // Start is called before the first frame update
    void Start()
    {
        side = transform.parent.name == "Player" ? 1 : -1;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, side * 7.5f * Time.deltaTime, 0f);

        if (transform.position.y > 5.25f || transform.position.y < -6.15f)
            Destroy(gameObject);
    }
}
