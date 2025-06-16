using UnityEngine;
using UnityEngine.Events;

// Garante que este GameObject tem SEMPRE um SpriteRenderer e um BoxCollider2D.
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SpriteButton : MonoBehaviour
{
    public UnityEvent OnClick;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private Color originalColor;

    public Color hoverColor = Color.gray;
    public Color pressedColor = Color.white;

    private bool isMouseOver = false;

    // NOVO: Campos para ajuste manual do collider no Inspector
    public bool useManualColliderValues = false; // Marque esta caixa para usar os valores abaixo
    public Vector2 manualColliderOffset = Vector2.zero; // Offset manual do collider
    public Vector2 manualColliderSize = Vector2.one;    // Tamanho manual do collider

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteButton: Awake - SpriteRenderer not found on '" + gameObject.name + "'.", this);
        }
        if (boxCollider == null)
        {
            Debug.LogError("SpriteButton: Awake - BoxCollider2D not found on '" + gameObject.name + "'.", this);
        }
    }

    void OnEnable()
    {
        // Certifica-se de que o Collider2D está ativo e NÃO é um trigger.
        if (boxCollider != null)
        {
            boxCollider.enabled = true; // Garante que o componente collider está ativo.
            boxCollider.isTrigger = false; // MUITO IMPORTANTE: Garante que NÃO é um trigger para OnMouse* funcionar.
        }
        else
        {
            Debug.LogError("SpriteButton: OnEnable - BoxCollider2D is null on '" + gameObject.name + "'. Clicks will not work.", this);
            enabled = false;
            return;
        }

        // NOVO: Aplica ajuste manual ou automático do collider.
        ApplyColliderAdjustment();

        // Restaura a cor original quando o botão é ativado.
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // DEBUG: Loga o estado final do collider ao ser ativado.
        Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' OnEnable. Collider enabled: " + boxCollider.enabled + ", Is Trigger: " + boxCollider.isTrigger + ", Final Size: " + boxCollider.size + ", Final Offset: " + boxCollider.offset, this);
    }

    // NOVO: Método para aplicar o ajuste do collider (manual ou automático).
    void ApplyColliderAdjustment()
    {
        if (useManualColliderValues)
        {
            // Usa os valores definidos manualmente no Inspector
            boxCollider.offset = manualColliderOffset;
            boxCollider.size = manualColliderSize;
            Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' - Usando valores de collider manuais. Offset: " + manualColliderOffset + ", Size: " + manualColliderSize, this);
        }
        else // Se não usar manual, tenta o ajuste automático
        {
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                boxCollider.size = new Vector2(
                    spriteRenderer.sprite.bounds.size.x * transform.localScale.x,
                    spriteRenderer.sprite.bounds.size.y * transform.localScale.y
                );
                boxCollider.offset = spriteRenderer.sprite.bounds.center;
                Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' - Usando valores de collider automáticos. Offset: " + boxCollider.offset + ", Size: " + boxCollider.size, this);
            }
            else
            {
                Debug.LogWarning("SpriteButton WARNING: '" + gameObject.name + "' - Sprite ou SpriteRenderer não atribuído. O collider pode não ser dimensionado corretamente.", this);
            }
        }
    }

    // Adiciona uma função que pode ser chamada pelo Editor para forçar o ajuste (apenas para verificar).
    [ContextMenu("Forçar Ajuste do Collider")]
    void ForceAdjustColliderInEditor()
    {
        Awake(); // Garante que as referências são obtidas.
        ApplyColliderAdjustment(); // Re-aplica o ajuste.
        Debug.Log("SpriteButton: Collider forced to adjust on '" + gameObject.name + "'. Size: " + boxCollider.size + ", Offset: " + boxCollider.offset, this);
    }

    private void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor;
        }
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isMouseOver = false;
    }

    private void OnMouseDown()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = pressedColor;
        }
        Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' OnMouseDown detected! Collider Is Trigger: " + boxCollider.isTrigger, this);
    }

    private void OnMouseUp()
    {
        if (spriteRenderer != null)
        {
            if (isMouseOver)
            {
                spriteRenderer.color = hoverColor;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }
        }

        if (isMouseOver)
        {
            Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' OnClick.Invoke() fired!", this);
            OnClick.Invoke();
        }
        else
        {
            Debug.Log("SpriteButton DEBUG: '" + gameObject.name + "' OnMouseUp detected, but mouse outside button. OnClick NOT fired.", this);
        }
    }
}
