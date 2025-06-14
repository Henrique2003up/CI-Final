using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public Sprite frontImage;
    public Sprite backImage;
    private bool isRevealed = false;
    private Button button;
    private Image image;
    private GameController gameController;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        gameController = FindObjectOfType<GameController>();
        button.onClick.AddListener(OnClick);
        Hide();
    }

    public void OnClick()
    {
        if (!isRevealed)
        {
            Reveal();
            gameController.CardRevealed(this);
        }
    }

    public void Reveal()
    {
        isRevealed = true;
        image.sprite = frontImage;
    }

    public void Hide()
    {
        isRevealed = false;
        image.sprite = backImage;
    }

    public bool IsRevealed() => isRevealed;
}
