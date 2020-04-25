using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAtacando : State
{
    // Start is called before the first frame update
    private Transform waypoint;

    float speed = 2.0f;
    Rigidbody2D rb;
    private Animator animator;
    private AudioSource source;
    public AudioClip audio_hit;
    private float hitdelay = 0.3f;
    private float hitloop = (1f/20f) * 11f;
    public float player_dist = 3.5f;


    public override void Awake()
    {
        base.Awake();
        selfname = "StateAtacando";
        source = GetComponent<AudioSource>();


        waypoint = GameObject.FindWithTag("Player").transform;

        Transition ToPerseguindo = new Transition();
        ToPerseguindo.condition = new ConditionDistRanWay(transform,
            GameObject.FindWithTag("Player").transform,
            player_dist);
        ToPerseguindo.target = GetComponent<StatePerseguindo>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToPerseguindo);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        invoked = false;

        Transition ToAngry = new Transition();
        ToAngry.condition = new ConditionAngry(gothit);
        ToAngry.target = GetComponent<StateAngry>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToAngry);




    }

    public void playhitAudio()
    {
        source.PlayOneShot(audio_hit);
    }

    // Update is called once per frame
    public override void Update()
    {
        gothit = GetComponent<StatePerseguindo>().gothit;
        if (!invoked)
        {
            InvokeRepeating("playhitAudio", hitdelay, hitloop);
            invoked = true;
        }

        animator.SetBool("isAttacking", true);
        animator.speed = 1f;

        waypoint = GameObject.FindWithTag("Player").transform;
        if (normalizedDirection() > 0)
            speed = 2.0f;
        else
            speed = -2.0f;


        rb.velocity = new Vector3(0, 0);

        // Conforme direção do personagem girar ele no eixo Y
        if (speed < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (speed > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } // se direção em x == 0 mantenha como está a rotação

    }

    int normalizedDirection()
    {
        float direction = waypoint.position.x - transform.position.x;
        return (int)(direction / Mathf.Abs(direction));
    }
}
