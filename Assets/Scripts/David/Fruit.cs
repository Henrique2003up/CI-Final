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
        Debug.Log("OnTriggerEnter2D chamado! Colidiu com: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");

        // A LÓGICA DE DESTROI DEVE SER APENAS AQUI:
        if (collision.gameObject.name == "Basket") // Ou collision.gameObject.name == "Basket"
        {
            Debug.Log("Condição 'Basket' satisfeita! Destruindo fruta.");
            Destroy(this.gameObject);
        }
        // REMOVA QUALQUER OUTRO DESTROY(THIS.GAMEOBJECT) FORA DESTA CONDIÇÃO.
        // Exemplo de algo a remover:
        // else { Destroy(this.gameObject); } <-- Isto destruiria se colidisse com qualquer coisa que não fosse o cesto.
    }
}
