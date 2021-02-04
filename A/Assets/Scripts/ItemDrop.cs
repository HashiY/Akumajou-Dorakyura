using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Consumableitem item;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        
        Inventory.inventory.AddItem(item);
        FindObjectOfType<UIManager>().UpdateUI();
        FindObjectOfType<UIManager>().SetMessage(item.message);
        Destroy(gameObject);
        
    }
}
