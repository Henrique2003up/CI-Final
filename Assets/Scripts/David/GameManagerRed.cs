using UnityEngine;
using TMPro; // Necessário para TextMeshPro
using UnityEngine.SceneManagement; // Necessário para gerir cenas
using System.Collections; // Necessário para IEnumerator

// Este script gerencia a pontuação, condições de vitória e transições de nível,
// e agora o Game Over quando o cesto é destruído por uma fruta podre, e a condição de vitoria.
public class GameManagerRed : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 10; // Pontuação necessária para vencer o nível.
    public int minScoreToLose = -5;

    public TextMeshProUGUI scoreText;

    public string nextLevelSceneName = "Nivel2"; // Nome da cena do próximo nível
    public string mainMenuSceneName = "IntroJogo"; // Nome da cena do menu principal

    public GameObject defeatPanel;
    private Animator defeatPanelAnimator;
    public GameObject basketGameObject;

    // --- NOVIDADE: Referências para o painel de Vitoria ---
    public GameObject winPanel; // Arraste o seu GameObject do painel de vitória aqui no Inspector.
    private Animator winPanelAnimator;
    // --------------------------------------------------------

    void Start()
    {
        score = 0;
        UpdateScoreText();

        // Configuração do Painel de Derrota
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
            defeatPanelAnimator = defeatPanel.GetComponent<Animator>();
            if (defeatPanelAnimator == null)
            {
                Debug.LogError("GameManagerRed ERROR: Animator NÃO ENCONTRADO no defeatPanel! Adicione um Animator ao GameObject 'Derrota'.");
            }
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: 'Defeat Panel' não atribuído no Inspector! O painel de derrota não vai aparecer.");
        }

        // --- Configuração do Painel de Vitoria ---
        if (winPanel != null)
        {
            winPanel.SetActive(false); // Garante que o painel de vitória está desativado no início.
            winPanelAnimator = winPanel.GetComponent<Animator>();
            if (winPanelAnimator == null)
            {
                Debug.LogError("GameManagerRed ERROR: Animator NÃO ENCONTRADO no winPanel! Adicione um Animator ao GameObject do painel de vitória.");
            }
        }
        else
        {
            Debug.LogWarning("GameManagerRed WARNING: 'Win Panel' não atribuído no Inspector! A funcionalidade de vitória pode não ser completa.");
        }
        // -----------------------------------------------------

        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        CheckWinCondition(); // Verifica a condição de vitória após adicionar pontos.
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        UpdateScoreText();
        if (score < minScoreToLose)
        {
            Debug.Log("Você perdeu por pontuação baixa!");
            GameOver(); // Chama a função de Game Over aqui.
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
            Debug.LogWarning("GameManagerRed: scoreText (UI TextMeshProUGUI) não atribuído no Inspector!");
        }
    }

    // --- MUDANÇA: Agora chama WinGame() em vez de carregar o próximo nível diretamente ---
    void CheckWinCondition()
    {
        if (score >= targetScore)
        {
            Debug.Log("GameManagerRed DEBUG: Condição de vitória atingida! Pontuação: " + score);
            WinGame(); // Chama a nova função para lidar com a vitória.
        }
    }
    // ---------------------------------------------------------------------------------

    // Corrotina para atrasar o carregamento da próxima cena.
    // Usada por botões, não por auto-transição
    IEnumerator DelayedLoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            Time.timeScale = 1f; // Garante que o tempo do jogo está a correr para a nova cena
            SceneManager.LoadScene(nextLevelSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: nextLevelSceneName não definido! Não é possível carregar o próximo nível.");
        }
    }

    public void TriggerBasketDestroyedGameOver()
    {
        Debug.Log("GameManagerRed DEBUG: Cesto atingido por fruta podre! Chamando Game Over.");
        if (basketGameObject != null)
        {
            Destroy(basketGameObject);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: basketGameObject não atribuído! Não é possível destruir o cesto.");
        }
        GameOver();
    }

    public void GameOver()
    {
        Debug.Log("GameManagerRed DEBUG: Função GameOver() chamada. Pausando jogo e ativando painel.");
        Time.timeScale = 0f; // Pausa o jogo (tudo para de se mover).

        if (defeatPanel != null)
        {
            Debug.Log("GameManagerRed DEBUG: defeatPanel existe. Ativando...");
            defeatPanel.SetActive(true); // Ativa o painel.

            if (defeatPanelAnimator != null)
            {
                Debug.Log("GameManagerRed DEBUG: Animator de Derrota encontrado. Forçando animação 'Derrota_QuedaFinal'.");
                defeatPanelAnimator.Play("Derrota_QuedaFinal"); // Use o nome do seu clip de animação de derrota
            }
            else
            {
                Debug.LogError("GameManagerRed ERROR: 'defeatPanelAnimator' é NULL! Não é possível tocar a animação de derrota.");
            }
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: 'Defeat Panel' (GameObject) não atribuído no Inspector! O painel de derrota não vai aparecer.");
        }
    }

    // --- NOVIDADE: Método chamado quando o jogador vence o jogo ---
    public void WinGame()
    {
        Debug.Log("GameManagerRed DEBUG: Função WinGame() chamada. Vitoria!");
        Time.timeScale = 0f; // Pausa o jogo.

        if (winPanel != null)
        {
            Debug.Log("GameManagerRed DEBUG: winPanel existe. Ativando...");
            winPanel.SetActive(true); // Ativa o painel de vitória.

            if (winPanelAnimator != null)
            {
                Debug.Log("GameManagerRed DEBUG: Animator de Vitoria encontrado. Forçando animação 'Vitoria_QuedaFinal'.");
                winPanelAnimator.Play("Vitoria_QuedaFinal"); // Nome do clip de animação de vitória.
            }
            else
            {
                Debug.LogError("GameManagerRed ERROR: 'winPanelAnimator' é NULL! Não é possível tocar a animação de vitória.");
            }
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: 'Win Panel' (GameObject) não atribuído no Inspector! O painel de vitória não vai aparecer.");
        }

        // REMOVIDO: StartCoroutine(DelayedLoadNextLevel(3f));
        // A transição será feita pelos botões no painel de vitória.
    }
    // ---------------------------------------------------------------------------------

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: mainMenuSceneName não definido! Não é possível carregar o menu principal.");
        }
    }

    // --- NOVIDADE: Função para Carregar o Próximo Nível (para ser ligada a um botão) ---
    public void LoadNextLevelDelayed(float delay)
    {
        StartCoroutine(DelayedLoadNextLevel(delay));
    }
    // ----------------------------------------------------------------------------------
}
