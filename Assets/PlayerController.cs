
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public CharacterController2D.CharacterCollisionState2D flags;
    public float walkSpeed = 4.0f;     // Depois de incluido, alterar no Unity Editor
    public float jumpSpeed = 8.0f;     // Depois de incluido, alterar no Unity Editor
    public float hurtSpeed = 8.0f;     // Depois de incluido, alterar no Unity Editor
    public float doubleJumpSpeed = 6.0f; //Depois de incluido, alterar no Editor
    public float gravity = 9.8f;       // Depois de incluido, alterar no Unity Editor

    public bool isGrounded;        // Se está no chão
    public bool isJumping;        // Se está pulando
    public bool isFacingRight;      // Se está olhando para a direita
    public bool doubleJumped; // informa se foi feito um pulo duplo
    public bool isDucking;
    public bool isHurt;
    public bool isHurt_from_left;
    public bool isDead;

    public bool isFalling;      // Se estiver caindo
    public bool isAttacking;      // Se estiver caindo
    public AudioClip coin;
    public AudioClip heart;

    public LayerMask mask;  // para filtrar os layers a serem analisados


    public LayerMask enemy_mask;  // para filtrar os layers a serem analisados
    public GameObject fireball;  // para filtrar os layers a serem analisados
    public float wait = 0.5f;
    private float timer = 0;
    private float timer_hurt = 0;
    private float attack_time = 0.32f;
    private float attack_delay = 0.2f;
    private float hurtdelay = 0.25f;
    private float hurtdelay_enemy = 0.3f;
    private float diedelay = 1.5f;




    private Vector3 moveDirection = Vector3.zero; // direção que o personagem se move
    private CharacterController2D characterController;    //Componente do Char. Controller

    private BoxCollider2D boxCollider;
    private float colliderSizeY;
    private float colliderOffsetY;

    private Animator animator;


    private PlayerStats playerstats;
    private AudioSource audioSource;
    public AudioClip audio_jump;
    public AudioClip audio_attack;
    public AudioClip audio_fire;
    public AudioClip audio_hurt;


    void Start()
    {
        characterController = GetComponent<CharacterController2D>(); //identif. o componente
        boxCollider = GetComponent<BoxCollider2D>();
        colliderSizeY = boxCollider.size.y;
        colliderOffsetY = boxCollider.offset.y;
        animator = GetComponent<Animator>();
        isFacingRight = true;
        playerstats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();

        InvokeRepeating("checkEnemies", 0, 5f);

    }

    void Update()
    {
        if (!isHurt)
        {
            moveDirection.x = Input.GetAxis("Horizontal"); // recupera valor dos controles
            moveDirection.x *= walkSpeed;
        }
        else
        {
            moveDirection.x *= 0.95f;
        }


        // Conforme direção do personagem girar ele no eixo Y
        if (moveDirection.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
        else if (moveDirection.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        } // se direção em x == 0 mantenha como está a rotação



        timer += Time.deltaTime;
        timer_hurt += Time.deltaTime;
        if (timer > wait && Input.GetButton("Fire1") && !isDead)
        {
            timer = 0;

            audioSource.PlayOneShot(audio_attack);
            audioSource.PlayOneShot(audio_fire);


            StartCoroutine(do_attack());


        }



        if (isGrounded && !isDead)
        {             // caso esteja no chão
            moveDirection.y = 0.0f;    // se no chão nem subir nem descer

            isJumping = false;
            doubleJumped = false; // se voltou ao chão pode faz pulo duplo


            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                isJumping = true;

                audioSource.PlayOneShot(audio_jump);

            }
        }
        else
        {
            // caso esteja pulando 
            if (Input.GetButtonUp("Jump") && moveDirection.y > 0) // Soltando botão diminui pulo
                moveDirection.y *= 0.5f;

            if (Input.GetButtonDown("Jump") && !doubleJumped) // Segundo clique faz pulo duplo
            {
                moveDirection.y = doubleJumpSpeed;
                doubleJumped = true;
                audioSource.PlayOneShot(audio_jump);

            }

        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 4f, mask);
        if (hit.collider != null && isGrounded)
        {
            transform.SetParent(hit.transform);
            if (Input.GetAxis("Vertical") < 0)
            {
                moveDirection.y = -jumpSpeed;
                StartCoroutine(PassPlatform(hit.transform.gameObject));
            }

        }
        else
        {
            transform.SetParent(null);
        }

        if (moveDirection.y < 0)
            isFalling = true;
        else
            isFalling = false;


        moveDirection.y -= gravity * Time.deltaTime;    // aplica a gravidade
        characterController.move(moveDirection * Time.deltaTime);    // move personagem    

        flags = characterController.collisionState;     // recupera flags
        isGrounded = flags.below;                // define flag de chão


        if (Input.GetAxis("Vertical") < 0 && moveDirection.x == 0 && !isDead)
        {
            if (!isDucking)
            {
                boxCollider.size = new Vector2(boxCollider.size.x, 2 * colliderSizeY / 3);
                boxCollider.offset = new Vector2(boxCollider.offset.x, colliderOffsetY - colliderSizeY / 6);
                characterController.recalculateDistanceBetweenRays();
            }
            isDucking = true;
        }
        else
        {
            if (isDucking)
            {
                boxCollider.size = new Vector2(boxCollider.size.x, colliderSizeY);
                boxCollider.offset = new Vector2(boxCollider.offset.x, colliderOffsetY);
                characterController.recalculateDistanceBetweenRays();
                isDucking = false;
            }
        }


        animator.SetFloat("movementX", Mathf.Abs(moveDirection.x / walkSpeed)); // +Normalizado
        animator.SetFloat("movementY", moveDirection.y);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isDucking", isDucking);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isDoubleJump", doubleJumped);


    }

    private void checkEnemies()
    {
        if (GameObject.FindGameObjectsWithTag("Golem").Length == 0)
        {
            SceneManager.LoadScene(3);

        }

    }

    IEnumerator PassPlatform(GameObject platform)
    {
        platform.GetComponent<EdgeCollider2D>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        platform.GetComponent<EdgeCollider2D>().enabled = true;
    }

    IEnumerator do_attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attack_delay);
        GameObject fire = Instantiate(fireball, gameObject.transform.position, Quaternion.identity);
        fire.GetComponent<bullet>().player = gameObject;
        fire.GetComponent<bullet>().isFacingRight = isFacingRight;
        yield return new WaitForSeconds(attack_time-attack_delay);
        isAttacking = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "coin" && !isDead)
        {
            AudioSource.PlayClipAtPoint(coin, this.gameObject.transform.position);
            playerstats.increaseScore(10);
            Destroy(other.gameObject);
        }

        if (other.tag == "heart" && !isDead)
        {
            AudioSource.PlayClipAtPoint(heart, this.gameObject.transform.position);
            playerstats.Heal(1);
            Destroy(other.gameObject);
        }


        if (timer_hurt > hurtdelay_enemy && other.tag == "Golem" && !isDead)
        {
            timer_hurt = 0;
            isHurt_from_left = other.transform.position.x < gameObject.transform.position.x;
            playerstats.increaseScore(-10);
            took_hit();
        }

    }

    public void took_hit()
    {
        audioSource.PlayOneShot(audio_hurt);

        isHurt = true;

        moveDirection.y = hurtSpeed;
        if (isHurt_from_left)
        {
            moveDirection.x = 3 * hurtSpeed;

        }
        else
        {
            moveDirection.x = - 3 * hurtSpeed;

        }
        characterController.move(moveDirection * Time.deltaTime);
        StartCoroutine(hurt());

    }

    IEnumerator Death()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);

        yield return new WaitForSeconds(diedelay);
        GameState.score = GetComponent<PlayerStats>().Score;
        SceneManager.LoadScene(2);

    }


    IEnumerator hurt()
    {

        isHurt = true;
        animator.SetBool("isHurt", isHurt);
        playerstats.TakeDamage(1);

        if(playerstats.Health <= 0)
        {
            StartCoroutine(Death());

        }

        yield return new WaitForSeconds(hurtdelay);
        isHurt = false;
        animator.SetBool("isHurt", isHurt);

    }

}
