using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePerseguindo : State
{
    private Transform waypoint;
    int nextWaypoint;
    float speed = 2.0f;
    Rigidbody2D rb;
    private Animator animator;

    public GameObject exclamationObject;
    GameObject exclamation_instance;
    private AudioSource source;
    public AudioClip audio_sawplayer;
    public float player_dist = 3.5f;
    public float player_dist_ran = 12.0f;


    public override void Awake()
    {
        base.Awake();
        selfname = "StatePerseguindo";
        source = GetComponent<AudioSource>();


        waypoint = GameObject.FindWithTag("Player").transform;

        Transition ToAtacando = new Transition();
        ToAtacando.condition = new ConditionDistLT(transform,
            GameObject.FindWithTag("Player").transform,
            player_dist);
        ToAtacando.target = GetComponent<StateAtacando>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToAtacando);


        Transition ToPararPerseguindo = new Transition();
        ToPararPerseguindo.condition = new ConditionDistRanWay(transform,
            GameObject.FindWithTag("Player").transform,
            12.0f);
        ToPararPerseguindo.target = GetComponent<StatePatrulha>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToPararPerseguindo);
        animator = GetComponent<Animator>();

        Transition ToAngry = new Transition();
        ToAngry.condition = new ConditionAngry(gothit);
        ToAngry.target = GetComponent<StateAngry>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToAngry);

        rb = GetComponent<Rigidbody2D>();



    }

    void InstantiateExclamation()
    {
            source.PlayOneShot(audio_sawplayer);

            exclamation_instance = Instantiate(exclamationObject, transform.position, transform.rotation);
            exclamation_instance.transform.parent = gameObject.transform;
            exclamation_instance.transform.position = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z);
            StartCoroutine(DestroyExclamation(exclamation_instance));
    }

    IEnumerator DestroyExclamation(GameObject exclamation_instance)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(exclamation_instance);
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
