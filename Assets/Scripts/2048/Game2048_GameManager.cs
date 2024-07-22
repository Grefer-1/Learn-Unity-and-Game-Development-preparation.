using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game2048_GameManager : MonoBehaviour
{
    public Game2048_TileBoard TileBoard;
    public CanvasGroup CanvasGroup;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;

    private int Value;

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        HighScore.text = LoadHighScore().ToString();

        CanvasGroup.alpha = 0f;
        CanvasGroup.interactable = false;

        TileBoard.ClearBoard();
        TileBoard.CreateTile();
        TileBoard.CreateTile();
        TileBoard.enabled = true;
    }

    public void GameOver()
    {
        TileBoard.enabled = false;
        CanvasGroup.interactable = true;

        StartCoroutine(Fade(CanvasGroup, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float target, float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float current = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(current, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = target;
    }

    public void Scored(int value)
    {
        SetScore(Value + value);
    }

    private void SetScore(int score)
    {
        Value = score;
        Score.text = score.ToString();
    }

    private void SaveHighScore()
    {
        int value = LoadHighScore();
        if (Value > value)
            PlayerPrefs.SetInt("2048_HighScore", Value);
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("2048_HighScore", 0);
    }
}
