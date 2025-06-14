using UnityEngine;
using UnityEngine.UI; // Necessário para interagir com elementos UI como Button
using System.Collections.Generic; // Para usar List<>
using TMPro; // *IMPORTANTE: Adicionado para compatibilidade com TextMeshPro*

public class GameController : MonoBehaviour
{
    // --- Variáveis de UI ---
    [Header("UI Elements")]
    [Tooltip("Arrasta todos os 9 botões do tabuleiro para aqui. A ordem é importante (0-8).")]
    public Button[] boardButtons; // Array dos botões do tabuleiro (cada célula)

    [Tooltip("O objeto de texto TextMeshProUGUI que exibe de quem é o turno.")]
    public TextMeshProUGUI turnText; // *Tipo alterado para TextMeshProUGUI*

    [Tooltip("O painel que aparece quando o jogo termina.")]
    public GameObject gameOverPanel; // Painel que aparece quando o jogo termina

    [Tooltip("O texto TextMeshProUGUI dentro do painel de fim de jogo que mostra o vencedor ou empate.")]
    public TextMeshProUGUI gameOverText; // *Tipo alterado para TextMeshProUGUI*

    // --- Variáveis de Imagens ---
    [Header("Game Images")]
    [Tooltip("A imagem (Sprite) para as células vazias do tabuleiro. Opcional.")]
    public Sprite emptySprite; // Imagem para a célula vazia (opcional)
    [Tooltip("A imagem (Sprite) para o Jogador Maca.")]
    public Sprite macaSprite;     // Imagem para o Jogador X
    [Tooltip("A imagem (Sprite) para o Jogador Brocolo.")]
    public Sprite brocoloSprite;     // Imagem para o Jogador O

    // --- Variáveis de Jogo ---
    [Header("Game State")]
    private string[] boardState; // Representa o estado atual do tabuleiro (X, O ou vazio)
    private bool isPlayerMacaTurn;  // True se for o turno do Jogador X, False se for do Jogador O
    private int movesMade;       // Contador de jogadas para verificar empate

    // --- Condições de Vitória ---
    // Todas as combinações possíveis para vencer no jogo da velha (linhas, colunas, diagonais)
    private int[,] winConditions = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Linhas
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Colunas
        {0, 4, 8}, {2, 4, 6}            // Diagonais
    };

    void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// Inicializa ou reinicia o jogo.
    /// Define o estado inicial do tabuleiro, botões, turno e visibilidade do painel de fim de jogo.
    /// </summary>
    void InitializeGame()
    {
        boardState = new string[9]; // 9 células para o tabuleiro 3x3
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = ""; // Todas as células são inicializadas como vazias

            // Define a imagem do botão para a sprite vazia (ou null se não houver emptySprite)
            // Certifica-te de que cada botão tem um componente Image!
            if (boardButtons[i].image != null)
            {
                boardButtons[i].image.sprite = emptySprite;
            }

            boardButtons[i].interactable = true; // Torna os botões clicáveis novamente
        }

        isPlayerMacaTurn = true; // Jogador X sempre começa o jogo
        turnText.text = "É a vez do Jogador Maca"; // Atualiza o texto do turno na UI
        movesMade = 0; // Zera o contador de jogadas

        gameOverPanel.SetActive(false); // Esconde o painel de fim de jogo no início
    }

    /// <summary>
    /// Chamado quando um botão do tabuleiro é clicado.
    /// Este método é conectado ao evento OnClick de cada botão no Inspector do Unity.
    /// </summary>
    /// <param name="index">O índice do botão clicado (0-8), que corresponde à posição no tabuleiro.</param>
    public void OnBoardButtonClick(int index)
    {
        // Impede jogadas se a célula já estiver ocupada ou se o jogo já terminou
        if (boardState[index] != "" || gameOverPanel.activeSelf)
        {
            return;
        }

        // Determina o símbolo do jogador atual ("X" ou "O")
        string currentPlayerSymbol = isPlayerMacaTurn ? "Maca" : "Brocolo";
        boardState[index] = currentPlayerSymbol; // Atualiza o estado lógico do tabuleiro

        // Define a imagem do botão com base no jogador atual
        if (boardButtons[index].image != null)
        {
            if (isPlayerMacaTurn)
            {
                boardButtons[index].image.sprite = macaSprite;
            }
            else
            {
                boardButtons[index].image.sprite = brocoloSprite;
            }
        }

        boardButtons[index].interactable = false; // Desabilita o botão após a jogada para evitar cliques múltiplos

        movesMade++; // Incrementa o número de jogadas realizadas

        // Verifica se há um vencedor após a jogada
        if (CheckForWin())
        {
            EndGame(currentPlayerSymbol + " Venceu!"); // Se houver vencedor, termina o jogo com a mensagem de vitória
        }
        // Se não houver vencedor e todas as 9 células estiverem preenchidas, é um empate
        else if (movesMade == 9)
        {
            EndGame("Empate!"); // Termina o jogo com a mensagem de empate
        }
        else
        {
            // Se o jogo continua, alterna o turno e atualiza o texto na UI
            isPlayerMacaTurn = !isPlayerMacaTurn;
            turnText.text = "É a vez do Jogador " + (isPlayerMacaTurn ? "Maca" : "Brocolo");
        }
    }

    /// <summary>
    /// Verifica se o jogador atual venceu o jogo.
    /// Itera sobre todas as condições de vitória pré-definidas.
    /// </summary>
    /// <returns>True se um jogador venceu, False caso contrário.</returns>
    bool CheckForWin()
    {
        string currentSymbol = isPlayerMacaTurn ? "Maca" : "brocolo";

        // Percorre todas as combinações de vitória (linhas, colunas, diagonais)
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int cell1 = winConditions[i, 0];
            int cell2 = winConditions[i, 1];
            int cell3 = winConditions[i, 2];

            // Verifica se as três células da condição de vitória têm o símbolo do jogador atual
            if (boardState[cell1] == currentSymbol &&
                boardState[cell2] == currentSymbol &&
                boardState[cell3] == currentSymbol)
            {
                return true; // Se sim, há um vencedor
            }
        }
        return false; // Nenhuma condição de vitória foi encontrada
    }

    /// <summary>
    /// Finaliza o jogo e exibe a mensagem apropriada (vitória ou empate).
    /// Desabilita os botões do tabuleiro para impedir mais jogadas.
    /// </summary>
    /// <param name="message">A mensagem a ser exibida (Ex: "X Venceu!", "Empate!").</param>
    void EndGame(string message)
    {
        gameOverPanel.SetActive(true); // Ativa o painel de fim de jogo para que ele seja visível
        gameOverText.text = message;   // Define o texto de fim de jogo

        // Desabilitar todos os botões do tabuleiro para evitar mais jogadas
        foreach (Button button in boardButtons)
        {
            button.interactable = false;
        }
    }

    /// <summary>
    /// Chamado pelo botão "Jogar Novamente".
    /// Reinicia o jogo chamando o método InitializeGame().
    /// </summary>
    public void RestartGame()
    {
        InitializeGame();
    }
}