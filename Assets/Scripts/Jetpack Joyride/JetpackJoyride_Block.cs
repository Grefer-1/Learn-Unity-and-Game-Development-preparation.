using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackJoyride_Block : MonoBehaviour
{
    private enum Mode
    {
        None,
        Lerping,
    }

    private int side;
    private Mode mode;
    private float elapsed;
    private float speed = 3f;
    private float lerpDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        mode = Random.value <= 0.5f ? Mode.None : Mode.Lerping;
        FlipSide();
        elapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;

        if (gameObject.name.Contains("Plus"))
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, side * 3f, 0f), new Vector3(transform.position.x, side * -3f, 0f), elapsed / lerpDuration);
            transform.eulerAngles += Vector3.forward;
            if (elapsed / lerpDuration >= 1f)
                FlipSide();
        }
        else
        {
            switch (mode)
            {
                case Mode.None:
                    break;
                case Mode.Lerping:
                    elapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(new Vector3(transform.position.x, side * 4.5f, 0f), new Vector3(transform.position.x, side * -4.5f, 0f), elapsed / lerpDuration);
                    if (elapsed / lerpDuration >= 1f)
                        FlipSide();
                    break;
            }
        }

        if (transform.position.x < -10.5f)
            Destroy(gameObject);
    }

    private void FlipSide()
    {
        if (side == 0)
            side = Random.value < 0.5f ? -1 : 1;
        else
            side = -side;

        elapsed = 0f;
    }
}
