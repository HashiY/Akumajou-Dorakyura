using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Text upgradeCostText;
    public Text[] attributesText;
    public GameObject upgradePanel; //para ativar e desativar

    private bool upgradeActive = false;
    private Player player;
    private int cursorIndex;
    private bool axisInUse = false; // para no joystttc no menu nao ir direto para baixo ou cima quando aperta as cetas


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (upgradeActive)
        {
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                axisInUse = false;
            }
            if (Input.GetAxisRaw("Vertical") < 0 && !axisInUse) // para baixo
            {
                axisInUse = true;
                if(cursorIndex >= attributesText.Length - 1) // inpede q vai mas doq tem
                {
                    cursorIndex = attributesText.Length - 1;
                    
                }else
                cursorIndex++;
                UpdateText();
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && !axisInUse) //Input.GetKeyDown(KeyCode.UpArrow)
            { // para cima , ten q impedir q vai mas doq tem para cima
                axisInUse = true;
                if (cursorIndex <= 0) // para nao ter erro quando ir + encima q 0
                {
                    cursorIndex = 0;
                  
                }else
                cursorIndex--;
                UpdateText();
            }
            if (cursorIndex == 0)
            {
                attributesText[0].text = "Vida: " + player.maxHealth + ">" + Mathf.RoundToInt(player.maxHealth + (player.maxHealth * 0.1f));
                attributesText[0].color = Color.green;

            }
            else if (cursorIndex == 1)
            {
                attributesText[1].text = "Mana: " + player.maxMana + ">" + Mathf.RoundToInt(player.maxMana + (player.maxMana * 0.1f));
                attributesText[1].color = Color.green;
            }
            else if (cursorIndex == 2)
            {
                attributesText[2].text = "Força: " + player.strength + ">" + Mathf.RoundToInt(player.strength + (player.strength * 0.1f));
                attributesText[2].color = Color.green;
            }
            if(Input.GetButtonDown("Submit") && player.souls >= GameManager.gm.upgradeCost) // para fazer o upgrade
            {
                player.souls -= GameManager.gm.upgradeCost;
                GameManager.gm.upgradeCost += (GameManager.gm.upgradeCost / 2);
                if(cursorIndex == 0)
                {
                    player.maxHealth += (int)(player.maxHealth * 0.1f);
                }
                else if (cursorIndex == 1)
                {
                    player.maxMana += (int)(player.maxMana * 0.1f);
                }
                else if (cursorIndex == 2)
                {
                    player.strength += (int)(player.strength * 0.1f);
                }

                UpdateText();
                FindObjectOfType<UIManager>().UpdateUI();
            }
        }
    }

    void UpdateText()
    {
        upgradeCostText.text = "Custo de souls: " + GameManager.gm.upgradeCost + " Souls: " + player.souls;
        attributesText[0].text = "Vida: " + player.maxHealth;
        attributesText[1].text = "Mana: " + player.maxMana;
        attributesText[2].text = "Força: " + player.strength;
        for (int i = 0; i < attributesText.Length; i++)
        {
            attributesText[i].color = Color.white; 
        }
    }

    public void CallUpgradeManager()
    {
        upgradeActive = !upgradeActive;
        cursorIndex = 0;
        UpdateText();
        if (upgradeActive)
        {
            upgradePanel.SetActive(true);
        }
        else
        {
            upgradePanel.SetActive(false);
        }
    }
}
