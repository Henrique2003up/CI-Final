using UnityEngine;
using TMPro; // Necessário para TextMeshPro
using UnityEngine.SceneManagement; // Necessário para gerir cenas
using System.Collections; // Necessário para IEnumerator

// Este script gerencia a pontuação, condições de vitória e derrota,
// e carregamento de cenas. Deve estar presente em CADA CENA onde sua lógica é necessária.
public class GameManagerRed : MonoBehaviour
{
    [Header("Configurações do Nível Atual")]
    public int score = 0;
    public int targetScore = 10; // Meta de pontuação para ESTE NÍVEL.
    public int minScoreToLose = -5; // Pontuação mínima para perder ESTE NÍVEL.

    [Header("Referências da UI/Objetos Desta Cena")]
    public TextMeshProUGUI scoreText; // TextMeshProUGUI da pontuação DESTA CENA.
    public GameObject basketGameObject; // GameObject do cesto DESTA CENA.

    [Header("Nomes das Cenas de Destino")]
    // Nomes das cenas que este GameManager pode carregar.
    // Atribua no Inspector para CADA INSTÂNCIA deste script (em cada cena).
    public string winSceneName = "Parabéns"; // Cena de vitória para este nível.
    public string defeatSceneName = "Derrota"; // Cena de derrota.
    public string nextLevelSceneName = "Nivel2"; // Próximo nível (se este for um nível de jogo).
    public string mainMenuSceneName = "IntroJogo"; // Cena do menu principal.

    // REMOVIDO: Referências a painéis de derrota/vitória e animadores (se forem cenas completas).
    // Se ainda usa painéis com animadores, adicione as referências de volta aqui.
    // public GameObject defeatPanel;
    // private Animator defeatPanelAnimator;

    void Start()
    {
        score = 0; // Garante que a pontuação começa em 0 para CADA NOVO NÍVEL/CENA.
        UpdateScoreText();

        Time.timeScale = 1f; // Garante que o tempo do jogo está a correr.

        // REMOVIDO: Lógica de inicialização de painéis e animadores.
        // Se usar painéis, atribua-os e inicialize-os aqui.
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        CheckWinCondition();
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        UpdateScoreText();
        if (score < minScoreToLose)
        {
            Debug.Log("Você perdeu por pontuação baixa! Pontuação: " + score, this);
            GameOver();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "PONTOS: " + score + "/" + targetScore;
        }
        else
        {
            Debug.LogWarning("GameManagerRed: scoreText (UI TextMeshProUGUI) não atribuído no Inspector desta cena!");
        }
    }

    void CheckWinCondition()
    {
        if (score >= targetScore)
        {
            Debug.Log("GameManagerRed DEBUG: Condição de vitória atingida! Pontuação: " + score, this);
            WinGame();
        }
    }

    // Chamado pelas frutas podres quando colidem com o cesto.
    public void TriggerBasketDestroyedGameOver()
    {
        Debug.Log("GameManagerRed DEBUG: Cesto atingido por fruta podre! Chamando Game Over.", this);
        if (basketGameObject != null)
        {
            Destroy(basketGameObject);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: basketGameObject não atribuído nesta cena! Não é possível destruir o cesto.", this);
        }
        GameOver();
    }

    public void GameOver()
    {
        Debug.Log("GameManagerRed DEBUG: Função GameOver() chamada. Carregando cena de Derrota.", this);
        Time.timeScale = 1f; // Volta o tempo ao normal para que a nova cena carregue corretamente

        if (!string.IsNullOrEmpty(defeatSceneName))
        {
            SceneManager.LoadScene(defeatSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: defeatSceneName não definido nesta cena! Não é possível carregar a cena de derrota.", this);
        }
    }

    public void WinGame()
    {
        Debug.Log("GameManagerRed DEBUG: Função WinGame() chamada. Carregando cena de Vitória: " + winSceneName, this);
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(winSceneName))
        {
            SceneManager.LoadScene(winSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: winSceneName não definido nesta cena! Não é possível carregar a cena de vitória.", this);
        }
    }

    // --- MÉTODOS PÚBLICOS (NÃO ESTÁTICOS) PARA OS BOTÕES ---
    // Estes métodos serão ligados aos botões arrastando o GameObject deste GameManagerRed.
    public void RestartGame()
    {
        Debug.Log("GameManagerRed DEBUG: Reiniciando jogo para a cena atual: " + SceneManager.GetActiveScene().name, this);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Debug.Log("GameManagerRed DEBUG: Indo para o menu principal: " + mainMenuSceneName, this);
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: mainMenuSceneName não definido nesta cena! Não é possível carregar o menu principal.", this);
        }
    }

    public void LoadNextLevel()
    {
        Debug.Log("GameManagerRed DEBUG: Carregando o próximo nível: " + nextLevelSceneName, this);
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            SceneManager.LoadScene(nextLevelSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: nextLevelSceneName não definido nesta cena! Não é possível carregar o próximo nível.", this);
        }
    }
}
