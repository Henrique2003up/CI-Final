using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 instance;

    public int lives = 3;
    public int score = 0;
    public int targetScore = 5;
    public int currentLevel = 1;

    public TMPro.TextMeshProUGUI pontosText;
    public Text nivelText;

    void Awake() => instance = this;

    void Start()
    {
        UpdateUI();
    }

    public void CollectGoodFruit()
    {
        score++;
        UpdateUI();
        CheckLevelProgress();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();
        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        pontosText.text = $"{score}";
        nivelText.text = $"NÍVEL {currentLevel}";
    }

    void CheckLevelProgress()
    {
        if (score >= targetScore)
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        currentLevel++;
        if (currentLevel > 3)
        {
            Debug.Log("Parabéns! Você venceu o jogo!");
            // Aqui podes carregar uma tela de vitória
            return;
        }

        // Atualiza metas por nível
        switch (currentLevel)
        {
            case 2: targetScore = 10; break;
            case 3: targetScore = 15; break;
        }

        score = 0;
        lives = 3;
        UpdateUI();
        Debug.Log($"Avançou para o nível {currentLevel}!");
    }

    void GameOver()
    {
        Debug.Log("Fim de Jogo!");
        // Aqui podes carregar uma tela de fim
    }
}
