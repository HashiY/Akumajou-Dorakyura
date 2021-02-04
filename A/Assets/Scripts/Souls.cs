using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour
{
    public int souls;
    public static Souls instance;

    
    void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            player.souls += souls;
            FindObjectOfType<UIManager>().UpdateUI();
            gameObject.SetActive(false);
        }
    }
}
