using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nome ou �ndice da cena de jogo (confira em Build Settings)
    [SerializeField] private string jogo_escolher = "jogo_escolher";

    // Este m�todo ser� chamado pelo bot�o "Come�ar"
    public void StartGame()
    {
        SceneManager.LoadScene(jogo_escolher);
    }
}
