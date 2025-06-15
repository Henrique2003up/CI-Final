using UnityEngine;
using UnityEngine.Events; // Necessário para UnityEvent

// Este script permite que um GameObject com SpriteRenderer e Collider2D
// atue como um botão, detetando cliques do rato ou toques.
public class SpriteButton : MonoBehaviour
{
    // A função a ser chamada quando o botão é clicado.
    // Defina esta opção no Inspector.
    // Ex: 'GameManagerRed' GameObject e 'RestartGame' ou 'GoToMainMenu' função.
    public UnityEvent OnClick;

    // Referência ao SpriteRenderer para efeitos visuais de clique.
    private SpriteRenderer spriteRenderer;

    // Cor original do sprite.
    private Color originalColor;

    // Cor para quando o rato/toque está em cima do botão.
    public Color hoverColor = Color.gray;

    // Cor para quando o botão é pressionado.
    public Color pressedColor = Color.white;

    // Flag para verificar se o rato está atualmente sobre o botão.
    private bool isMouseOver = false;

    void Start()
    {
        // Obtém o componente SpriteRenderer anexado a este GameObject.
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Guarda a cor original.
        }
        else
        {
            Debug.LogError("SpriteButton: SpriteRenderer não encontrado no GameObject '" + gameObject.name + "'. Este script requer um SpriteRenderer.");
            enabled = false; // Desativa o script se não houver SpriteRenderer.
            return;
        }

        // Certifica-se de que o GameObject tem um Collider 2D para detetar cliques/toques.
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("SpriteButton: O GameObject '" + gameObject.name + "' precisa de um Collider2D para detetar cliques! Adicione um Box Collider 2D ou similar.");
            enabled = false; // Desativa o script se não houver Collider2D.
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
            spriteRenderer.color = originalColor; // Volta à cor original.
        }
        isMouseOver = false;
    }

    // Chamado quando o botão do rato é pressionado (e solto) enquanto sobre o Collider 2D.
    private void OnMouseDown()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = pressedColor; // Muda a cor para o efeito de "pressionado".
        }
    }

    // Chamado quando o botão do rato é libertado enquanto sobre o Collider 2D.
    private void OnMouseUp()
    {
        if (spriteRenderer != null && isMouseOver) // Verifica se o rato ainda está sobre o botão.
        {
            spriteRenderer.color = hoverColor; // Volta à cor de "hover" se o rato ainda estiver lá.
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // Volta à cor original se o rato já saiu.
        }

        // Se o rato estava sobre o botão quando foi solto, invoca o evento OnClick.
        if (isMouseOver)
        {
            OnClick.Invoke(); // Dispara as funções conectadas no Inspector.
        }
    }
}