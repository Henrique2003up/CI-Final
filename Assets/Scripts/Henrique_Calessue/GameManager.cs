using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static int rightAnswers = 0;
    private static int wrongAnswers = 0;

    // ✅ Novo campo para armazenar o tempo final
    private static float seconds = 0f;

    public static void IncrementRightAnswer()
    {
        rightAnswers++;
    }

    public static void IncrementWrongAnswer()
    {
        wrongAnswers++;
    }

    public static int GetRightAnswer()
    {
        return rightAnswers;
    }

    public static int GetWrongAnswer()
    {
        return wrongAnswers;
    }

    public static void Reset()
    {
        rightAnswers = 0;
        wrongAnswers = 0;
        seconds = 0f; // Também reinicia o tempo
    }

    // ✅ Métodos para definir e obter o tempo final
    public static void SetSeconds(float value)
    {
        seconds = value;
    }

    public static float GetSeconds()
    {
        return seconds;
    }
}
