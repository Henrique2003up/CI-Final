using UnityEngine;

public class Generatorlvl2 : MonoBehaviour
{
    [Header("Configuração de Spawns")]
    public float initialSpawnInterval = 1.5f;
    public float minSpawnInterval = 0.4f;
    public float difficultyRampRate = 0.01f;

    [Header("Chances de Frutas Podres")]
    [Range(0, 300)] public int initialBadFruitThreshold = 10;

    [Header("Altura do Spawn")]
    public float baseSpawnHeight = 400f;
    public float heightVariation = 50f;

    [Header("Prefabs das Frutas")]
    public GameObject goodFruit;
    public GameObject badFruit;

    [Header("Posição X de Spawn")]
    public float minX = 240f;
    public float maxX = 580f;

    private float currentSpawnInterval;
    private float timer = 0f;
    private int currentBadFruitThreshold;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentBadFruitThreshold = initialBadFruitThreshold;
        timer = currentSpawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnFruit();

            // Aumenta a dificuldade ao longo do tempo
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - difficultyRampRate);
            currentBadFruitThreshold = Mathf.Min(300, currentBadFruitThreshold + 1); // Aumenta chance de fruta podre

            timer = currentSpawnInterval;
        }
    }

    void SpawnFruit()
    {
        float posX = Random.Range(minX, maxX);
        float posY = baseSpawnHeight + Random.Range(-heightVariation, heightVariation);
        Vector3 spawnPos = new Vector3(posX, posY, 0.1f);

        int roll = Random.Range(0, 300);

        if (roll < currentBadFruitThreshold)
        {
            Instantiate(badFruit, spawnPos, Quaternion.identity);
        }
        else
        {
            Instantiate(goodFruit, spawnPos, Quaternion.identity);
        }
    }
}
