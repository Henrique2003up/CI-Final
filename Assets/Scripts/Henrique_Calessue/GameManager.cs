using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static int rightAnswers = 0;
    private static int wrongAnswers = 0;
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
    }
}
