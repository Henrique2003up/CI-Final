using UnityEngine;

public class Podre : MonoBehaviour
{
    Transform tr;
    // Referência para o GameManagerRed
    private GameManagerRed gameManager;

    void Start()
    {
        tr = GetComponent<Transform>();
        // Encontra o GameManagerRed na cena.
        gameManager = FindObjectOfType<GameManagerRed>();
        if (gameManager == null)
        {
            Debug.LogError("Podre: GameManagerRed não encontrado na cena!");
        }
    }

    // Update é chamado uma vez por frame.
    void FixedUpdate()
    {
        // REMOVA OU COMENTE ESTA LINHA se a fruta "Podre" já está a cair via Rigidbody 2D e gravidade.
        // Se a fruta "Podre" não tiver um Rigidbody 2D, MANTENHA esta linha para ela cair.
        // tr.position -= new Vector3(0f, 0.7f, 0f); 

        // Destrói a fruta "Podre" se ela cair para fora da tela.
        if (tr.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a colisão foi com um objeto com a Tag "Basket".
        // Usar CompareTag é mais eficiente e seguro do que comparar por nome.
        if (collision.CompareTag("Basket"))
        {
            Debug.Log("Fruta Podre colidiu com o Cesto!");

            // --- NOVIDADE: Chama o método de Game Over no GameManagerRed ---
            if (gameManager != null)
            {
                gameManager.TriggerBasketDestroyedGameOver();
            }
            // -----------------------------------------------------------

            Destroy(this.gameObject); // Destrói a própria fruta "Podre"
            // REMOVIDO: Destroy(collision.gameObject);
            // A destruição do cesto será gerida pelo GameManagerRed.
        }
    }
}
