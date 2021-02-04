using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // cria 
public class Weapons : ScriptableObject
{
    public int itemID;
    public string weaponName;
    public string description;
    public int damage;
    public Sprite image;
    public AnimationClip animation;
    public string message;
}
