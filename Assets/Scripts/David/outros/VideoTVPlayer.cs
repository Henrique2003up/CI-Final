using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTVPlayer : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painelVideo;
    public Button botaoTV;
    public Button botaoFechar;
    public VideoPlayer videoPlayer;
    public Animator tvAnimator;

    // Ajuste esses tempos para a duração das suas animações ZoomIn e ZoomOut
    private float duracaoZoomIn = 0.4f;
    private float duracaoZoomOut = 0.5f;

    void Start()
    {
        painelVideo.SetActive(false);
        tvAnimator.enabled = true;  // Mantém o Animator ativado para responder aos triggers

        botaoTV.onClick.AddListener(AbrirVideo);
        botaoFechar.onClick.AddListener(FecharVideo);
    }

    void AbrirVideo()
    {
        tvAnimator.SetTrigger("zoomin");

        // Mostra o vídeo após a animação ZoomIn finalizar
        Invoke(nameof(MostrarVideo), duracaoZoomIn);
    }

    void MostrarVideo()
    {
        painelVideo.SetActive(true);
        videoPlayer.Play();
    }

    void FecharVideo()
    {
        videoPlayer.Stop();
        painelVideo.SetActive(false);

        tvAnimator.SetTrigger("zoomout");

        // Desliga o Animator (ou faça o que for necessário) após ZoomOut
        Invoke(nameof(DesligarAnimator), duracaoZoomOut);
    }

    void DesligarAnimator()
    {
        // Se quiser manter o Animator ativo para próximas interações, pode comentar essa linha
        // tvAnimator.enabled = false;
    }
}
