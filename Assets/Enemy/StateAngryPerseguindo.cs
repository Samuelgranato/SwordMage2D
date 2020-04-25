using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAngryPerseguindo : State
{
    // Start is called before the first frame update
    private Transform waypoint;

    float speed = 2.0f;
    Rigidbody2D rb;
    private Animator animator;
    private AudioSource source;
    public AudioClip audio_hit;


    public GameObject exclamationObject;
    GameObject exclamation_instance;
    public AudioClip audio_sawplayer;
    public float player_dist = 3.5f;

    public override void Awake()
    {
        base.Awake();
        selfname = "StateAngryPerseguindo";
        source = GetComponent<AudioSource>();


        waypoint = GameObject.FindWithTag("Player").transform;



        // Criamos e populamos uma nova transição
        Transition ToAngry = new Transition();
        ToAngry.condition = new ConditionDistLT(transform,
            GameObject.FindWithTag("Player").transform,
           player_dist);
        ToAngry.target = GetComponent<StateAngry>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToAngry);



        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        invoked = false;    

    }

    IEnumerator DestroyExclamation(GameObject exclamation_instance)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(exclamation_instance);
    }

    public void InstantiateExclamation()
    {
        source.PlayOneShot(audio_sawplayer);

        exclamation_instance = Instantiate(exclamationObject, transform.position, transform.rotation);
        exclamation_instance.transform.parent = gameObject.transform;
        exclamation_instance.transform.position = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z);
        StartCoroutine(DestroyExclamation(exclamation_instance));
    }

    public override void Update()
    {
        gothit = GetComponent<StatePerseguindo>().gothit;


        animator.SetBool("isAttacking", false);
        animator.speed = 1f;


        waypoint = GameObject.FindWithTag("Player").transform;
        if (normalizedDirection() > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            speed = 2.0f;
        }
        else
        {
            speed = -2.0f;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        rb.velocity = new Vector3(speed, 0);
    }

    int normalizedDirection()
    {
        float direction = waypoint.position.x - transform.position.x;
        return (int)(direction / Mathf.Abs(direction));
    }
}
