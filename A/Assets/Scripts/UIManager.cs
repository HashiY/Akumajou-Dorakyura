using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Transform cursor;
    public GameObject[] menuOptions;
    public GameObject optionPanel;
    public GameObject itemList;
    public GameObject itemListPrefab;
    public RectTransform content;
    public Text descriptionText;
    public Scrollbar scrollVertical;
    public Text healthText, manaText, strenghText, attackText, defenseText;
    public Text healthUI, manaUI, soulsUI, potionUI; // para atualizar
    public Text messageText;

    private bool pauseMenu = false;
    private int cursorIndex = 0;
    private Inventory inventory;
    public List<ItemList> items;
    private bool itemListActive = false;
    private Player player;
    private bool isMessageActive = false; // para ver quando aparece e quando nao
    private float textTimer;
    private bool axisInUse = false; // para no joystttc no menu nao ir direto para baixo ou cima quando aperta as cetas

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.inventory;
        player = FindObjectOfType<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMessageActive)
        {
            Color color = messageText.color;
            color.a += 2f * Time.deltaTime;
            messageText.color = color; // para aparecer
            if(color.a >= 1) // o a vai de 0 a 1
            {
                isMessageActive = false;
                textTimer = 0;
            }
        }
        else if (!isMessageActive)
        {
            textTimer += Time.deltaTime;
            if(textTimer >= 2f)
            {
                Color color = messageText.color;
                color.a -= 2f * Time.deltaTime;
                messageText.color = color; // para sumir
                if (color.a <= 0) // o a vai de 0 a 1
                {
                    messageText.text = "";
                }
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            pauseMenu = !pauseMenu;
            cursorIndex = 0;
            itemListActive = false;
            descriptionText.text = "";
            itemList.SetActive(false);
            optionPanel.SetActive(true);
            UpdateAtributes();
            UpdateUI();
            if (pauseMenu)
            {
                pausePanel.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(false);
            }
        }

        if (pauseMenu)
        {
            if(Input.GetAxisRaw("Vertical") == 0)
            {
                axisInUse = false;
            }
            Vector3 cursorPosition = new Vector3();
            if (!itemListActive)
            {
                cursorPosition = menuOptions[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x - 100, cursorPosition.y, cursorPosition.z);
            }
            else if (itemListActive && items.Count > 0) // cursor na posiçao dos items da lista
            {
                cursorPosition = items[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x - 75, cursorPosition.y, cursorPosition.z);
            }
            
            if (Input.GetAxisRaw("Vertical") < 0 && !axisInUse)
            {
                axisInUse = true;
                if(!itemListActive && cursorIndex >= menuOptions.Length - 1) // total de op sao 4 e maximo 3 e nao deixa 
                {
                    cursorIndex = menuOptions.Length - 1;
                }
                else if(itemListActive && cursorIndex >= items.Count - 1)
                {
                    if (items.Count == 0)
                    {
                        cursorIndex = 0;
                    }
                    else
                    {
                        cursorIndex = items.Count - 1;
                    }
                }
                else
                cursorIndex++;

                if(itemListActive && items.Count > 0) // chamar denomo para atualizar a descriçao
                {
                    scrollVertical.value -= (1f / (items.Count - 1));// para diminuir a barra dos itens e ele mexer junto com o cursor
                    UpdateDescription();

                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && !axisInUse)
            {
                axisInUse = true;
                if (cursorIndex == 0) // para nao ter erro quando ir + encima q 0
                {
                    cursorIndex = 0;
                }else
                cursorIndex--;

                if (itemListActive && items.Count > 0) // chamar denovo para atualizar a descriçao
                {
                    scrollVertical.value += (1f / (items.Count - 1));
                    UpdateDescription();
                }
            }
            if (Input.GetButtonDown("Submit") && !itemListActive)
            {
                optionPanel.SetActive(false);
                itemList.SetActive(true);
                RefreshItemList();
                UpdateItemsList(cursorIndex);
                cursorIndex = 0;
                if(items.Count > 0 )
                UpdateDescription();
                itemListActive = true;
            }
            else if(Input.GetButtonDown("Submit") && itemListActive)
            {
                if(items.Count > 0)
                {
                    UseItem();
                }
            }
        }
    }
    void UseItem()
    {
        if (items[cursorIndex].weapons != null)
        {
            player.AddWeapon(items[cursorIndex].weapons);
        }
        else if(items[cursorIndex].consumableitem != null)
        {
            player.UseItem(items[cursorIndex].consumableitem);
            inventory.RemoveItem(items[cursorIndex].consumableitem);
            cursorIndex = 0;
            RefreshItemList();
            UpdateItemsList(2);//atualizar o q escluiu
            scrollVertical.value = 1;
        }
        else if (items[cursorIndex].armor != null)
        {
            player.AddArmor(items[cursorIndex].armor);
        }
        UpdateAtributes();
        UpdateDescription(); // mudar o cursor
    }

    void UpdateDescription()
    {
        if (items[cursorIndex].weapons != null)
        {
            descriptionText.text = items[cursorIndex].weapons.description;
        }
        else if (items[cursorIndex].consumableitem != null)
        {
            descriptionText.text = items[cursorIndex].consumableitem.description;
        }
        else if (items[cursorIndex].key != null)
        {
            descriptionText.text = items[cursorIndex].key.description;
        }
        else if (items[cursorIndex].armor != null)
        {
            descriptionText.text = items[cursorIndex].armor.description;
        }
    }
    void RefreshItemList()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
    }


    void UpdateItemsList(int option)
    {
        if(option == 0)
        {
            for (int i = 0; i < inventory.weapons.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);//instanciar como o filho de contents
                tempItem.GetComponent<ItemList>().SetUpWeapon(inventory.weapons[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if (option == 1)
        {
            for (int i = 0; i < inventory.armors.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);//instanciar como o filho de contents
                tempItem.GetComponent<ItemList>().SetUpArmor(inventory.armors[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if(option == 2)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);//instanciar como o filho de contents
                tempItem.GetComponent<ItemList>().SetUpItem(inventory.items[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
        else if (option == 3)
        {
            for (int i = 0; i < inventory.keys.Count; i++)
            {
                GameObject tempItem = Instantiate(itemListPrefab, content.transform);//instanciar como o filho de contents
                tempItem.GetComponent<ItemList>().SetUpKey(inventory.keys[i]);
                items.Add(tempItem.GetComponent<ItemList>());
            }
        }
    }
    void UpdateAtributes()
    {
        healthText.text = "Vida: " + player.GetHealth() + "/" + player.maxHealth;
        manaText.text = "Mana: " + player.GetMana() + "/" + player.maxMana;
        strenghText.text = "Força: " + player.strength;
        attackText.text = "Ataque: " + (player.strength + player.GetComponentInChildren<Attack>().GetDamage());//pois esta no obj filho dele
        defenseText.text = "Defesa: " + player.defense;
    }

    public void UpdateUI() // atualizar a ui
    {
        healthUI.text = player.GetHealth() + " / " + player.maxHealth; // nao precisa usar o tostring , pois tem o / no meio
        manaUI.text = player.GetMana() + " / " + player.maxMana;
        soulsUI.text = "Souls: " + player.souls;
        potionUI.text = "x" + inventory.CountItems(player.item);
    }

    public void SetMessage(string message) // feid out
    {
        messageText.text = message;
        Color color = messageText.color;
        color.a = 0;
        messageText.color = color; // para nao ver no começo
        isMessageActive = true;
    }
}
