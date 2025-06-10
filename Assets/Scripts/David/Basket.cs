using UnityEngine;

public class Basket : MonoBehaviour
{
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {
        // Debug para a seta direita
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            Debug.Log("Seta Direita Pressionada! Pos X: " + tr.position.x); // Adicione este log
            if (tr.position.x < 580f)
            {
                tr.position += new Vector3(1f, 0f, 0f);
            }
            else
            {
                Debug.Log("Limite de X atingido (direita): " + tr.position.x); // Adicione este log
            }
        }

        // Debug para a seta esquerda
        if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            Debug.Log("Seta Esquerda Pressionada! Pos X: " + tr.position.x); // Adicione este log
            if (tr.position.x > 240f)
            {
                tr.position += new Vector3(-0.5f, 0f, 0f);
            }
            else
            {
                Debug.Log("Limite de X atingido (esquerda): " + tr.position.x); // Adicione este log
            }
        }
    }
}
