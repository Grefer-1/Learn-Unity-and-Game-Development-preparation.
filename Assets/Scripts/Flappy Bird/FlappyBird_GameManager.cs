using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlappyBird_GameManager : MonoBehaviour
{
    public static FlappyBird_GameManager Instance;

    [SerializeField] private GameObject tunnelPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FlappyBird_Birb birb;
    [SerializeField] private float spawnTimer = 3f;

    private int score;
    private float time;
    private float[] tunnelOffsetNums = new float[] { 0f, 0.5f, 1, 1.5f, 2f, 2.5f};

    public bool isGameOver { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (time <= 0f)
            {
                time = spawnTimer;
                int offsetUporDown = UnityEngine.Random.value <= 0.5 ? -1 : 1;
                FlappyBird_Tunnel tunnel = Instantiate(tunnelPrefab, new Vector3(12f, offsetUporDown * tunnelOffsetNums[UnityEngine.Random.Range(0, tunnelOffsetNums.Length)], 0f), Quaternion.identity).GetComponent<FlappyBird_Tunnel>();
                tunnel.OnScored += Tunnel_OnScored;
            }
            else
                time -= Time.deltaTime;
        }
    }

    private void Tunnel_OnScored(object sender, System.EventArgs e)
    {
        FlappyBird_Tunnel tunnel = sender as FlappyBird_Tunnel;
        if (!tunnel.hasScored)
        {
            tunnel.Scored();
            score++;
            scoreText.text = $"{score}";
            tunnel.OnScored -= Tunnel_OnScored;
        }
    }

    public void SetGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }
}
