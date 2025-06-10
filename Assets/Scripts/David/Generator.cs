using UnityEngine;

public class Generator : MonoBehaviour
{
    float timer = 1;
    public GameObject[] gm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            int chance = Random.Range(300, -133);
            float pos_x = Random.Range(240f, 580f);

            if (chance <= 20)
            {
                Instantiate(gm[1], new Vector3(pos_x, 300f, 0.1f), new Quaternion(0, 0, 0, 0));

            }
            else
            {
                Instantiate(gm[0], new Vector3(pos_x, 300f, 0.1f), new Quaternion(0, 0, 0, 0));

            }

            timer = 0.7f;
        }
    }

}
