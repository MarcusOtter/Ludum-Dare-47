using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PostGameHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _score, _highScore, _highScoreNotifier;



    private void Awake()
    {
        print(PlayerPrefs.GetInt("playerprefsentry"));
        GameManager.Instance.OnGameOver += EnablePanel;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= EnablePanel;
    }

    public void EnablePanel(int score)
    {
        gameObject.SetActive(true);
        _score.text = $"You scored: {score}!";

        string playerprefsentry = "";

        switch (GameManager.Instance.GetNextBugTypeToSpawn())
        {
            case BugType.Ant:
                playerprefsentry = "AntsHighscore";
                break;
            case BugType.Ladybug:
                playerprefsentry = "CaterpillarHighscore";
                break;
            case BugType.Caterpillar:
                playerprefsentry = "LadyBirdsHighscore";
                break;
            case BugType.Random:
                playerprefsentry = "RandomHighscore";
                break;
        }


        int oldHighScore = PlayerPrefs.GetInt(playerprefsentry);

        if (score >= PlayerPrefs.GetInt("playerprefsentry"))
        {
            _highScoreNotifier.gameObject.SetActive(true);
            PlayerPrefs.SetInt(playerprefsentry, score);
            PlayerPrefs.Save();
        }
        if (score == oldHighScore)
        {
            _highScoreNotifier.text = "Highscore Duplicated!";
        }
        if (score < oldHighScore)
        {
            _highScoreNotifier.gameObject.SetActive(false);
        }

        _highScore.text = $"Highscore: {PlayerPrefs.GetInt(playerprefsentry).ToString()}"; //for some reason this only ever displays the score you last achieved and it's like 6:30 am I'm going to bed


    }

}

