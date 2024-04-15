using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Breakout_GameManager : MonoBehaviour
{
    public static Breakout_GameManager instance;

    private const string BREAKOUT_HIGH_SCORE = "BreakOutHighScore";

    [SerializeField] private float spawnOffsetX;
    [SerializeField] private float spawnOffsetY;
    [SerializeField] private GameObject ball;
    [SerializeField] private Color[] scoreColors;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private GameObject defaultBrick;
    [SerializeField] private Vector3 spawnStartPoint;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private int lifes = 3;
    private int currLifeCounts;
    private int prevLifeCounts;
    private bool isResetting;
    private float lifeDelayTimer;
    private float lifeLostDelay = 1.5f;
    private List<GameObject> spawnedBricks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        currLifeCounts = prevLifeCounts = lifes = 3;
        lifeDelayTimer = lifeLostDelay;
        lifeText.text = $"Lifes left: {lifes}";
        bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt(BREAKOUT_HIGH_SCORE)}";
        int rowCount = 8;
        int brickCount = 16;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < brickCount; j++)
            {
                GameObject spawnedBrick;
                if (i == 0)
                {
                    if (j == 0)
                        spawnedBrick = Instantiate(defaultBrick, spawnStartPoint, Quaternion.identity, spawnParent);
                    else
                        spawnedBrick = Instantiate(defaultBrick, spawnStartPoint + new Vector3(1f * j + spawnOffsetX * j, 0f, 0f) , Quaternion.identity, spawnParent);
                    spawnedBrick.SetActive(true);
                    spawnedBrick.GetComponent<Breakout_Brick>().SetScore(8 - i);
                    spawnedBrick.GetComponent<SpriteRenderer>().color = scoreColors[8 - 1 - i];
                    spawnedBricks.Add(spawnedBrick);
                }
                else
                {
                    if (j == 0)
                        spawnedBrick = Instantiate(defaultBrick, spawnStartPoint + new Vector3(0f, spawnOffsetY * i, 0f), Quaternion.identity, spawnParent);
                    else
                        spawnedBrick = Instantiate(defaultBrick, spawnStartPoint + new Vector3(1f * j + spawnOffsetX * j, spawnOffsetY * i, 0f), Quaternion.identity, spawnParent);
                    spawnedBrick.SetActive(true);
                    spawnedBrick.GetComponent<Breakout_Brick>().SetScore(8 - i);
                    spawnedBrick.GetComponent<SpriteRenderer>().color = scoreColors[8 - 1 - i];
                    spawnedBricks.Add(spawnedBrick);
                }
            }
        }
    }

    void Update()
    {
        if (!isResetting && currLifeCounts == 3 && prevLifeCounts == 3)
        {
            if (ball.transform.position == Vector3.zero && ball.GetComponent<Rigidbody2D>().velocity == Vector2.zero && Input.anyKey)
                ball.GetComponent<Breakout_Ball>().ApplyForce();
        }

        if (!isResetting && currLifeCounts != prevLifeCounts)
        {
            lifeDelayTimer -= Time.deltaTime;
            if (lifeDelayTimer <= 0)
            {
                if (ball.transform.position == Vector3.zero && ball.GetComponent<Rigidbody2D>().velocity == Vector2.zero && Input.anyKey)
                {
                    ball.GetComponent<Breakout_Ball>().ApplyForce();
                    lifeDelayTimer = lifeLostDelay;
                    prevLifeCounts = currLifeCounts;
                }
            }
        }

        if (ball.transform.position.y <= -6f)
        {
            if (currLifeCounts == 0)
                Time.timeScale = 0;
            currLifeCounts--;
            lifeText.text = $"Lifes left: {currLifeCounts}";
            ball.transform.position = Vector3.zero;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void AddScore(int score)
    {
        int currScore = int.Parse(scoreText.text);
        currScore += score;
        scoreText.text = $"{currScore}";

        if (CheckBrickActiveState())
        {
            ball.transform.position = Vector3.zero;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke(nameof(ResetStage), 2f);
        }

        if (currScore > PlayerPrefs.GetInt(BREAKOUT_HIGH_SCORE))
        {
            PlayerPrefs.SetInt(BREAKOUT_HIGH_SCORE, currScore);
            bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt(BREAKOUT_HIGH_SCORE)}";
        }
    }

    private void ResetStage()
    {
        foreach (GameObject go in spawnedBricks)
            go.SetActive(true);

        isResetting = true;
    }

    private bool CheckBrickActiveState()
    {
        if (spawnedBricks.Any(go => go.activeSelf == true))
            return false;
        return true;
    }
}
