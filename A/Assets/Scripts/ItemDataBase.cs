using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour // mante dotod os jogos q existe no jogo
{
    public List<Weapons> weapons;
    public List<Consumableitem> consumableitems;
    public List<Armor> armors;
    public List<Key> keys;

    //retornar os items pilo id
    public Weapons GetWeapons(int itemId)
    {
        foreach (var item in weapons) // cada item q yem na lista de weapons
        {
            if(item.itemID == itemId)//procura e se tiver o mesmo id retorna
            {
                return item;
            }
        }
        return null; // se nao achar
    }

    public Consumableitem GetConsumableitem(int itemId)
    {
        foreach (var item in consumableitems) // cada item q yem na lista de weapons
        {
            if (item.itemID == itemId)//procura e se tiver o mesmo id retorna
            {
                return item;
            }
        }
        return null; // se nao achar
    }

    public Armor GetArmor(int itemId)
    {
        foreach (var item in armors) // cada item q yem na lista de weapons
        {
            if (item.itemID == itemId)//procura e se tiver o mesmo id retorna
            {
                return item;
            }
        }
        return null; // se nao achar
    }

    public Key GetKey(int itemId)
    {
        foreach (var item in keys) // cada item q yem na lista de weapons
        {
            if (item.itemID == itemId)//procura e se tiver o mesmo id retorna
            {
                return item;
            }
        }
        return null; // se nao achar
    }
}
