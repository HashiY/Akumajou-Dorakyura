using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    public List<Weapons> weapons;
    public List<Key> keys; // depois deixar private
    public List<Consumableitem> items;
    public List<Armor> armors;

    public ItemDataBase itemDataBase;

    void Awake()
    {
        if(inventory == null) // para ter so um inventory
        {
            inventory = this;
        }
        else if( inventory != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
        LoadInventory(); //notutorial aqui funcionou e apagou os itens q ja possuia, mas aqui buga

    }

    void Start()
    {
        //LoadInventory(); // foi apagado
        //FindObjectOfType<UIManager>().UpdateUI(); //no tuto aqui q funciona, mas aqui nao
    }

    public void LoadInventory() // carregar items salvos no gamemanager
    {
        for (int i = 0; i < GameManager.gm.weaponId.Length; i++) // o tamanho do vetor de weaponid
        {
            AddWeapon(itemDataBase.GetWeapons(GameManager.gm.weaponId[i])); // acrescentar o id da weapon 
        }
        for (int i = 0; i < GameManager.gm.itemId.Length; i++) // o tamanho do vetor de weaponid
        {
            AddItem(itemDataBase.GetConsumableitem(GameManager.gm.itemId[i])); // acrescentar o id da weapon 
        }
        for (int i = 0; i < GameManager.gm.armorId.Length; i++) // o tamanho do vetor de weaponid
        {
            AddArmor(itemDataBase.GetArmor(GameManager.gm.armorId[i])); // acrescentar o id da weapon 
        }
        for (int i = 0; i < GameManager.gm.keyId.Length; i++) // o tamanho do vetor de weaponid
        {
            AddKey(itemDataBase.GetKey(GameManager.gm.keyId[i])); // acrescentar o id da weapon 
        }
    }

    public void AddWeapon(Weapons weapon)
    {
        weapons.Add(weapon);
    }

    public void AddArmor(Armor armor)
    {
        armors.Add(armor);
    }

    public void AddKey (Key key)
    {
        keys.Add(key);
    }

    public bool CheckKey(Key key) // para ver se tem a chave
    {
        for (int i = 0; i <  keys.Count; i++)
        {
            if(keys[i] == key)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(Consumableitem item)
    {
        items.Add(item);

    }

    public void RemoveItem(Consumableitem item) // para consumir as potions
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i] == item)
            {
                items.RemoveAt(i);
                break;
            }
        }
    }
    public int CountItems(Consumableitem item)//para colocar os numeros na UI
    {
        int numberOfItems = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if(item == items[i])
            {
                numberOfItems++;
            }
        }
        return numberOfItems;
    }
}
