using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] fruits; // boas e más
    public float spawnInterval = 1f;
    public float xRange = 7f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnFruit), 1f, spawnInterval);
    }

    void SpawnFruit()
    {
        Vector3 pos = new Vector3(Random.Range(-xRange, xRange), transform.position.y, 0);
        Instantiate(fruits[Random.Range(0, fruits.Length)], pos, Quaternion.identity);
    }
}
