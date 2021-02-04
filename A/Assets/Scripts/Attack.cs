using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator anim;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationClip clip)
    {
        anim.Play(clip.name);
    }
    public void SetWeapon(int damageValue)
    {
        damage = damageValue;
    }
    public int GetDamage()
    {
        return damage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage + FindObjectOfType<Player>().strength);
        }
    }
}
