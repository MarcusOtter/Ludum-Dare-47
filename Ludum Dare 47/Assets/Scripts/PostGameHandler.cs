using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PostGameHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText, _highscoreText, _highScoreNotifier;
    private static int _highScore = 0;


    private void Awake()
    {

        _highScore = PlayerPrefs.GetInt(GetPlayerPrefsEntryString());
        GameManager.Instance.OnGameOver += EnablePanel;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= EnablePanel;
    }

    private string GetPlayerPrefsEntryString()
    {

        string playerprefsentry = "";

        switch (GameManager.Instance.GetGameMode())
        {
            case GameMode.Ants:
                playerprefsentry = "AntsHighscore";
                break;
            case GameMode.LadyBirds:
                playerprefsentry = "CaterpillarHighscore";
                break;
            case GameMode.Caterpillars:
                playerprefsentry = "LadyBirdsHighscore";
                break;
            case GameMode.Random:
                playerprefsentry = "RandomHighscore";
                break;
        }
        return playerprefsentry;
    }

    private int GetHighscore()
    {

        int returnInt = Mathf.Max(PlayerPrefs.GetInt(GetPlayerPrefsEntryString()), _highScore);
        return returnInt;
    }

    private ScoreType SetHighscore(int score)
    {
        if(score > GetHighscore())
        {
            return ScoreType.Highscore;
        }
        if(score == GetHighscore())
        {
            return ScoreType.HighscoreDuplicate;
        }
        return ScoreType.Nothing;
    }

    public void EnablePanel(int score)
    {
        gameObject.SetActive(true);
        _scoreText.text = $"You scored: {score}";


        switch(SetHighscore(score))
        {
            case ScoreType.Highscore:
                _highScoreNotifier.gameObject.SetActive(true);
                
                PlayerPrefs.SetInt(GetPlayerPrefsEntryString(), score);
                _highScore = score;
                PlayerPrefs.Save();
                
                break;

            case ScoreType.HighscoreDuplicate:
                _highScoreNotifier.gameObject.SetActive(true);
                _highScoreNotifier.text = "Highscore Duplicate!";
                break;

            default:
                break; 

        }


        _highscoreText.text = $"Highscore: {GetHighscore()}"; //for some reason this only ever displays the score you last achieved and it's like 6:30 am I'm going to bed


    }

    private enum ScoreType
    {
        Highscore,
        HighscoreDuplicate,
        Nothing
    }

}

