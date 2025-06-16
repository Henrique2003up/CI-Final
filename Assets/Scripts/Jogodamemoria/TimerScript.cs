using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    private TMP_Text timerText;
    private float currentTimer;
    private bool isCounting;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        currentTimer = 0f;
        isCounting = true;
    }

    void Update()
    {
        if (!isCounting)
            return;

        currentTimer += Time.deltaTime;
        int roundedTime = Mathf.FloorToInt(currentTimer);
        timerText.text = roundedTime.ToString() + "s";
    }

    public float GetTimerAndStop()
    {
        isCounting = false;
        return currentTimer;
    }

    public void StopTimer()
    {
        isCounting = false;
    }

    public void ResetTimer()
    {
        currentTimer = 0f;
        isCounting = true;
    }

    public void PauseTimer()
    {
        isCounting = false;
    }
}
