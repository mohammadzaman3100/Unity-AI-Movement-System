using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScriptTest2 : MonoBehaviour
{
    // Components
    private Animator animator;
    private NavMeshAgent agent;

    // Targets
    private Transform player;
    private Vector3 lastSeenPosition;

    // Detection
    private RaycastHit hit;
    public GameObject nose;
    public float range = 15f;
    public float fieldOfView = 90f;
    public float lookTime = 10f;
    private float timeLooking = 0f;

    // States
    private enum State
    {
        PATROL,
        CHASE,
        ATTACK
    }

    private State currentState;

    // Patrol
    public List<Vector3> patrolPoints;
    private int curIndex = 0;
    private Vector3 currentPoint;
    private int listMax;

    // Attack
    public float attackRange = 1f;
    public float attackRate = 1f;
    private float timeSinceLastAttack = 0f;

    void Start()
    {
        // Initialize components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Initialize patrol
        listMax = patrolPoints.Count;
        currentPoint = patrolPoints[0];
        currentState = State.PATROL;
    }

    void Update()
    {
        // Update current state
        switch (currentState)
        {
            case State.PATROL:
                Patrol();
                break;
            case State.CHASE:
                Chase();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        // Look at next patrol point
        this.transform.LookAt(currentPoint);

        // Move to next patrol point
        agent.destination = currentPoint;

        // Check if at patrol point
        if (Vector3.Distance(this.transform.position, currentPoint) < 3f)
        {
            // Set next patrol point
            curIndex += 1;
            if (curIndex < listMax)
            {
                currentPoint = patrolPoints[curIndex];
            }
            else
            {
                curIndex = 0;
                currentPoint = patrolPoints[curIndex];
            }
        }

        // Check if player is within range and in view
        if (Vector3.Distance(nose.transform.position, player.position) <= range && IsInFieldOfView())
        {
            // Set last seen position and start chasing player
            lastSeenPosition = player.position;
            currentState = State.CHASE;
        }
    }

    void Chase()
    {
        // Look at player
        this.transform.LookAt(player);

        // Update time looking for player
        timeLooking += Time.deltaTime;
        // Chase player
        agent.destination = player.position;
        
        // Check if player is within attack range
        if (Vector3.Distance(this.transform.position, player.position) <= attackRange)
        {
            // Attack player
            currentState = State.ATTACK;
        }
        
        // Check if player is out of range or out of sight
        if (Vector3.Distance(nose.transform.position, player.position) > range || !IsInFieldOfView())
        {
            // Reset time looking and return to patrol
            timeLooking = 0f;
            currentState = State.PATROL;
        }
    }
    
    void Attack()
    {
        // Look at player
        this.transform.LookAt(player);
        
        // Check if it's time to attack again
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackRate)
        {
            // Reset attack timer
            timeSinceLastAttack = 0f;
            
            // Play attack animation
            animator.SetTrigger("Attack");
        }
        
        // Check if player is out of range
        if (Vector3.Distance(this.transform.position, player.position) > attackRange)
        {
            // Return to chasing player
            currentState = State.CHASE;
        }
    }
    
    bool IsInFieldOfView()
    {
        // Check if player is within field of view
        Vector3 direction = player.position - nose.transform.position;
        float angle = Vector3.Angle(direction, nose.transform.forward);
        return angle < fieldOfView / 2;
    }
}