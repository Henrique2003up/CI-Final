using UnityEngine;

public class Basket : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(move, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            GameManager2.instance.CollectGoodFruit();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BadFruit"))
        {
            GameManager2.instance.LoseLife();
            Destroy(other.gameObject);
        }
    }
}
