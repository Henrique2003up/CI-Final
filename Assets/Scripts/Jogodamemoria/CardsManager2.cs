using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
public class CardsManager2 : MonoBehaviour
{
    [SerializeField]
    private List<CardScript> listOfCards;
    [SerializeField]
    private List<Color> colors;
    [SerializeField]
    private List<Sprite> sprites; //TODO para o final da ficha 
    [SerializeField]
    private bool shouldUseSprites; //TODO Assim que utilizar os sprites, deve alterar esta variável 
    [SerializeField]
    private AudioSource victoryMusic;
    [SerializeField]
    private TimerScript timerScript;
    private CardScript firstSelectedItem;
    private CardScript secondSelectedItem;
    private int numberOfMatches = 0;
    private CanvasGroup canvasGroup;
    public void Start()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();

        if ((!shouldUseSprites && listOfCards.Count / 2 != colors.Count)
         || (shouldUseSprites && listOfCards.Count / 2 != sprites.Count))
        {
            throw new ApplicationException("A configuração do Game está errada.");
        }
        //Activar todas as Covers 

        //colocar as cores / sprites no sítio certo 
        for (var i = 0; i < listOfCards.Count; i++)
        {
            if (shouldUseSprites)
            {
                listOfCards[i].SetBelowImage(sprites[i / 2]);
            }
            else
            {
                listOfCards[i].SetBelowColor(colors[i / 2]);
            }
        }
        //Troca as ordens da lista de cartões 
        Shuffle(listOfCards);
    }
    // Function to shuffle a list 
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        //Com base na troca feita na função anterior, troca a sua posição no Unity. 
        for (int i = 0; i < listOfCards.Count; i++)
        {
            listOfCards[i].transform.SetSiblingIndex(i);
        }
    }
    public void OnCardClick()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            return;
        }
        if (firstSelectedItem && secondSelectedItem)
        {
            return;
        }
        var
        clickedItem =
        EventSystem.current.currentSelectedGameObject.GetComponentInParent<CardScript>();
        if (!firstSelectedItem)
        {

            firstSelectedItem = clickedItem;
            firstSelectedItem.DisableCover();
        }
        else
        {
            secondSelectedItem = clickedItem;
            secondSelectedItem.DisableCover();
            CompareChosenItems();
        }
    }
    private void CompareChosenItems()
    {
        if (!shouldUseSprites)
        {
            if (firstSelectedItem.Below.color == secondSelectedItem.Below.color)
            {
                numberOfMatches++;
                StartCoroutine(ResetAndCheckFinish(0, false));
            }
            else
            {
                StartCoroutine(ResetAndCheckFinish(2, true));
            }
        }
        else
        {
            //TODO: Fazer a parte das sprites 
        }
    }

    IEnumerator ResetAndCheckFinish(int numberOfSecondsToWait, bool shouldReset)
    {
        canvasGroup.interactable = false;
        yield return new WaitForSeconds(numberOfSecondsToWait);
        if (shouldReset)
        {
            firstSelectedItem.EnableCover();
            secondSelectedItem.EnableCover();
        }
        firstSelectedItem = null;
        secondSelectedItem = null;
        canvasGroup.interactable = true;
        if (numberOfMatches == listOfCards.Count / 2)
        {
            StartCoroutine(LoadFinalScene());
        }
    }

    IEnumerator LoadFinalScene()
    {
        GameManager.SetSeconds(timerScript.GetTimerAndStop());

        //Toca o audio de vitória 
        victoryMusic.Play();

        //Espera que o audio termine 
        yield return new WaitForSeconds(victoryMusic.clip.length);

        //Carrega a outra cena 
        SceneManager.LoadScene("FinalScene");
    }
}
