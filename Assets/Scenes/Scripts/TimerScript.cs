using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//Este script deve estar associado a um GameObject que tamb�m tenha o TMP_Text 
public class TimerScript : MonoBehaviour
{
    public TMP_Text timerText;
    private float currentTimer;
    private bool isCounting;
    // Start is called before the first frame update 
    void Start()
    {
        //timerText = GetComponent<TMP_Text>();
        currentTimer = 0;
        isCounting = true;
    }
    // Update is called once per frame 
    void Update()
    {
        if (!isCounting)
        {
            return;
        }
        currentTimer += Time.deltaTime;
        //Subtract elapsed time every frame 
        float seconds = Mathf.FloorToInt(currentTimer % 30);
        timerText.text = $"{seconds:00}";
    }
    public int GetTimerAndStop()
    {
        // isCounting = false; // Remova ou comente esta linha
        return (int)currentTimer;

    }
}
