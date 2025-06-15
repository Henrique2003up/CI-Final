using UnityEngine;

public class Generatorlvl1 : MonoBehaviour
{
    // Intervalo de tempo entre spawns de frutas.
    // Quanto maior este valor, menos frutas aparecem = jogo mais fácil.
    public float spawnInterval = 1.5f;

    // Limiar para a chance de spawnar a fruta podre (gm[1]).
    // Quanto menor este valor, menos chances da fruta ser podre = jogo mais fácil.
    // O Range [0, 300] é para facilitar o ajuste no Inspector.
    [Range(0, 300)]
    public int badFruitChanceThreshold = 10;

    // Altura onde as frutas spawnam.
    // Quanto maior este valor, mais tempo a fruta leva para cair = jogo mais fácil.
    public float spawnHeight = 400f;

    // Timer interno para controlar o spawn.
    float timer = 0;

    // Array de GameObjects para as frutas (gm[0] = fruta boa, gm[1] = fruta podre).
    public GameObject[] gm;

    // Start é chamado uma vez antes do primeiro frame update.
    void Start()
    {
        // Inicializa o timer para que a primeira fruta spawne no início do jogo.
        timer = spawnInterval;
    }

    // Update é chamado uma vez por frame.
    void Update()
    {
        // Decrementa o timer até chegar a zero.
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        // Quando o timer chega a zero, spawna uma nova fruta.
        else
        {
            // Gera um número aleatório entre -133 e 299 (limite superior exclusivo).
            // Este é o mesmo range que tinha antes, apenas para base da chance.
            int chance = Random.Range(300, -133);

            // Gera uma posição X aleatória para o spawn da fruta.
            float pos_x = Random.Range(240f, 580f);

            // Verifica se o número aleatório está abaixo ou igual ao limiar da fruta podre.
            // Esta é a linha onde você controla a chance de spawnar a fruta podre!
            if (chance <= badFruitChanceThreshold)
            {
                // Spawna a fruta podre (assumindo que gm[1] é a fruta podre).
                Instantiate(gm[1], new Vector3(pos_x, spawnHeight, 0.1f), Quaternion.identity); // Usar Quaternion.identity é o mesmo que new Quaternion(0,0,0,0)
            }
            else
            {
                // Spawna a fruta boa (assumindo que gm[0] é a fruta boa).
                Instantiate(gm[0], new Vector3(pos_x, spawnHeight, 0.1f), Quaternion.identity); // Usar Quaternion.identity é o mesmo que new Quaternion(0,0,0,0)
            }

            // Reseta o timer para o próximo spawn.
            timer = spawnInterval;
        }
    }
}
