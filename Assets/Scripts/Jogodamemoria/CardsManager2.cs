using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Necessário para EventSystem.current
using UnityEngine.SceneManagement; // Necessário para SceneManager.LoadScene
using UnityEngine.UI; // Necessário para usar Button (para interagir com o botão da carta)
using Random = UnityEngine.Random; // Para evitar ambiguidade com System.Random

public class CardsManager2 : MonoBehaviour
{
    [SerializeField]
    private List<CardScript> listOfCards; // Lista de todas as cartas no jogo
    [SerializeField]
    private List<Color> colors; // Cores disponíveis para as cartas (se não usar sprites)
    [SerializeField]
    private List<Sprite> sprites; // Sprites disponíveis para as cartas (se usar sprites) 
    [SerializeField]
    private bool shouldUseSprites; // Flag para determinar se o jogo usa cores ou sprites
    [SerializeField]
    private AudioSource victoryMusic; // Áudio a tocar quando o jogo termina
    [SerializeField]
    private TimerScript timerScript; // Referência ao script do temporizador

    private CardScript firstSelectedItem; // Primeira carta selecionada
    private CardScript secondSelectedItem; // Segunda carta selecionada
    private int numberOfMatches = 0; // Contador de pares encontrados
    private CanvasGroup canvasGroup; // Para controlar a interatividade de todo o Canvas

    private bool canClick = true; // Flag para controlar se o jogador pode clicar em cartas

