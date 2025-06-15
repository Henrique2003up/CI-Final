using UnityEngine;
using TMPro; // Necessário para TextMeshPro
using UnityEngine.SceneManagement; // Necessário para gerir cenas
using System.Collections; // Necessário para IEnumerator

// Este script gerencia a pontuação, condições de vitória e transições de nível,
// e agora o Game Over quando o cesto é destruído por uma fruta podre.
public class GameManagerRed : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 10;
    public int minScoreToLose = -5;

    public TextMeshProUGUI scoreText;

    public string nextLevelSceneName = "Level2";
    public string mainMenuSceneName = "MainMenu";

    public GameObject defeatPanel;
    private Animator defeatPanelAnimator;
    public GameObject basketGameObject;

    void Start()
    {
        score = 0;
        UpdateScoreText();

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
            defeatPanelAnimator = defeatPanel.GetComponent<Animator>();
            if (defeatPanelAnimator == null)
            {
                Debug.LogError("GameManagerRed ERROR: Animator NÃO ENCONTRADO no defeatPanel! Por favor, adicione um Animator ao GameObject 'Derrota'.");
            }
            else
            {
                Debug.Log("GameManagerRed DEBUG: Animator encontrado no defeatPanel ao iniciar.");
            }
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: 'Defeat Panel' não atribuído no Inspector! O painel de derrota não vai aparecer.");
        }

        Time.timeScale = 1f;
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
            Debug.Log("Você perdeu por pontuação baixa!");
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

    void CheckWinCondition()
    {
        if (score >= targetScore)
        {
            Debug.Log("GameManagerRed DEBUG: Condição de vitória atingida! Pontuação: " + score);
            StartCoroutine(DelayedLoadNextLevel(2f));
        }
    }

    IEnumerator DelayedLoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
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
            Debug.Log("GameManagerRed DEBUG: defeatPanel existe. Ativando... (Current active state: " + defeatPanel.activeSelf + ")");
            defeatPanel.SetActive(true); // Ativa o painel.
            Debug.Log("GameManagerRed DEBUG: defeatPanel ativado. (New active state: " + defeatPanel.activeSelf + ")");

            if (defeatPanelAnimator != null)
            {
                // --- MUDANÇA AQUI: Usa Animator.Play() para forçar a animação ---
                Debug.Log("GameManagerRed DEBUG: Animator encontrado. Forçando animação 'fallin'.");
                defeatPanelAnimator.Play("fallin"); // Força a reprodução da animação "fallin" pelo nome
                // ----------------------------------------------------------------
            }
            else
            {
                Debug.LogError("GameManagerRed ERROR: 'defeatPanelAnimator' é NULL! Não é possível tocar a animação. Verifique atribuição do Animator.");
            }
        }
        else
        {
            Debug.LogError("GameManagerRed ERROR: 'Defeat Panel' (GameObject) não atribuído no Inspector! O painel de derrota não vai aparecer.");
        }
    }

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
}
