using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardImages;
    public Sprite backImage;
    public Transform grid;
    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;

    void Start()
    {
        InitCards();
    }

    void InitCards()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < cardImages.Length; i++)
        {
            ids.Add(i);
            ids.Add(i); // dois de cada
        }

        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, grid);
            Card card = cardObj.GetComponent<Card>();
            card.id = ids[i];
            card.frontImage = cardImages[ids[i]];
            card.backImage = backImage;
            cards.Add(card);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void CardRevealed(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstCard.id == secondCard.id)
        {
            // Mantém reveladas
        }
        else
        {
            firstCard.Hide();
            secondCard.Hide();
        }

        firstCard = null;
        secondCard = null;
    }
}
