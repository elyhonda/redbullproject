using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    public Slider progressBar; // Barra de carregamento
    public TMP_Text loadingText;  // Texto dinâmico ("Em Breve")
    public TMP_Text loadingStatus; // Texto abaixo ("Carregando...")
    public Image progressFill; // Imagem do preenchimento do Slider
    public CanvasGroup canvasGroup; // CanvasGroup para fade-out geral
    public Canvas canvas;
    public string sceneName;
    private string[] frases = {
        "Red Bull gives you wings!",
        "Need energy? Grab a Red Bull!",
        "Stay focused, stay energized!",
        "Red Bull: Powering champions!",
        "One sip, endless possibilities!"
    };

    private void Awake()
    {
        // Se já existe um LoadingScreen, destrói o novo
        if (Instance != null)
        {
            Destroy(canvas.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(canvas.gameObject); // Mantém o LoadingScreen entre cenas
    }

    void Start()
    {
        StartCoroutine(LoadGameScene());
        StartCoroutine(ChangeText());
    }

    IEnumerator LoadGameScene()
    {
        progressBar.value = 0; // Começa do zero

        yield return new WaitForSeconds(0.5f); // Pequena espera

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false; // Impede a ativação automática

        // Animação da barra de progresso enquanto a cena carrega
        while (loadOperation.progress < 0.9f)
        {
            progressBar.DOValue(loadOperation.progress, 0.5f).SetEase(Ease.OutQuad);
            yield return null;
        }

        // Preenche a barra até 100% e espera um pouco
        progressBar.DOValue(1f, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.5f);

        // Agora ativa a cena
        loadOperation.allowSceneActivation = true;

        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        yield return new WaitForSeconds(0.5f);

        UpdateCanvasCamera(); // Ajusta a câmera após a nova cena carregar

        canvasGroup.DOFade(0, 0.5f).OnComplete(() => Destroy(canvas.gameObject));
    }

    IEnumerator ChangeText()
    {
        while (true)
        {
            string frase = frases[Random.Range(0, frases.Length)];
            loadingText.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);

            loadingText.text = frase;
            loadingText.DOFade(1, 0.5f);
            yield return new WaitForSeconds(2f);
        }
    }
    private void UpdateCanvasCamera()
    {
        Camera mainCamera = Camera.main; // Pega a câmera principal da nova cena
        if (mainCamera != null && canvas != null)
        {
            canvas.worldCamera = mainCamera;
        }
    }
}