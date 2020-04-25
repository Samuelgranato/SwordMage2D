using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAngry : State
{
    // Start is called before the first frame update
    private Transform waypoint;

    float speed = 2.0f;
    Rigidbody2D rb;
    private Animator animator;
    private AudioSource source;
    public AudioClip audio_hit;
    private float hitdelay = 0.3f;
    private float hitloop = (1f / 20f) * 11f;
    public float player_dist = 3.5f;


    public override void Awake()
    {
        base.Awake();
        selfname = "StateAngry";
        source = GetComponent<AudioSource>();

        Transition toAngry = new Transition();
        toAngry.condition = new ConditionDistRanWay(transform,
            GameObject.FindWithTag("Player").transform,
            player_dist);
        toAngry.target = GetComponent<StateAngryPerseguindo>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(toAngry);
        animator = GetComponent<Animator>();


        waypoint = GameObject.FindWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        invoked = false;




    }

    public void playhitAudio()
    {
        source.PlayOneShot(audio_hit);
    }

    // Update is called once per frame
    public override void Update()
    {
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
