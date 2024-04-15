using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JetpackJoyride_GameManager : MonoBehaviour
{
    public static JetpackJoyride_GameManager Instance;

    private const string JETPACK_JOYRIDE_HIGH_SCORE = "JetpackJoyrideHighScore";

    [SerializeField] GameObject[] blocks;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] private float spawnTimer = 2f;

    private int score;
    private float spawnTime;
    private float scoreTime;
    private float scoreTimer = 0.25f;
    private float timeScaleRatio;
    public bool isGameOver { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        score = 0;
        scoreText.text = $"Score: {score}";
        highScoreText.text = $"High Score: {PlayerPrefs.GetInt(JETPACK_JOYRIDE_HIGH_SCORE)}";
        timeScaleRatio = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (spawnTime <= 0f)
            {
                spawnTime = spawnTimer;
                Instantiate(blocks[Random.Range(0, blocks.Length)], new Vector3(12f, Random.Range(-3.5f, 3.5f), 0f), Quaternion.identity);
            }
            else
                spawnTime -= Time.deltaTime;

            if (scoreTime <= 0f)
            {
                score++;
                if (PlayerPrefs.GetInt(JETPACK_JOYRIDE_HIGH_SCORE) < score)
                {
                    PlayerPrefs.SetInt(JETPACK_JOYRIDE_HIGH_SCORE, score);
                    highScoreText.text = $"High Score: {PlayerPrefs.GetInt(JETPACK_JOYRIDE_HIGH_SCORE)}";
                }
                scoreText.text = $"Score: {score}";
                scoreTime = scoreTimer;
            }
            else
                scoreTime -= Time.deltaTime;

            if (score >= timeScaleRatio * 150)
            {
                timeScaleRatio++;
                Time.timeScale = 1 + timeScaleRatio * 0.25f;
            }
        }
    }

    public void SetGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }
}
