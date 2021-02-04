using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Key : ScriptableObject
{
    public int itemID;
    public string keyName;
    public string description;
    public Sprite image; //cena e no inventario
    public string message; //quando pega na cena
}
