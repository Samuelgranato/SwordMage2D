using UnityEngine;

public class StatePatrulha : State
{
    public Transform[] waypoints;
    int nextWaypoint;
    float speed = 2.0f;
    Rigidbody2D rb;
    private Animator animator;
    public float distance_patrol;
    public float waypoint_distance;
    public float player_dist = 3.5f;





    public override void Awake()
    {
        base.Awake();
        selfname = "StatePatrulha";

        // Criamos e populamos uma nova transição
        Transition ToPerseguindo = new Transition();
        ToPerseguindo.condition = new ConditionDistLT(transform,
            GameObject.FindWithTag("Player").transform,
            distance_patrol);
        ToPerseguindo.target = GetComponent<StatePerseguindo>();
        // Adicionamos a transição em nossa lista de transições
        transitions.Add(ToPerseguindo);


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }



    public override void Update()
    {
        gothit = GetComponent<StatePerseguindo>().gothit;

        animator.SetBool("isAttacking", false);

        animator.speed = 0.7f;



        if (normalizedDirection() > 0)
            speed = 0.5f;
        else
            speed = -0.5f;

        if (Vector2.Distance(transform.position, waypoints[nextWaypoint].position) < waypoint_distance)
        {
            nextWaypoint++;
            if (nextWaypoint >= waypoints.Length) nextWaypoint = 0;
        }

        rb.velocity = new Vector3(speed, 0);

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
        float direction = waypoints[nextWaypoint].position.x - transform.position.x;
        return (int)(direction / Mathf.Abs(direction));
    }

}