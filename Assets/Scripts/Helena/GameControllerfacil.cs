using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameControllerfacil : MonoBehaviour
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
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = "";

            if (boardButtons[i].image != null)
            {
                boardButtons[i].image.sprite = emptySprite;
            }

            boardButtons[i].interactable = true;
        }

        isPlayerMacaTurn = true;
        turnText.text = "É a vez do Jogador Maca";
        movesMade = 0;

        gameOverPanel.SetActive(false);
    }

    public void OnBoardButtonClick(int index)
    {
        if (boardState[index] != "" || gameOverPanel.activeSelf)
        {
            return;
        }

        string currentPlayerSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo";
        boardState[index] = currentPlayerSymbol;

        if (boardButtons[index].image != null)
        {
            boardButtons[index].image.sprite = isPlayerMacaTurn ? macaSprite : brocoloSprite;
        }

        boardButtons[index].interactable = false;
        movesMade++;

        if (CheckForWin())
        {
            EndGame(currentPlayerSymbol + " Venceu!");
        }
        else if (movesMade == 9)
        {
            EndGame("Empate!");
        }
        else
        {
            isPlayerMacaTurn = !isPlayerMacaTurn;
            turnText.text = "É a vez do Jogador " + (isPlayerMacaTurn ? "Maca" : "Brocolo");
        }
    }

    bool CheckForWin()
    {
        string currentSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo"; // Corrigido aqui!

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int cell1 = winConditions[i, 0];
            int cell2 = winConditions[i, 1];
            int cell3 = winConditions[i, 2];

            if (boardState[cell1] == currentSymbol &&
                boardState[cell2] == currentSymbol &&
                boardState[cell3] == currentSymbol)
            {
                return true;
            }
        }
        return false;
    }

    void EndGame(string message)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = message;

        foreach (Button button in boardButtons)
        {
            button.interactable = false;
        }
    }

    public void RestartGame()
    {
        InitializeGame();
    }
}
