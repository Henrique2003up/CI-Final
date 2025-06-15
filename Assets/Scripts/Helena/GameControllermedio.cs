using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameControllerMedio : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] boardButtons;
    public TextMeshProUGUI turnText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    [Header("Game Images")]
    public Sprite emptySprite;
    public Sprite macaSprite;
    public Sprite brocoloSprite;

    [Header("Game State")]
    private string[] boardState;
    private bool isPlayerMacaTurn;
    private int movesMade;

    private int[,] winConditions = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
        {0, 4, 8}, {2, 4, 6}
    };

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        boardState = new string[9];
        for (int i = 0; i < 9; i++)
        {
            boardState[i] = "";
            boardButtons[i].image.sprite = emptySprite;
            boardButtons[i].interactable = true;
        }

        isPlayerMacaTurn = true;
        turnText.text = "É a vez do Jogador Maca";
        movesMade = 0;
        gameOverPanel.SetActive(false);
    }

    public void OnBoardButtonClick(int index)
    {
        if (boardState[index] != "" || !isPlayerMacaTurn || gameOverPanel.activeSelf)
            return;

        MakeMove(index, "Maca", macaSprite);
        isPlayerMacaTurn = false;

        if (!gameOverPanel.activeSelf)
            Invoke("MakeAIMove", 0.8f); // Aguardar antes da IA jogar
    }

    void MakeAIMove()
    {
        int bestMove = FindBlockingMove();

        if (bestMove == -1)
        {
            // Faz jogada aleatória
            List<int> emptyIndices = new List<int>();
            for (int i = 0; i < 9; i++)
                if (boardState[i] == "") emptyIndices.Add(i);

            if (emptyIndices.Count > 0)
                bestMove = emptyIndices[Random.Range(0, emptyIndices.Count)];
        }

        if (bestMove != -1)
        {
            MakeMove(bestMove, "Brocolo", brocoloSprite);
            isPlayerMacaTurn = true;
        }
    }

    void MakeMove(int index, string player, Sprite sprite)
    {
        boardState[index] = player;
        boardButtons[index].image.sprite = sprite;
        boardButtons[index].interactable = false;
        movesMade++;

        if (CheckForWin(player))
        {
            EndGame(player + " venceu!");
        }
        else if (movesMade == 9)
        {
            EndGame("Empate!");
        }
        else
        {
            turnText.text = "É a vez do Jogador " + (isPlayerMacaTurn ? "Maca" : "Brocolo");
        }
    }

    int FindBlockingMove()
    {
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (boardState[a] == "Maca" && boardState[b] == "Maca" && boardState[c] == "")
                return c;
            if (boardState[a] == "Maca" && boardState[c] == "Maca" && boardState[b] == "")
                return b;
            if (boardState[b] == "Maca" && boardState[c] == "Maca" && boardState[a] == "")
                return a;
        }
        return -1; // Nenhum bloqueio necessário
    }

    bool CheckForWin(string symbol)
    {
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (boardState[a] == symbol && boardState[b] == symbol && boardState[c] == symbol)
                return true;
        }
        return false;
    }

    void EndGame(string message)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = message;

        foreach (Button btn in boardButtons)
            btn.interactable = false;
    }

    public void RestartGame()
    {
        InitializeGame();
    }
}
