using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadFacil()
    {
        SceneManager.LoadScene("jogo_nivel_1"); // Nome exato da cena f�cil
    }

    public void LoadMedio()
    {
        SceneManager.LoadScene("jogo_nivel_2"); // Nome exato da cena m�dia
    }

    public void LoadDificil()
    {
        SceneManager.LoadScene("jogo_nivel_3"); // Nome exato da cena dif�cil
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo."); // S� aparece no build
    }
}