    public void Start()
    {
        // Obtém o CanvasGroup no GameObject pai ou no próprio GameObject (se estiver no root do Canvas)
        canvasGroup = GetComponentInParent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Fallback caso não encontre no pai, procura no próprio objeto
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogError("CanvasGroup não encontrado! Certifique-se que o CardsManager2 ou o seu pai tem um CanvasGroup.");
            }
        }

        // Validação da configuração do jogo
        if ((!shouldUseSprites && listOfCards.Count / 2 != colors.Count)
         || (shouldUseSprites && listOfCards.Count / 2 != sprites.Count))
        {
            throw new ApplicationException("A configuração do Jogo está errada: O número de cartas não corresponde ao número de cores/sprites.");
        }

        // Ativa todas as Covers no início para que as cartas comecem viradas para baixo
        foreach (var card in listOfCards)
        {
            card.EnableCover();
        }

        // Atribui as cores ou sprites e define os CardIDs
        for (var i = 0; i < listOfCards.Count; i++)
        {
            if (shouldUseSprites)
            {
                listOfCards[i].SetBelowImage(sprites[i / 2]);
                listOfCards[i].SetCardID(i / 2); // Atribui um ID com base no índice do sprite
            }
            else
            {
                listOfCards[i].SetBelowColor(colors[i / 2]);
                // Para cores, podemos usar o índice da cor como ID, se necessário para outra lógica
                listOfCards[i].SetCardID(i / 2); // Opcional: Atribui um ID para cores também, para consistência
            }
        }

        // Embaralha a ordem visual das cartas no Unity
        Shuffle(listOfCards);

       
    }

    // Função genérica para embaralhar uma lista (Fisher-Yates shuffle)
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1); // Escolhe um índice aleatório
            // Troca os elementos de posição
            (list[k], list[n]) = (list[n], list[k]);
        }

        // Atualiza a posição visual dos objetos no Unity para corresponder à ordem embaralhada
        for (int i = 0; i < listOfCards.Count; i++)
        {
            listOfCards[i].transform.SetSiblingIndex(i);
        }
    }

    // Chamada quando um botão de carta é clicado
    public void OnCardClick()
    {
        // Impede cliques se o jogo estiver a processar uma combinação ou não permitir cliques
        if (!canClick)
        {
            Debug.Log("Clique bloqueado: canClick é falso.");
            return;
        }

        // Verifica se há um objeto atualmente selecionado pelo sistema de eventos
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            Debug.LogWarning("Nenhum GameObject selecionado no EventSystem.");
            return;
        }

        // Obtém o componente CardScript do item clicado
        // Procura no próprio GameObject do botão ou no seu pai
        var clickedItem = EventSystem.current.currentSelectedGameObject.GetComponentInParent<CardScript>();
        if (clickedItem == null)
        {
            Debug.LogError("CardScript não encontrado no item clicado ou no seu pai.");
            return;
        }

        // Previne clicar na mesma carta duas vezes ou numa carta que já está virada (match)
        // A capa ativa significa que a carta está virada para baixo
        if (clickedItem == firstSelectedItem || !clickedItem.Cover.gameObject.activeSelf)
        {
            Debug.Log("Clique inválido: Carta já selecionada ou já virada.");
            return;
        }

        if (firstSelectedItem == null) // Se esta é a primeira carta a ser selecionada
        {
            firstSelectedItem = clickedItem;
            firstSelectedItem.DisableCover(); // Revela a carta
            Debug.Log("Primeira carta selecionada: " + firstSelectedItem.name);
        }
        else // Se esta é a segunda carta a ser selecionada
        {
            secondSelectedItem = clickedItem;
            secondSelectedItem.DisableCover(); // Revela a carta
            Debug.Log("Segunda carta selecionada: " + secondSelectedItem.name);
            canClick = false; // Desabilita cliques enquanto as cartas estão a ser comparadas
            CompareChosenItems(); // Inicia a comparação
        }
    }

    // Compara as duas cartas selecionadas
    private void CompareChosenItems()
    {
        bool isMatch = false;

        if (!shouldUseSprites) // Se estiver a usar cores
        {
            if (firstSelectedItem.Below.color == secondSelectedItem.Below.color)
            {
                isMatch = true;
                Debug.Log("Comparação por cor: Match!");
            }
            else
            {
                Debug.Log("Comparação por cor: No Match.");
            }
        }
        else // Se estiver a usar sprites
        {
            if (firstSelectedItem.CardID == secondSelectedItem.CardID) // Compara usando o CardID
            {
                isMatch = true;
                Debug.Log("Comparação por sprite (ID): Match!");
            }
            else
            {
                Debug.Log("Comparação por sprite (ID): No Match.");
            }
        }

        if (isMatch)
        {
            numberOfMatches++;
            // Chamar ResetAndCheckFinish com shouldReset = false para manter as cartas viradas
            // Pequeno delay para o jogador ver o match antes de as cartas ficarem desativadas
            StartCoroutine(ResetAndCheckFinish(0.5f, false));

            // Desativar a interatividade das cartas que deram match
            // Garante que o componente Button existe nas cartas
            Button firstButton = firstSelectedItem.GetComponent<Button>();
            Button secondButton = secondSelectedItem.GetComponent<Button>();

            if (firstButton != null) firstButton.interactable = false;
            if (secondButton != null) secondButton.interactable = false;

            Debug.Log("Número de matches: " + numberOfMatches);
        }
        else
        {
            // Chamar ResetAndCheckFinish com shouldReset = true para virar as cartas de volta
            // Maior delay para o jogador ver o erro
            StartCoroutine(ResetAndCheckFinish(1.5f, true));
        }
    }

    // Corrotina para redefinir as cartas e verificar se o jogo terminou
    IEnumerator ResetAndCheckFinish(float numberOfSecondsToWait, bool shouldReset)
    {
        // Desativa a interatividade de todos os elementos UI no Canvas durante o atraso
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
        }

        yield return new WaitForSeconds(numberOfSecondsToWait); // Espera o tempo definido

        if (shouldReset) // Se não foi um match, vira as cartas de volta
        {
            if (firstSelectedItem != null) firstSelectedItem.EnableCover();
            if (secondSelectedItem != null) secondSelectedItem.EnableCover();
            Debug.Log("Cartas viradas para baixo.");
        }

        // Reseta as referências das cartas selecionadas para a próxima jogada
        firstSelectedItem = null;
        secondSelectedItem = null;

        // Reativa a interatividade do Canvas e permite novos cliques
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
        }
        canClick = true;
        Debug.Log("Cliques reativados.");

        // Verifica se todos os pares foram encontrados
        if (numberOfMatches == listOfCards.Count / 2)
        {
            Debug.Log("Todos os matches encontrados! Carregando cena final.");
            StartCoroutine(LoadFinalScene());
        }
    }

    // Corrotina para carregar a cena final após a vitória
    IEnumerator LoadFinalScene()
    {
        // Define o tempo do jogo usando o TimerScript e o para
        if (timerScript != null)
        {
            Game.SetSeconds(timerScript.GetTimerAndStop());
        }

        // Toca o áudio de vitória
        if (victoryMusic != null)
        {
            victoryMusic.Play();
            // Espera que o áudio termine
            yield return new WaitForSeconds(victoryMusic.clip.length);
        }
        else
        {
            // Se não houver música, espera um pouco para dar feedback visual
            yield return new WaitForSeconds(1.0f);
        }

        // Carrega a cena final
        SceneManager.LoadScene("Parabéns_Galo");
    }
}
