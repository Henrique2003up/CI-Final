using UnityEngine;

public class MusicaDeFundo : MonoBehaviour
{
    private static MusicaDeFundo instancia;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject); // Já existe uma instância, destrói a duplicata
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject); // Persiste entre cenas

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.loop = true;
            audio.Play();
        }
    }
}
