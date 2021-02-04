using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Key key;
    public Sprite doorOpen;

    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //direto pela tag
        if (other.gameObject.CompareTag("Player"))
        {
            if (Inventory.inventory.CheckKey(key)) // se tem chave
            {
                sprite.sprite = doorOpen;
                boxCollider.enabled = false; // para passar a porta
            }
            else
            {
                FindObjectOfType<UIManager>().SetMessage("Precisa da " + key.keyName);
            }
        }

    }


}
