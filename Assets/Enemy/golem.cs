using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class golem : MonoBehaviour
{

    private Animator animator;
    public bool isHurt;
    private float hurtdelay = 0.45f;
    private float diedelay = 1.5f;
    private bool walking;
    public int lives = 3;
    private float changetime = 1f/20f;
    public GameObject player;
    private PlayerStats playerstats;

    private AudioSource source;
    public AudioClip audio_hurt;
    public AudioClip audio_die;

    public bool gothit;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        InvokeRepeating("UpdateCollider", 0, changetime);
        playerstats = player.GetComponent<PlayerStats>();


    }

    void UpdateCollider()
    {
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        PolygonCollider2D col = gameObject.AddComponent<PolygonCollider2D>();
        col.isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void took_hit()
    {
        StartCoroutine(hurt());

    }

    IEnumerator hurt()
    {
        lives -= 1;
        gothit = true;

        if (animator.speed < 1)
        {
            animator.speed = 1;
            walking = true;
        }
        isHurt = true;
        source.PlayOneShot(audio_hurt);
        animator.SetBool("isHurt", isHurt);
        GetComponent<StateAtacando>().CancelInvoke();
        GetComponent<StateAtacando>().enabled = false; ;
        GetComponent<StatePatrulha>().enabled = false; ;
        GetComponent<StatePerseguindo>().enabled = false;
        GetComponent<StateAngryPerseguindo>().enabled = true;
        GetComponent<StateAngryPerseguindo>().invoked = false;
        GetComponent<StateAngryPerseguindo>().InstantiateExclamation();
        GetComponent<StateAngry>().invoked = false;
        GetComponent<StateAngry>().enabled = false;
        yield return new WaitForSeconds(hurtdelay);
        isHurt = false;
        animator.SetBool("isHurt", isHurt);
        if (walking)
        {
            animator.speed = 0.7f;
        }
        walking = false;

        if (lives == 0)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        animator.SetBool("isDead", true);
        CancelInvoke();
        playerstats.increaseScore(100);
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<StatePatrulha>().enabled = false;
        GetComponent<StateAtacando>().CancelInvoke();
        GetComponent<StateAtacando>().enabled = false;
        GetComponent<StatePerseguindo>().enabled = false;
        source.PlayOneShot(audio_die);


        yield return new WaitForSeconds(diedelay);



        Destroy(gameObject);

    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{


    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        print('a');
    //        other.gameObject.GetComponent<PlayerController>().took_hit();
    //    }

    //}
}
