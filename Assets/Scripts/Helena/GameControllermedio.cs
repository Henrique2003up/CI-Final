using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameControllermedio : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] boardButtons;
    public TextMeshProUGUI turnText;

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
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = "";
            if (boardButtons[i].image != null)
                boardButtons[i].image.sprite = emptySprite;

            boardButtons[i].interactable = true;
        }

        movesMade = 0;
        isPlayerMacaTurn = Random.value > 0.5f;

        if (isPlayerMacaTurn)
        {
            turnText.text = "É a vez do Jogador Maçã";
        }
        else
        {
            turnText.text = "É a vez do Jogador Brócolo";
            Invoke("MakeAIMove", 1f);
        }
    }

    public void OnBoardButtonClick(int index)
    {
        if (boardState[index] != "" || !boardButtons[index].interactable)
            return;

        string currentPlayerSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo";
        boardState[index] = currentPlayerSymbol;

        if (boardButtons[index].image != null)
            boardButtons[index].image.sprite = isPlayerMacaTurn ? macaSprite : brocoloSprite;

        boardButtons[index].interactable = false;
        movesMade++;

        if (CheckForWin())
        {
            if (isPlayerMacaTurn)
                SceneManager.LoadScene("VitoriaVerde");
            else
                SceneManager.LoadScene("DerrotaVerde");
        }
        else if (movesMade == 9)
        {
            SceneManager.LoadScene("EmpateVerde");
        }
        else
        {
            isPlayerMacaTurn = !isPlayerMacaTurn;

            if (!isPlayerMacaTurn)
            {
                turnText.text = "É a vez do Jogador Brócolo";
                Invoke("MakeAIMove", 0.6f);
            }
            else
            {
                turnText.text = "É a vez do Jogador Maçã";
            }
        }
    }

    void MakeAIMove()
    {
        // 1. Tentar ganhar
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
            {
                boardState[i] = "Brocolo";
                if (CheckForWinSimulation("Brocolo"))
                {
                    boardState[i] = "";
                    OnBoardButtonClick(i);
                    return;
                }
                boardState[i] = "";
            }
        }

        // 2. Bloquear o jogador
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
            {
                boardState[i] = "Maca";
                if (CheckForWinSimulation("Maca"))
                {
                    boardState[i] = "";
                    OnBoardButtonClick(i);
                    return;
                }
                boardState[i] = "";
            }
        }

        // 3. Jogada aleatória
        List<int> availableMoves = new List<int>();
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
            {
                availableMoves.Add(i);
            }
        }

        if (availableMoves.Count > 0)
        {
            int aiMove = availableMoves[Random.Range(0, availableMoves.Count)];
            OnBoardButtonClick(aiMove);
        }
    }

    bool CheckForWin()
    {
        string currentSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo";
        return CheckForWinSimulation(currentSymbol);
    }

    // Usado também pela IA para simular vitórias
    bool CheckForWinSimulation(string symbol)
    {
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (boardState[a] == symbol &&
                boardState[b] == symbol &&
                boardState[c] == symbol)
            {
                return true;
            }
        }
        return false;
    }
}
