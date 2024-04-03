using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private float elapsed;
    private float duration;
    private int side;

    void Start()
    {
        GenerateDurationAndSide();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        transform.position = Vector3.Lerp(new Vector3(8.3f, side * 3.1f, 0f), new Vector3(8.3f, side * -3.1f, 0f), elapsed / duration);
        if (elapsed / duration >= 1f)
        {
            elapsed = 0f;
            GenerateDurationAndSide();
        }
    }

    private void GenerateDurationAndSide() {
        duration = Random.Range(0.25f, 0.75f);
        if (side == 0)
            side = Random.value < 0.5f ? -1 : 1;
        else
            side = -side;
    }
}
