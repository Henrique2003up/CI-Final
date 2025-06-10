using UnityEngine;

public class Fruit : MonoBehaviour
{
    Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tr.position -= new Vector3(0f, 0.7f, 0f);
        if (tr.position.y < -7f) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Basket")) // << Mude aqui
        {
            Destroy(this.gameObject);
        }
    }
}
