using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int health;
    public GameObject itemDrop;
    public Consumableitem item;
    public int damage;
    public int souls;
    public Vector2 damageForce;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 playerDistance;
    private bool facingRight = false; // o lado do sprite(向いてる方向)
    private bool isDead = false;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // para ir atras do player
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            //para o inimigo se movimentar deacordo com a distancia do player no x,y
            playerDistance = player.transform.position - transform.position;
            if (Mathf.Abs(playerDistance.x) < 12 && Mathf.Abs(playerDistance.y) < 3) // para ser absoluto nos dois lados
            {
                rb.velocity = new Vector2(speed * (playerDistance.x / Mathf.Abs(playerDistance.x)), rb.velocity.y);
            }

            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if (rb.velocity.x > 0.5f && !facingRight) // se deixar 1 o skeleton da ムーンウォーク pios sua velo e 1
            {
                Flip();
            }
            else if (rb.velocity.x < 0.5f && facingRight)
            {
                Flip();
            }
        }
        
    }
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1; // para a escala viraar para esquerda e direita
        transform.localScale = scale; // para atualizar 
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Dead");
            FindObjectOfType<Player>().souls += souls;
            FindObjectOfType<UIManager>().UpdateUI();
            if (item != null)
            {
                GameObject tempItem = Instantiate(itemDrop, transform.position, transform.rotation);
                tempItem.GetComponent<ItemDrop>().item = item;
            }
        }
        else
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine()// se tomar o dano muda de cor
    {
        for (float i = 0; i < 0.2f; i+= 0.2f)
        {
            sprite.color = Color.red; // muda para vermelho
            yield return new WaitForSeconds(0.1f);//por 1s
            sprite.color = Color.white; // passa para o branco(volta)
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(damage);
            Vector2 newDamageForce = new Vector2(damageForce.x * (playerDistance.x / Mathf.Abs(playerDistance.x)), damageForce.y);
            //para o player ser jogado no lado q foi atacado e a força 10 pode ser mudada dependendo do inimigo com uma variavel
            // player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10 * (playerDistance.x / Mathf.Abs(playerDistance.x)), ForceMode2D.Impulse);
            //apagado pois subia na pantera
            player.GetComponent<Rigidbody2D>().AddForce(newDamageForce, ForceMode2D.Impulse);
        }
    }
}
