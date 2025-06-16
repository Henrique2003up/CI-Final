using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public Image Below;
    public Image Cover;

    public void Awake()
    {
        if (Below != null)
            Below.gameObject.SetActive(true);

        if (Cover != null)
            Cover.gameObject.SetActive(true);
    }

    public void SetBelowColor(Color newColor)
    {
        if (Below != null)
            Below.color = newColor;
    }

    public void SetBelowImage(Sprite newImage)
    {
        if (Below != null)
        {
            Below.color = Color.white;
            Below.sprite = newImage;
        }
    }

    public void DisableCover()
    {
        if (Cover != null)
            Cover.gameObject.SetActive(false);
    }

    public void EnableCover()
    {
        if (Cover != null)
            Cover.gameObject.SetActive(true);
    }
}
