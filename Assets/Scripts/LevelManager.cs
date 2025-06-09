using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private enum RightAnswerPossibility
    {
        Btn1,
        Btn2,
        Btn3
    }

    [SerializeField]
    private RightAnswerPossibility rightAnswer;

    public string nextQuestionSceneName;

    // Feedback de erro
    public Color wrongColor = Color.red;
    public float feedbackDuration = 1f;

    private bool isLocked = false;

    public void OnButtonClick()
    {
        if (isLocked) return;

        var clickedButton = EventSystem.current.currentSelectedGameObject;
        var clickedItemName = clickedButton.name;

        if (clickedItemName == rightAnswer.ToString())
        {
            GameManager.IncrementRightAnswer();
            SceneManager.LoadScene(nextQuestionSceneName);
        }
        else
        {
            GameManager.IncrementWrongAnswer();
            StartCoroutine(ShowWrongFeedback(clickedButton));
        }
    }

    private IEnumerator ShowWrongFeedback(GameObject button)
    {
        isLocked = true;

        // Tenta pegar o componente de imagem e mudar a cor
        Image img = button.GetComponent<Image>();
        Color originalColor = img != null ? img.color : Color.white;

        if (img != null)
            img.color = wrongColor;

        // Espera um pouco para mostrar o erro
        yield return new WaitForSeconds(feedbackDuration);

        if (img != null)
            img.color = originalColor;

        isLocked = false;
    }
}


