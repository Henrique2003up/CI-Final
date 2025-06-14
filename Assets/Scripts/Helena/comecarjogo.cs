using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nome ou índice da cena de jogo (confira em Build Settings)
    [SerializeField] private string jogo_escolher = "jogo_escolher";

    // Este método será chamado pelo botão "Começar"
    public void StartGame()
    {
        SceneManager.LoadScene(jogo_escolher);
    }
}
