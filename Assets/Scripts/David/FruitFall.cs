using UnityEngine;

public class FruitFall : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (gameObject.CompareTag("Fruit"))
            {
                GameManager2.instance.LoseLife();
            }
            Destroy(gameObject);
        }
    }
}
