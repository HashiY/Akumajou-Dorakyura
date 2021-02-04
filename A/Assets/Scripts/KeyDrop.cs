using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    public Key key;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = key.image;

        for (int i = 0; i < Inventory.inventory.keys.Count; i++)
        {
            if (Inventory.inventory.keys[i] == key)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            Inventory.inventory.AddKey(key);
            FindObjectOfType<UIManager>().SetMessage(key.message);
            Destroy(gameObject);
        }
    }

}
