using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ResultScreenAnimation : MonoBehaviour
{
    public RectTransform panelResultado;
    public TMP_Text tituloNivelCompleto;
    public Image[] estrelas;
    public RectTransform ranking;
    public RectTransform botoesContainer;
    public TMP_Text pontuacaoTexto;
    public Button[] botoes;
    public int pontuacaoFinal;

    void Start()
    {
        DOTween.Init();
        AnimarTela();
    }

    void AnimarTela()
    {
        // Zerando posições iniciais
        panelResultado.localScale = Vector3.zero;
        ranking.anchoredPosition = new Vector2(800, ranking.anchoredPosition.y);
        botoesContainer.localScale = Vector3.one;
        pontuacaoTexto.text = "0";

        // Ajusta título para iniciar invisível
        tituloNivelCompleto.color = new Color(1, 1, 1, 0);

        foreach (var estrela in estrelas)
        {
            estrela.gameObject.SetActive(true); // Garante que a estrela está ativa
            estrela.transform.localScale = Vector3.zero;
            estrela.color = new Color(estrela.color.r, estrela.color.g, estrela.color.b, 0);
            estrela.SetAllDirty(); // Atualiza a UI
        }

        foreach (var botao in botoes)
        {
            botao.gameObject.SetActive(true);
            botao.transform.localScale = Vector3.zero;
        }

        // Animação do painel principal
        panelResultado.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        // Animação do título "Nível Completo"
        tituloNivelCompleto.DOFade(1, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                tituloNivelCompleto.transform.DOShakePosition(0.5f, 10, 10)
                    .SetUpdate(true);
                tituloNivelCompleto.DOFade(0.7f, 0.7f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetUpdate(true);
            });

        // Animação das estrelas (fade-in + rotação + escala)
        for (int i = 0; i < estrelas.Length; i++)
        {
            estrelas[i].DOFade(1, 0.5f).SetDelay(0.2f * i).SetUpdate(true);
            estrelas[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f * i).SetUpdate(true);
            estrelas[i].transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutExpo)
                .SetDelay(0.2f * i)
                .SetUpdate(true);
        }

        // Ranking deslizando da direita
        ranking.DOAnchorPosX(0, 0.6f)
            .SetEase(Ease.OutExpo)
            .SetDelay(0.3f)
            .SetUpdate(true);

        // Animação da pontuação aumentando
        DOTween.To(() => 0, x => pontuacaoTexto.text = x.ToString(), pontuacaoFinal, 1.2f)
            .SetEase(Ease.OutQuad)
            .SetDelay(0.5f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pontuacaoTexto.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1).SetUpdate(true);
            });

        // Botões aparecendo em cascata com bounce
        for (int i = 0; i < botoes.Length; i++)
        {
            botoes[i].gameObject.SetActive(true);
            botoes[i].transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBounce)
                .SetDelay(0.8f + (i * 0.2f))
                .SetUpdate(true);
        }
    }
}