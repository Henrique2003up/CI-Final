using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Facil, Medio, Dificil }
    public Difficulty currentDifficulty;

    public void SetDifficulty(int difficultyIndex)
    {
        currentDifficulty = (Difficulty)difficultyIndex;
        Debug.Log("Dificuldade definida como: " + currentDifficulty);
    }
}
