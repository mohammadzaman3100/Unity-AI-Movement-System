using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyScriptTest4 : MonoBehaviour
{
    // Transform of the player
    Transform target;

    // Raycast hit information
    RaycastHit hit;

    // Nose game object at the front of the enemy
    public GameObject nose;

    // Vector pointing towards the player
    Vector3 faceToPlayer;

    // Booleans to control the enemy's behavior
    public bool canRotateToPlayer = false;
    public bool canMove = false;
    public bool backToPatrol = true;

    // List of patrol points for the enemy
    public List<Vector3> patrolPoints;

    // Current patrol point the enemy is moving towards
    private Vector3 currentPoint;

    // Maximum number of patrol points
    private int listMax;

    // Current index of the patrol point list
    private int curIndex = 0;

    // Range at which the enemy can see the player
    public float range = 15f;

    // Range at which the enemy will stop chasing the player
    private float stopRange = 1f;

    // Time to keep looking for the player after losing sight of them
    private float timeToKeepLooking = 10f;

    // Time to stop looking for the player after losing sight of them
    private float timeToStopLooking = 2f;

    // Last position where the player was seen
    Vector3 lastSeenPosition;

    // NavMeshAgent component to handle movement
    NavMeshAgent agent;

    // Animator component to control animations
    Animator animator;

    // Health of the enemy
    public int health = 3;

    // Maximum health of the enemy
    public int maxHealth = 3;

    // Flag to indicate if the enemy is defeated
    public bool defeated = false;

    void Start()
    {
        // Get the player's transform
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // Get the maximum number of patrol points
        listMax = patrolPoints.Count;
        // Set the current patrol point
        currentPoint = patrolPoints[0];

        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        // Set the destination of the NavMeshAgent to the current patrol point
        agent.destination = currentPoint;

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Check if the enemy is defeated
        if (!defeated)
        {
            // Look at the player
            faceToPlayer = target.position;
            rotateToFacesPlayer();
            // Follow and attack the player if within range
            moveToPlayer();
            // Patrol if not following the player
            enemyPatroll();
        }
    }

    void rotateToFacesPlayer()
    {
        // Check if the player is within range and not behind cover
        if (Vector3.Distance(nose.transform.position, target.position) <= range)
        {
            // Check if the player is in sight using a linecast
            if (Physics.Linecast(this.transform.position, target.position, out hit))
            {
                // Debug line showing the linecast
                Debug.DrawLine(this.transform.position, target.position, Color.red);

                // If the player is in sight, set the canRotateToPlayer flag to true and update the last seen position
                if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
                {
                    canRotateToPlayer = true;
                    lastSeenPosition = target.position;
                    timeToKeepLooking = 0f;
                }
                else if (hit.collider.tag == "Obstacle" || hit.collider.tag == "Door")
                {
                    // If the player is not in sight but an obstacle is in the way, set the canRotateToPlayer flag to false and start the timer
                    canRotateToPlayer = false;
                    timeToKeepLooking += Time.deltaTime;
                }
            }
        }
        else
        {
            // If the player is not within range, set the canMove and canRotateToPlayer flags to false and set the backToPatrol flag to true
            canMove = false;
            canRotateToPlayer = false;
            backToPatrol = true;
        }

        // If the timer is less than or equal to the time to stop looking for the player, check if the canRotateToPlayer flag is true
        if (timeToKeepLooking <= timeToStopLooking)
        {
            if (canRotateToPlayer == true)
            {
                // If the canRotateToPlayer flag is true, set the canMove flag to true, set the backToPatrol flag to false, and rotate towards the player
                canMove = true;
                backToPatrol = false;
                transform.LookAt(faceToPlayer);
            }
        }
        else
        {
            // If the timer has exceeded the time to stop looking for the player, set the backToPatrol flag to true
            backToPatrol = true;
        }
    }

    void enemyPatroll()
    {
        // If the enemy is not following the player, move towards the current patrol point
        if (backToPatrol == true)
        {
            // Look at the current patrol point
            this.transform.LookAt(currentPoint);

            // Check if the enemy has reached the patrol point
            if (Vector3.Distance(this.transform.position, currentPoint) > 3f)
            {
                // Set the NavMeshAgent's destination to the current patrol point
                agent.destination = currentPoint;
            }

            // If the enemy has reached the patrol point
            if (Vector3.Distance(this.transform.position, currentPoint) < 3f)
            {
                // Set the NavMeshAgent's destination to the current patrol point
                agent.destination = currentPoint;

                // Move to the next patrol point
                curIndex += 1;
                if (curIndex < listMax)
                {
                    currentPoint = patrolPoints[curIndex];
                }
                else
                {
                    // If the end of the list has been reached, start from the beginning
                    curIndex = 0;
                    currentPoint = patrolPoints[curIndex];
                }
            }
        }
    }

    void moveToPlayer()
    {
        // Check if the enemy is within range of the player and can move
        if (Vector3.Distance(this.transform.position, lastSeenPosition) > stopRange && canMove == true)
        {
            // Set the NavMeshAgent's destination to the last seen position of the player
            agent.destination = lastSeenPosition;
        }
        else
        {
            // If the enemy is within range of the player, stop moving and attack
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(int damage)
    {
        // Decrement the enemy's health by the specified amount of damage
        health -= damage;

        // Check if the enemy's health is less than or equal to 0
        if (health <= 0)
        {
            // Set the defeated flag to true
            defeated = true;
            // Play the defeat animation
            animator.SetTrigger("Defeated");
            // Disable the NavMeshAgent
            agent.enabled = false;
            // Disable the enemy's collider
            GetComponent<Collider>().enabled = false;
            // Disable the enemy's renderer
            GetComponent<Renderer>().enabled = false;
        }
        
    }
    
}
