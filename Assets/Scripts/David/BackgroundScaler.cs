using UnityEngine;

// Este script ajusta a escala de um GameObject com SpriteRenderer
// para que ele preencha a tela da câmera principal.
// É ideal para backgrounds 2D em jogos.

public class BackgroundScaler : MonoBehaviour
{
    // A referência para a Main Camera (Câmera Principal).
    // Será preenchida automaticamente no Start.
    private Camera mainCamera;

    // Start é chamado antes do primeiro frame update.
    void Start()
    {
        // Encontra a câmera principal na cena.
        // É importante que sua câmera esteja marcada como "MainCamera" (tag padrão do Unity).
        mainCamera = Camera.main;

        // Se a câmera principal não for encontrada, exibe um erro e desativa o script.
        if (mainCamera == null)
        {
            Debug.LogError("BackgroundScaler: Main Camera não encontrada. Certifique-se de que sua câmera tem a tag 'MainCamera'.");
            enabled = false; // Desativa este script se a câmera não for encontrada.
            return;
        }

        // Chama a função para ajustar o tamanho do background.
        AdjustBackgroundSize();
    }

    // AdjustBackgroundSize é chamado para redimensionar o background.
    // Pode ser chamado no Start e também se a resolução da tela mudar.
    void AdjustBackgroundSize()
    {
        // Pega a altura da câmera em unidades do mundo.
        // Para uma câmera ortográfica, mainCamera.orthographicSize é metade da altura visível.
        float cameraHeight = mainCamera.orthographicSize * 2f;

        // Pega a largura da câmera em unidades do mundo, baseada na altura e no aspect ratio.
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Pega o componente SpriteRenderer anexado a este GameObject.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Verifica se o SpriteRenderer e o sprite estão presentes.
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogError("BackgroundScaler: SpriteRenderer ou Sprite não encontrado no GameObject '" + gameObject.name + "'.");
            return;
        }

        // Calcula a escala necessária em X e Y para que o sprite preencha a largura e altura da câmera.
        // Largura do sprite em unidades do mundo = sprite.bounds.size.x
        // Altura do sprite em unidades do mundo = sprite.bounds.size.y
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        // Calcula a escala para a largura.
        // transform.localScale.x é a escala atual.
        float scaleX = cameraWidth / spriteWidth;
        // Calcula a escala para a altura.
        float scaleY = cameraHeight / spriteHeight;

        // Aplica a nova escala ao GameObject.
        // O transform.localScale é um Vector3, então precisamos definir X, Y e Z.
        // Z permanece 1 (ou o que for necessário para o seu jogo 2D).
        transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // Opcional: Centraliza o background na câmera.
        // Isso é útil se o background não for filho da câmera.
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }

    // Opcional: Pode adicionar este método para ajustar o background
    // se a resolução da tela mudar durante o jogo.
    // void OnRectTransformDimensionsChange() // Para UI elements, não para SpriteRenderers
    // {
    //     AdjustBackgroundSize();
    // }
    // Este não é necessário para SpriteRenderers em geral, mas é bom ter em mente.
    // Para redimensionamento de janela, pode-se usar um evento de redimensionamento da tela
    // ou chamar AdjustBackgroundSize() no Update (com cuidado para não sobrecarregar).
}
