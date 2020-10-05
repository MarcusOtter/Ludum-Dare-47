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
        switch (GameManager.Instance.GetNextBugTypeToSpawn())
        {
            case BugType.Ant: return "AntsHighscore";
            case BugType.Ladybug: return "CaterpillarHighscore";
            case BugType.Caterpillar: return "LadyBirdsHighscore";
            case BugType.Random: return "RandomHighscore";
            default: return "";
        }
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
        }

        _highscoreText.text = $"Highscore: {GetHighscore()}";
    }

    private enum ScoreType
    {
        Highscore,
        HighscoreDuplicate,
        Nothing
    }
}
