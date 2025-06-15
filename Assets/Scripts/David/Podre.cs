using UnityEngine;

public class Podre : MonoBehaviour
{
    Transform tr;
    // Refer�ncia para o GameManagerRed
    private GameManagerRed gameManager;

    void Start()
    {
        tr = GetComponent<Transform>();
        // Encontra o GameManagerRed na cena.
        gameManager = FindObjectOfType<GameManagerRed>();
        if (gameManager == null)
        {
            Debug.LogError("Podre: GameManagerRed n�o encontrado na cena!");
        }
    }

    // Update � chamado uma vez por frame.
    void FixedUpdate()
    {
        // REMOVA OU COMENTE ESTA LINHA se a fruta "Podre" j� est� a cair via Rigidbody 2D e gravidade.
        // Se a fruta "Podre" n�o tiver um Rigidbody 2D, MANTENHA esta linha para ela cair.
        // tr.position -= new Vector3(0f, 0.7f, 0f); 

        // Destr�i a fruta "Podre" se ela cair para fora da tela.
        if (tr.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a colis�o foi com um objeto com a Tag "Basket".
        // Usar CompareTag � mais eficiente e seguro do que comparar por nome.
        if (collision.CompareTag("Basket"))
        {
            Debug.Log("Fruta Podre colidiu com o Cesto!");

            // --- NOVIDADE: Chama o m�todo de Game Over no GameManagerRed ---
            if (gameManager != null)
            {
                gameManager.TriggerBasketDestroyedGameOver();
            }
            // -----------------------------------------------------------

            Destroy(this.gameObject); // Destr�i a pr�pria fruta "Podre"
            // REMOVIDO: Destroy(collision.gameObject);
            // A destrui��o do cesto ser� gerida pelo GameManagerRed.
        }
    }
}
