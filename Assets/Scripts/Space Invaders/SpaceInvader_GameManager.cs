using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpaceInvader_GameManager : MonoBehaviour
{
    public static SpaceInvader_GameManager Instance;

    private const string SPACE_INVADERS_HIGH_SCORE = "SpaceInvadersHighScore";

    [SerializeField] private float spawnOffsetX;
    [SerializeField] private float spawnOffsetY;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Color[] enemyColors;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Vector3 spawnStartPoint;
    [SerializeField] private Vector3[] corners;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private int score;
    private float elapsed;
    private float duration = 3f;
    private int lerpFirstIndex = 0;
    private int lerpSecondIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        int rowCount = 5;
        int enemyCount = 10;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject spawnedEnemy;
                if (i == 0)
                {
                    if (j == 0)
                        spawnedEnemy = Instantiate(enemy, spawnStartPoint, Quaternion.identity, spawnParent);
                    else
                        spawnedEnemy = Instantiate(enemy, spawnStartPoint + new Vector3(1f * j + spawnOffsetX * j, 0f, 0f), Quaternion.identity, spawnParent);
                    spawnedEnemy.SetActive(true);
                    spawnedEnemy.GetComponent<SpaceInvader_Enemy>().SetScore(rowCount - i);
                    spawnedEnemy.GetComponent<SpriteRenderer>().color = enemyColors[rowCount - 1 - i];
                    spawnedEnemy.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -180f));
                    spawnedEnemies.Add(spawnedEnemy);
                }
                else
                {
                    if (j == 0)
                        spawnedEnemy = Instantiate(enemy, spawnStartPoint + new Vector3(0f, spawnOffsetY * i, 0f), Quaternion.identity, spawnParent);
                    else
                        spawnedEnemy = Instantiate(enemy, spawnStartPoint + new Vector3(1f * j + spawnOffsetX * j, spawnOffsetY * i, 0f), Quaternion.identity, spawnParent);
                    spawnedEnemy.SetActive(true);
                    spawnedEnemy.GetComponent<SpaceInvader_Enemy>().SetScore(rowCount - i);
                    spawnedEnemy.GetComponent<SpriteRenderer>().color = enemyColors[rowCount - 1 - i];
                    spawnedEnemy.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -180f));
                    spawnedEnemies.Add(spawnedEnemy);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        spawnParent.transform.position = Vector3.Lerp(corners[lerpFirstIndex], corners[lerpSecondIndex], elapsed / duration);
        if (elapsed / duration >= 1f)
        {
            lerpFirstIndex = lerpFirstIndex >= corners.Length - 1 ? 0 : lerpFirstIndex + 1;
            lerpSecondIndex = lerpSecondIndex >= corners.Length - 1 ? 0 : lerpSecondIndex + 1;
            if (lerpFirstIndex == 1 && lerpSecondIndex == 2 || lerpFirstIndex == 3 && lerpSecondIndex == 0)
                duration /= 2;
            else
                duration *= 2;

            elapsed = 0;
        }
    }

    public void AddScore(int score)
    {
        this.score += score;
        if (PlayerPrefs.GetInt(SPACE_INVADERS_HIGH_SCORE) < this.score)
        {
            highScoreText.text = $"High Score: {this.score}";
            PlayerPrefs.SetInt(SPACE_INVADERS_HIGH_SCORE, this.score);
        }
        scoreText.text = $"Score: {this.score}";
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
    }
}
