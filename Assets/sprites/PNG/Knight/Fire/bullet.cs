using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class bullet : MonoBehaviour
{
    private float speed = 0.2f;

    public AudioClip shootSoundExplosao;
    public Animator animator;

    private AudioSource source;

    private bool hit = false;

    public bool isFacingRight;      // Se está olhando para a direita
    public GameObject player;
    private PlayerStats playerstats;

    //[SerializeField] public TextMeshProUGUI TMP_Score;
    //public GameObject explosion;





    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerstats = player.GetComponent<PlayerStats>();
        if (!isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            Vector3 position = transform.position;
            position.x -= 1f;
            transform.position = position;

        }
        else
        {
            Vector3 position = transform.position;
            position.x += 1f;
            transform.position = position;
        }
    }
    // Update is called once per frame
    void Awake()
    {




    }
    private IEnumerator WaitAndBoom(float waitTime, Collider2D collision, int score)
    {
        //StartCoroutine(explod(1f, collision));
        //Instantiate(explosion, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
        //float vol = Random.Range(volLowRange, volHighRange);
        source.PlayOneShot(shootSoundExplosao);
        //increaseScore(score);
        speed = 0;
        animator.SetBool("fire_hit", true);
        yield return new WaitForSeconds(waitTime);

        //Destroy(collision.gameObject);
        Destroy(gameObject);

    }




    void Update()
    {
        if (isFacingRight)
        {
            gameObject.transform.position += Vector3.right * speed;

        }
        else
        {
            gameObject.transform.position += Vector3.left * speed;

        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Golem" && !hit)
        {
            hit = true;
            speed = 0;
            collision.gameObject.GetComponent<golem>().took_hit();
            playerstats.increaseScore(10);
            StartCoroutine(WaitAndBoom(0.3f, collision, 10));

        }
        else
        {
            if (collision.tag == "scenario")
            {
                speed = 0;
                StartCoroutine(WaitAndBoom(0.3f, collision, 10));

            }
        }

    }

    void increaseScore(int add_score)
    {
        //score.scoreValue += add_score;
    }


}
