using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour

{


    public GameManager gameManager;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        anim = GetComponent<Animator>(); 
        boxCollider = GetComponent<BoxCollider2D>(); 
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update() 
    {
         if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") )
        {
        rigid.AddForce(Vector2.up * jumpPower , ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
        PlaySound("JUMP");
        }
        
        


        if(Input.GetButtonUp("Horizontal"))
        {   
            rigid.velocity= new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        } 

        if(Input.GetButton("Horizontal"))
        {
        spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if(Mathf.Abs(rigid.velocity.normalized.x) < 0.3) 
        anim.SetBool("isRunning" , false);
        else
        anim.SetBool("isRunning" , true);


    }

 
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed ){
        rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y );
        }
         else if(rigid.velocity.x < maxSpeed*(-1))
         {
        rigid.velocity = new Vector2(maxSpeed*(-1),rigid.velocity.y );
         }
        if(rigid.velocity.y < 0)  
        {
         Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
         RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
         if(rayHit.collider != null ) 
         {  
            if(rayHit.distance < 0.5f)
             anim.SetBool("isJumping", false);
         }
        }
    }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Enemy") {

                if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                {
                  OnAttack(collision.transform); 
                  PlaySound("ATTACK");
                }
                else
                OnDamaged(collision.transform.position);   
                PlaySound("DAMAGED");
            }
        }


        void OnTriggerEnter2D(Collider2D collision)
        {
                if(collision.gameObject.tag == "Item")
                {
                    bool isBronze = collision.gameObject.name.Contains("Bronze");
                    bool isSilver = collision.gameObject.name.Contains("Silver");
                    bool isGold = collision.gameObject.name.Contains("Gold");

                        if(isBronze)
                        gameManager.stagePoint += 50;
                        else if(isSilver)
                        gameManager.stagePoint += 100;
                        else if(isGold)
                        gameManager.stagePoint += 300;
                        
                    collision.gameObject.SetActive(false);
                    PlaySound("ITEM");
                }

                else if (collision.gameObject.tag == "Finish")
                {
                    gameManager.NextStage();
                    PlaySound("FINISH");
                }


        }
        void OnAttack(Transform enemy)
        {
            gameManager.stagePoint += 100;

            EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
            enemyMove.OnDamaged();
        }

        void OnDamaged(Vector2 targetPos)
        {
            gameManager.HealthDown();

            gameObject.layer = 11;

            spriteRenderer.color = new Color(1,1,1,0.4f);

            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);

            Invoke("OffDamaged", 3);
            PlaySound("DAMAGED");


        }

        void OffDamaged()
        {
             gameObject.layer = 10;
             spriteRenderer.color = new Color(1,1,1,1);
        }
      
      public void OnDie()
      {
        spriteRenderer.color = new Color(1,1,1,0.4f);
        spriteRenderer.flipY = true;
        boxCollider.enabled = false;
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
      }

      public void velocityZero()
      {
        rigid.velocity = Vector2.zero;
      }

      void PlaySound(string action)
    {
    switch(action){
    case "JUMP":
        audioSource.clip = audioJump;
        break;
    case "ATTACk":
        audioSource.clip = audioAttack;
        break;
    case "DAMAGED":
        audioSource.clip = audioDamaged;
        break;
    case "ITEM":
        audioSource.clip = audioItem;
        break;
    case "DIE":
        audioSource.clip = audioDie;
        break;
    case "FINISH":
        audioSource.clip = audioFinish;
        break;
        }
        audioSource.Play();
    }

}