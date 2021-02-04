using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Consumableitem : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string description;
    public Sprite image;
    public int healthGain;
    public int manaGain;
    public string message; 

}
