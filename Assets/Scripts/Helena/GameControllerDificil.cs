using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameControllerDificil : MonoBehaviour
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
            Invoke("MakeAIMove", 0.6f);
    }

    void MakeAIMove()
    {
        int move = FindBestMove();
        if (move != -1)
        {
            MakeMove(move, "Brocolo", brocoloSprite);
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

    int FindBestMove()
    {
        // 1. Tenta vencer
        int winMove = FindCriticalMove("Brocolo");
        if (winMove != -1) return winMove;

        // 2. Bloqueia o jogador
        int blockMove = FindCriticalMove("Maca");
        if (blockMove != -1) return blockMove;

        // 3. Prioridade: centro
        if (boardState[4] == "") return 4;

        // 4. Canto estratégico
        int[] corners = { 0, 2, 6, 8 };
        foreach (int i in corners)
            if (boardState[i] == "") return i;

        // 5. Laterais
        int[] sides = { 1, 3, 5, 7 };
        foreach (int i in sides)
            if (boardState[i] == "") return i;

        return -1; // Não encontrou jogada possível (deveria ser empate)
    }

    int FindCriticalMove(string symbol)
    {
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (boardState[a] == symbol && boardState[b] == symbol && boardState[c] == "") return c;
            if (boardState[a] == symbol && boardState[c] == symbol && boardState[b] == "") return b;
            if (boardState[b] == symbol && boardState[c] == symbol && boardState[a] == "") return a;
        }
        return -1;
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
