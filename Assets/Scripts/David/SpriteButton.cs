using UnityEngine;
using UnityEngine.Events; // Necess�rio para UnityEvent

// Este script permite que um GameObject com SpriteRenderer e Collider2D
// atue como um bot�o, detetando cliques do rato ou toques.
public class SpriteButton : MonoBehaviour
{
    // A fun��o a ser chamada quando o bot�o � clicado.
    // Defina esta op��o no Inspector.
    // Ex: 'GameManagerRed' GameObject e 'RestartGame' ou 'GoToMainMenu' fun��o.
    public UnityEvent OnClick;

    // Refer�ncia ao SpriteRenderer para efeitos visuais de clique.
    private SpriteRenderer spriteRenderer;

    // Cor original do sprite.
    private Color originalColor;

    // Cor para quando o rato/toque est� em cima do bot�o.
    public Color hoverColor = Color.gray;

    // Cor para quando o bot�o � pressionado.
    public Color pressedColor = Color.white;

    // Flag para verificar se o rato est� atualmente sobre o bot�o.
    private bool isMouseOver = false;

    void Start()
    {
        // Obt�m o componente SpriteRenderer anexado a este GameObject.
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Guarda a cor original.
        }
        else
        {
            Debug.LogError("SpriteButton: SpriteRenderer n�o encontrado no GameObject '" + gameObject.name + "'. Este script requer um SpriteRenderer.");
            enabled = false; // Desativa o script se n�o houver SpriteRenderer.
            return;
        }

        // Certifica-se de que o GameObject tem um Collider 2D para detetar cliques/toques.
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("SpriteButton: O GameObject '" + gameObject.name + "' precisa de um Collider2D para detetar cliques! Adicione um Box Collider 2D ou similar.");
            enabled = false; // Desativa o script se n�o houver Collider2D.
        }
    }

    // Chamado quando o rato entra no Collider 2D do GameObject.
    private void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor; // Muda a cor para o efeito de "hover".
        }
        isMouseOver = true;
    }

    // Chamado quando o rato sai do Collider 2D do GameObject.
    private void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // Volta � cor original.
        }
        isMouseOver = false;
    }

    // Chamado quando o bot�o do rato � pressionado (e solto) enquanto sobre o Collider 2D.
    private void OnMouseDown()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = pressedColor; // Muda a cor para o efeito de "pressionado".
        }
    }

    // Chamado quando o bot�o do rato � libertado enquanto sobre o Collider 2D.
    private void OnMouseUp()
    {
        if (spriteRenderer != null && isMouseOver) // Verifica se o rato ainda est� sobre o bot�o.
        {
            spriteRenderer.color = hoverColor; // Volta � cor de "hover" se o rato ainda estiver l�.
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // Volta � cor original se o rato j� saiu.
        }

        // Se o rato estava sobre o bot�o quando foi solto, invoca o evento OnClick.
        if (isMouseOver)
        {
            OnClick.Invoke(); // Dispara as fun��es conectadas no Inspector.
        }
    }
}