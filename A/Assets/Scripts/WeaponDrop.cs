using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Weapons weapon;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = weapon.image;

        for (int i = 0; i < Inventory.inventory.weapons.Count; i++)
        {
            if (Inventory.inventory.weapons[i] == weapon)
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
            player.AddWeapon(weapon);
            Inventory.inventory.AddWeapon(weapon);
            FindObjectOfType<UIManager>().SetMessage(weapon.message);
            Destroy(gameObject);
        }

    }
}
