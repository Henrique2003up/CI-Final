using UnityEngine;

public class Fruit : MonoBehaviour
{
    Transform tr;
    private GameManagerRed gameManager; // Refer�ncia para o GameManager
    void Start()
    {
        tr = GetComponent<Transform>();
        gameManager = FindObjectOfType<GameManagerRed>();
        if (gameManager == null)
        {
            Debug.LogError("Fruit: GameManager n�o encontrado na cena!");
        }
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

        // A L�GICA DE DESTROI DEVE SER APENAS AQUI:
        if (collision.gameObject.name == "Basket") // Ou collision.gameObject.name == "Basket"
        {
            if (gameManager != null)
            {
                    gameManager.AddScore(1); // Se for fruta boa, adiciona 1 ponto   
            }
            Debug.Log("Condi��o 'Basket' satisfeita! Destruindo fruta.");
            Destroy(this.gameObject);
        }
        // REMOVA QUALQUER OUTRO DESTROY(THIS.GAMEOBJECT) FORA DESTA CONDI��O.
        // Exemplo de algo a remover:
        // else { Destroy(this.gameObject); } <-- Isto destruiria se colidisse com qualquer coisa que n�o fosse o cesto.
    }
}
