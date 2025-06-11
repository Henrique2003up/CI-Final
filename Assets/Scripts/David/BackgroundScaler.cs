using UnityEngine;

// Este script ajusta a escala de um GameObject com SpriteRenderer
// para que ele preencha a tela da c�mera principal.
// � ideal para backgrounds 2D em jogos.

public class BackgroundScaler : MonoBehaviour
{
    // A refer�ncia para a Main Camera (C�mera Principal).
    // Ser� preenchida automaticamente no Start.
    private Camera mainCamera;

    // Start � chamado antes do primeiro frame update.
    void Start()
    {
        // Encontra a c�mera principal na cena.
        // � importante que sua c�mera esteja marcada como "MainCamera" (tag padr�o do Unity).
        mainCamera = Camera.main;

        // Se a c�mera principal n�o for encontrada, exibe um erro e desativa o script.
        if (mainCamera == null)
        {
            Debug.LogError("BackgroundScaler: Main Camera n�o encontrada. Certifique-se de que sua c�mera tem a tag 'MainCamera'.");
            enabled = false; // Desativa este script se a c�mera n�o for encontrada.
            return;
        }

        // Chama a fun��o para ajustar o tamanho do background.
        AdjustBackgroundSize();
    }

    // AdjustBackgroundSize � chamado para redimensionar o background.
    // Pode ser chamado no Start e tamb�m se a resolu��o da tela mudar.
    void AdjustBackgroundSize()
    {
        // Pega a altura da c�mera em unidades do mundo.
        // Para uma c�mera ortogr�fica, mainCamera.orthographicSize � metade da altura vis�vel.
        float cameraHeight = mainCamera.orthographicSize * 2f;

        // Pega a largura da c�mera em unidades do mundo, baseada na altura e no aspect ratio.
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Pega o componente SpriteRenderer anexado a este GameObject.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se o SpriteRenderer e o sprite est�o presentes.
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogError("BackgroundScaler: SpriteRenderer ou Sprite n�o encontrado no GameObject '" + gameObject.name + "'.");
            return;
        }

        // Calcula a escala necess�ria em X e Y para que o sprite preencha a largura e altura da c�mera.
        // Largura do sprite em unidades do mundo = sprite.bounds.size.x
        // Altura do sprite em unidades do mundo = sprite.bounds.size.y
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        // Calcula a escala para a largura.
        // transform.localScale.x � a escala atual.
        float scaleX = cameraWidth / spriteWidth;
        // Calcula a escala para a altura.
        float scaleY = cameraHeight / spriteHeight;

        // Aplica a nova escala ao GameObject.
        // O transform.localScale � um Vector3, ent�o precisamos definir X, Y e Z.
        // Z permanece 1 (ou o que for necess�rio para o seu jogo 2D).
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // Opcional: Centraliza o background na c�mera.
        // Isso � �til se o background n�o for filho da c�mera.
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }

    // Opcional: Pode adicionar este m�todo para ajustar o background
    // se a resolu��o da tela mudar durante o jogo.
    // void OnRectTransformDimensionsChange() // Para UI elements, n�o para SpriteRenderers
    // {
    //     AdjustBackgroundSize();
    // }
    // Este n�o � necess�rio para SpriteRenderers em geral, mas � bom ter em mente.
    // Para redimensionamento de janela, pode-se usar um evento de redimensionamento da tela
    // ou chamar AdjustBackgroundSize() no Update (com cuidado para n�o sobrecarregar).
}
