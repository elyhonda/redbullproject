using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DG.Tweening;
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
    [SerializeField] private Slider comboSlider;
    [SerializeField] private RectTransform comboSliderRect;

    [Header("Ranking")]
    public UnityEvent<string, int> submitScoreEvent;
    public Leaderboard leaderboard;

    [Header("Sons")]
    public AudioClip clickButton, clickButton2;
    public AudioClip winSFX, winVoice;
    public AudioSource sfx;

    public ResultScreenAnimation resulSA;
    public LoadingScreen loadingScreen;

    private bool isStarted = false;
    private int lastWholeSecond;

    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 0.5f; // Tempo de exibição do feedback
    private int comboCount = 0;
    private float comboTimer = 0f;
    private float comboResetTime = 8f; 
    private bool isComboActive = false;
    void Start()
    {
        Cursor.visible = false;
        isStarted = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        Time.timeScale = 1;
        redbullCan = 0;
        timerText.text = timer.ToString("F2") + "/" + timerGoal.ToString("F2");
        canText.text = redbullCan.ToString() + "/" + redbullCanGoal; ToString();

        comboSlider.maxValue = comboResetTime;
        comboSlider.value = 0;
        comboSliderRect.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("F2") + "/" + timerGoal.ToString("F2");

            if ((int)timer > lastWholeSecond)
            {
                lastWholeSecond = (int)timer;
                AnimateTimer();
            }

            canText.text = redbullCan.ToString() + "/" + redbullCanGoal;
        }

        if (timer > timerGoal)
        {
            GameOver();
        }

        if (win == true)
        {
            GameWin();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
        
        if (isComboActive)
        {
            float timeLeft = comboResetTime - (Time.time - comboTimer);
            comboSlider.value = Mathf.Clamp(timeLeft, 0, comboResetTime);

            if (timeLeft <= 0)
            {
                comboSliderRect.DOScale(0, 0.3f).SetEase(Ease.InBack);
                comboCount = 0; // Reseta o combo
                isComboActive = false;
            }
        }
    }
    public void CollectRedbull()
    {
        redbullCan++;
        canText.text = redbullCan.ToString() + "/" + redbullCanGoal;

        // Atualiza combo
        if (Time.time - comboTimer < comboResetTime)
        {
            comboCount++;
        }
        else
        {
            comboCount = 1; // Reseta se passou muito tempo
        }
        comboTimer = Time.time; // Atualiza o tempo da última coleta

        string feedbackMessage;
        Color feedbackColor;

        switch (comboCount)
        {
            case 1:
                feedbackMessage = "Perfect!";
                feedbackColor = Color.white; // Branco (Neutro)
                break;
            case 2:
                feedbackMessage = "2x Turbo Boost!";
                feedbackColor = Color.white; // Azul Red Bull
                break;
            case 3:
                feedbackMessage = "3x Max Energy!";
                feedbackColor = Color.white; // Amarelo Red Bull
                break;
            case 4:
                feedbackMessage = "4x Full Power!";
                feedbackColor = new Color(1f, 0f, 0f); // Vermelho Red Bull
                break;
            default:
                feedbackMessage = $"{comboCount}x Overdrive!";
                feedbackColor = new Color(1f, 0f, 0f); // Vermelho intenso para combos altos
                break;
        }
        ShowFeedback(feedbackMessage, feedbackColor);
        
        if (!isComboActive)
        {
            comboSliderRect.DOScale(1, 0.3f).SetEase(Ease.OutBack); // Animação de escala
            isComboActive = true;
        }
        comboSlider.value = comboResetTime; 
    }
    private void ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.transform.localScale = Vector3.one * 0.5f; // Começa pequeno

        feedbackText.DOFade(1, 0.2f); // Aparece rapidamente
        feedbackText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack); // Expande com bounce
    }

    private void AnimateTimer()
    {
        // Animação de pulso no timer
        timerText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 8, 1);
    }
    public void PlayGame()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        isStarted = true;
    }

    public void GameOver()
    {
        Cursor.visible = true;
        Debug.Log("perdeu");
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    public void Restart()
    {
        Cursor.visible = true;
        LoadingScene("Nivel" + level);
        Time.timeScale = 1;
        sfx.PlayOneShot(clickButton);
    }

    public void Menu()
    {
        Cursor.visible = true;
        LoadingScene("Menu");
        Time.timeScale = 1;
        sfx.PlayOneShot(clickButton);
    }

    public void NextLevel()
    {
        Cursor.visible = true;
        LoadingScene("Nivel" + (level + 1));
        Time.timeScale = 1;
        sfx.PlayOneShot(clickButton);
    }

    public void GameWin()
    {
        Cursor.visible = true;
        sfx.PlayOneShot(winSFX);
        sfx.PlayOneShot(winVoice);
        Debug.Log("ganhou");
        Time.timeScale = 0;
        scoreFinal = (redbullCan * 100) + ((timerGoal - timer) * 1000);
        resulSA.pontuacaoFinal = Mathf.RoundToInt(scoreFinal);
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

    public void LoadingScene(string sceneName)
    {
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.sceneName = sceneName;
    }
}
