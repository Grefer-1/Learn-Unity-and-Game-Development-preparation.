using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakout_Brick : MonoBehaviour
{
    public int score { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score) => this.score = score;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        Breakout_GameManager.instance.AddScore(score);
    }
}
