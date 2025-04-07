using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class DefeatScreenAnimation : MonoBehaviour
{
    public RectTransform panelDerrota;
    public TMP_Text tituloGameOver;
    public RectTransform botoesContainer;
    public Button[] botoes;
    public Image fundoVermelho;

    void Start()
    {
        DOTween.Init();
        AnimarDerrota();
    }

    void AnimarDerrota()
    {
        // Inicializações
        panelDerrota.localScale = Vector3.zero;
        botoesContainer.localScale = Vector3.one;
        tituloGameOver.color = new Color(1, 1, 1, 0);

        foreach (var botao in botoes)
        {
            botao.gameObject.SetActive(true);
            botao.transform.localScale = Vector3.zero;
        }

        // Painel vermelho com fade-in e efeito pulsante
        fundoVermelho.color = new Color(1, 0, 0, 0); // transparente
        fundoVermelho.DOFade(0.3f, 0.5f)
            .SetEase(Ease.OutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                fundoVermelho.DOFade(0.4f, 0.6f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetUpdate(true);
            });

        // Painel principal aparece
        panelDerrota.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        // Título "Game Over" com fade e tremor
        tituloGameOver.DOFade(1, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetDelay(0.2f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                tituloGameOver.transform.DOShakeScale(0.6f, 0.4f, 10, 90)
                    .SetUpdate(true);
                tituloGameOver.DOFade(0.7f, 0.8f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetUpdate(true);
            });

        // Botões (Home, Retry) com bounce em cascata
        for (int i = 0; i < botoes.Length; i++)
        {
            botoes[i].transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBounce)
                .SetDelay(0.6f + (i * 0.2f))
                .SetUpdate(true);
        }
    }
}