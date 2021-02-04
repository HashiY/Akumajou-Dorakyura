using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerSkill // enumerador 0 e 1 , para saber qual skill desb
{
    dash, doubleJump
}

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public Transform groundCheck;
    public float jumpForce;
    public float fireRate;
    public Consumableitem item;
    public int maxHealth;
    public int maxMana;
    public int strength;
    public int defense;
    public int souls;
    public float dashForce;
    public bool doubleJumpSkill = false; // item desb
    public bool dashSkill = false;
    public int manaCost;
    public Rigidbody2D projectile; // para adicionar força

    private float speed;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool onGroound;
    private bool jump = false;
    private bool doublejump;
    public Weapons weaponEquipped;
    private Animator anim;
    private Attack attack;
    private float nextAttack;
    private int health;
    private int mana;
    public Armor armor;
    private bool canDamage = true;
    private SpriteRenderer sprite;
    private bool isDead = false;
    private bool dash = false;
    private GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attack = GetComponentInChildren<Attack>();
        sprite = GetComponent<SpriteRenderer>();
        
        gm = GameManager.gm;
        SetPlayer();
        FindObjectOfType<UIManager>().UpdateUI(); // no  tutorial tira, mas aqui nao funciona
    }

    
    void Update()
    {
        if (!isDead)
        {
            onGroound = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (onGroound)
            {
                anim.SetBool("Jump", false);
                
                doublejump = false;
            }
            if (Input.GetButtonDown("Jump") && (onGroound || (!doublejump && doubleJumpSkill)))
            {
                jump = true;
                
                if (!doublejump && !onGroound)
                {
                    anim.SetBool("Jump", true);
                    doublejump = true;
                    
                }
            }
            if (Input.GetButtonDown("Fire1") && Time.time > nextAttack && weaponEquipped != null)
            {
                dash = false;
                anim.SetTrigger("Attack");
                attack.PlayAnimation(weaponEquipped.animation);
                nextAttack = Time.time + fireRate;
            }
            if (Input.GetButtonDown("Fire3"))
            {
                UseItem(item);
                Inventory.inventory.RemoveItem(item);
                FindObjectOfType<UIManager>().UpdateUI();
            }
            if (Input.GetButtonDown("Dash") && onGroound && !dash && dashSkill) //GetKeyDown(KeyCode.Q)
            {
                rb.velocity = Vector2.zero;
                anim.SetTrigger("Dash");
            }
            else if (Input.GetButtonDown("Fire2") && mana >= manaCost)
            {
                // chama na cena no local do player com a propria rotaçao
                Rigidbody2D temProjectile = Instantiate(projectile, transform.position, transform.rotation);
                temProjectile.GetComponent<Attack>().SetWeapon(50);
                if (facingRight) // direita 右
                {
                    temProjectile.AddForce(new Vector2(5, 10),ForceMode2D.Impulse);// força da 手裏剣
                }
                else //esquerda 左
                {
                    temProjectile.AddForce(new Vector2(-5, 10), ForceMode2D.Impulse);// força da 手裏剣
                }

                mana -= manaCost;
                FindObjectOfType<UIManager>().UpdateUI();
            }
        }

        
    }

    private void FixedUpdate() // movimentaçao , atualizado a cada tepo fixo
    {
        if (!isDead)
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (canDamage && !dash) // para conseguir ter o mov do dano , pois se basei no de baixo
                rb.velocity = new Vector2(h * speed, rb.velocity.y);

            anim.SetFloat("Speed", Mathf.Abs(h)); // h pode ser -1,0,1 entao coloca mthf para nao ficar -

            if (h > 0 && !facingRight)
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if (jump)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Jump", true);
                rb.AddForce(Vector2.up * jumpForce);
                jump = false;

            }
            if (dash)
            {
                int hforce = facingRight ? 1 : -1; // se for verdade 1 e falso -1
                rb.velocity = Vector2.left * dashForce * hforce;
            }
        }
        

    }
    void Flip() // direita esquerda
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1; // para a escala viraar para esquerda e direita
        transform.localScale = scale; // para atualizar 
    }
    public void AddWeapon(Weapons weapon)
    {
        weaponEquipped = weapon;
        attack.SetWeapon(weaponEquipped.damage);
    }
    public void AddArmor(Armor item)
    {
        armor = item;
        defense = armor.defense;
    }

    public void UseItem(Consumableitem item)
    {
        health += item.healthGain;
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
        mana += item.manaGain;
        if(mana >= maxMana)
        {
            mana = maxMana;
        }
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetMana()
    {
        return mana;
    }
    public void TakeDamage(int damage)
    {
        if (canDamage)
        {
            canDamage = false;
            health -= (damage - defense);
            FindObjectOfType<UIManager>().UpdateUI();
            if(health <= 0)
            {
                anim.SetTrigger("Daed");
                Invoke("ReloadScene", 3f);
                isDead = true;
            }
            else
            {
                StartCoroutine(DamageCoroutine());
            }
        }
    }
    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.6f; i+= 0.2f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        canDamage = true;
    }

    void ReloadScene() // so carrega a cena atual
    {
        Souls.instance.gameObject.SetActive(true);
        Souls.instance.souls = souls;
        Souls.instance.transform.position = transform.position; // propria posiçao

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DashTrue() // usado para a animation
    {
        dash = true;
    }
    public void DashFalse() // usado para a animation
    {
        dash = false;
    }

    public void SetPlayerSkill(PlayerSkill skill) // desb skill
    {
        if(skill == PlayerSkill.dash)
        {
            dashSkill = true;
        }
        else if ( skill == PlayerSkill.doubleJump)
        {
            doubleJumpSkill = true;
        }
    }

    public void SetPlayer()
    {
        Vector3 playerPos = new Vector3(gm.playerPosX, gm.playerPosY, 0);
        transform.position = playerPos;
        maxHealth = gm.health;
        maxMana = gm.mana;
        speed = maxSpeed;
        health = maxHealth;
        mana = maxMana;
        strength = gm.strength;
        souls = gm.souls;
        doubleJumpSkill = gm.canDoubleJump;
        dashSkill = gm.canBackDash;
        if (gm.currentArmorId > 0)
            AddArmor(Inventory.inventory.itemDataBase.GetArmor(gm.currentArmorId));
        if (gm.currentWeaponId > 0)
            AddWeapon(Inventory.inventory.itemDataBase.GetWeapons(gm.currentWeaponId));
    }

    public bool GetSkill(PlayerSkill skill) // para apgar se ja tiver a skill do cenario
    {
        if (skill == PlayerSkill.dash)
        {
            return dashSkill;
        }
        else if (skill == PlayerSkill.doubleJump)
        {
            return doubleJumpSkill;
        }
        else return false;
    }
}
