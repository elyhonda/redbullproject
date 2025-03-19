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
    [SerializeField] private GameObject player;
    public static int redbullCan = 0;
    public static bool win = false;

    [Header("UI")]
    [SerializeField] private float scoreFinal;
    [SerializeField] private TMP_Text scoreFinalText, scoreTimerText, scoreCanText, highscoreText;
    [SerializeField] private GameObject ranking;
    [SerializeField] private GameObject notifyNewScore;
    [SerializeField] private GameObject gameOver;

    [Header("Ranking")]
    public UnityEvent<string, int> submitScoreEvent;
    public Leaderboard leaderboard;

    [Header("Sons")]
    public AudioClip clickButton, clickButton2;
    public AudioClip winSFX, winVoice;
    public AudioSource sfx;


    private bool isStarted = false;
    void Start()
    {
        Cursor.visible = false;
        isStarted = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        Time.timeScale = 1;
        redbullCan = 0;
        timerText.text = timer.ToString("F2") + "/" + timerGoal.ToString("F2");
        canText.text = redbullCan.ToString() + "/" + redbullCanGoal; ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("F2") + "/" + timerGoal.ToString("F2");
            canText.text = redbullCan.ToString() + "/" + redbullCanGoal; ToString();
        }

        if (timer > timerGoal)
        {
            GameOver();
        }

        if (win == true)
        {
            GameWin();
        }
    }

    public void PlayGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        isStarted = true;
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
        sfx.PlayOneShot(clickButton);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
        sfx.PlayOneShot(clickButton);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Nivel" + (level + 1));
        Time.timeScale = 1;
        sfx.PlayOneShot(clickButton);
    }

    public void GameWin()
    {
        sfx.PlayOneShot(winSFX);
        sfx.PlayOneShot(winVoice);
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
