using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Level levelScriptable;
    [SerializeField] public int level;

    [Header("Gameplay")]
    [SerializeField] private float timer, timerGoal;
    [SerializeField] private int redbullCanGoal;
    [SerializeField] private TMP_Text timerText, canText;
    public static int redbullCan = 0;
    public static bool win = false;

    [Header("UI")]
    [SerializeField] private float scoreFinal;
    [SerializeField] private TMP_Text scoreFinalText, scoreTimerText, scoreCanText, highscoreText;
    [SerializeField] private GameObject ranking;
    [SerializeField] private GameObject notifyNewScore;
    [SerializeField] private GameObject gameOver;

    public UnityEvent<string, int> submitScoreEvent;
    public Leaderboard leaderboard;


    void Start()
    {
        Time.timeScale = 1;
        redbullCan = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = timer.ToString("F2") + "/" + timerGoal.ToString("F2");
        canText.text = redbullCan.ToString() + "/" + redbullCanGoal; ToString();

        //if(Input.GetButtonDown("r"))
        if (timer > timerGoal)
        {
            GameOver();
        }

        if (win == true)
        {
            GameWin();
        }
    }

    public void GameOver()
    {
        Debug.Log("perdeu");
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Nivel" + level);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Nivel" + (level + 1));
        Time.timeScale = 1;
    }

    public void GameWin()
    {
        Debug.Log("ganhou");
        Time.timeScale = 0;
        scoreFinal = (redbullCan * 100) + ((timerGoal - timer) * 1000);
        scoreCanText.text = redbullCan.ToString() + "/" + redbullCanGoal.ToString();
        scoreTimerText.text = timer.ToString("F2") + "/" + timerGoal.ToString();
        scoreFinalText.text = Mathf.RoundToInt(scoreFinal).ToString();
        SubmitScore();
        if(PlayerPrefs.GetInt("Nivel" + level) == null || PlayerPrefs.GetInt("Nivel" + level) < Mathf.RoundToInt(scoreFinal))
        {
            PlayerPrefs.SetInt("Nivel" + level, Mathf.RoundToInt(scoreFinal));
            notifyNewScore.SetActive(true);
        }
        highscoreText.text = PlayerPrefs.GetInt("Nivel" + level).ToString();
        ranking.SetActive(true);
    }

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(PlayerPrefs.GetString("Name"), int.Parse(scoreFinalText.text));
        leaderboard.GetLeaderboard();
    }

}
