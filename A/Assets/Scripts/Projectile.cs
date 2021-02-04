using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTime;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime); //para nao ficar na cena
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime); //mexe no eixo z
    }
}
