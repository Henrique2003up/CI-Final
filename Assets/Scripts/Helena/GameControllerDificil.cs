using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameControllerDificil : MonoBehaviour
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
                SceneManager.LoadScene("ParabensVerde");
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
        int move = FindBestMoveForAI();

        if (move != -1)
        {
            OnBoardButtonClick(move);
        }
    }

    int FindBestMoveForAI()
    {
        string aiSymbol = "Brocolo";
        string playerSymbol = "Maca";

        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
            {
                boardState[i] = aiSymbol;
                if (CheckForWinCondition(aiSymbol))
                {
                    boardState[i] = "";
                    return i;
                }
                boardState[i] = "";
            }
        }

        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
            {
                boardState[i] = playerSymbol;
                if (CheckForWinCondition(playerSymbol))
                {
                    boardState[i] = "";
                    return i;
                }
                boardState[i] = "";
            }
        }

        if (boardState[4] == "")
        {
            return 4;
        }

        int[] corners = new int[] { 0, 2, 6, 8 };
        foreach (int i in corners)
        {
            if (boardState[i] == "")
                return i;
        }

        List<int> availableMoves = new List<int>();
        for (int i = 0; i < boardState.Length; i++)
        {
            if (boardState[i] == "")
                availableMoves.Add(i);
        }

        if (availableMoves.Count > 0)
        {
            return availableMoves[Random.Range(0, availableMoves.Count)];
        }

        return -1;
    }

    bool CheckForWin()
    {
        string currentSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo";
        return CheckForWinCondition(currentSymbol);
    }

    bool CheckForWinCondition(string symbol)
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
