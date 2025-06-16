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

    // Feedback visual
    public Color wrongColor = Color.red;
    public float feedbackDuration = 1f;

    private bool isLocked = false;

    // Áudio
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        if (isLocked) return;

        var clickedButton = EventSystem.current.currentSelectedGameObject;
        var clickedItemName = clickedButton.name;

        if (clickedItemName == rightAnswer.ToString())
        {
            PlaySound(correctSound);
            GameManager.IncrementRightAnswer();
            SceneManager.LoadScene(nextQuestionSceneName);
        }
        else
        {
            PlaySound(wrongSound);
            GameManager.IncrementWrongAnswer();
            StartCoroutine(ShowWrongFeedback(clickedButton));
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator ShowWrongFeedback(GameObject button)
    {
        isLocked = true;

        Image img = button.GetComponent<Image>();
        Color originalColor = img != null ? img.color : Color.white;

        if (img != null)
            img.color = wrongColor;

        yield return new WaitForSeconds(feedbackDuration);

        if (img != null)
            img.color = originalColor;

        isLocked = false;
    }
}

