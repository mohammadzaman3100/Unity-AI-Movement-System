using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path_Enemy_Controller_Version_2 : MonoBehaviour
{
    Transform target;

    RaycastHit hit;

    public GameObject nose;

    Vector3 faceToPlayer;

    public bool canRotateToPlayer = false;
    public bool canMove = false;
    public bool backToPatrol = true;

    public List<Vector3> patrolPoints;
    private Vector3 currentPoint;
    private int listMax;
    private int curIndex = 0;

    public  float range = 15f;
    private float stopRange = 1f;
    
    private float timeToKeepLooking = 10f;
    private float timeToStopLooking = 2f;

    Vector3 lastSeenPosition;

    private Vector3 goal; //nav mesh goal position

    NavMeshAgent agent;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        listMax = patrolPoints.Count;
        currentPoint = patrolPoints[0];

        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal;
    }

    void FixedUpdate()
    {
        faceToPlayer = target.position; // look at player
        rotateToFacesPlayer();
        moveToPlayer();
        enemyPatroll();
    }

    void rotateToFacesPlayer() // check of player is close and not behind cover. rotate enemy to face player
    {
        if (Vector3.Distance(nose.transform.position, target.position) <= range) // Object nose at the front of enemy, so that it doesn't "see" too far behind
        {
            if (Physics.Linecast(this.transform.position, target.position, out hit))
            {
                Debug.DrawLine(this.transform.position, target.position, Color.red);

                if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
                {
                    canRotateToPlayer = true;
                    lastSeenPosition = target.position;
                    timeToKeepLooking = 0f;
                }
                else if (hit.collider.tag == "Obstacle" || hit.collider.tag == "Door")
                {
                    canRotateToPlayer = false;
                    timeToKeepLooking += Time.deltaTime;
                }
            }
        }
        else
        {
            canMove = false;
            canRotateToPlayer = false;
            backToPatrol = true;
        }
        
        if (timeToKeepLooking <= timeToStopLooking)
        {
            if (canRotateToPlayer == true)
            {
                canMove = true;
                backToPatrol = false;
                transform.LookAt(faceToPlayer);
            }
        }
        else
        {
            backToPatrol = true;
        }
    }

    void enemyPatroll()
    {
        if (backToPatrol == true)
        {
            this.transform.LookAt(currentPoint);

            if (Vector3.Distance(this.transform.position, currentPoint) > 3f) // Move to next goal 
            {
                goal = currentPoint; //NavMesh goal
                agent.destination = goal;
            }

            if (Vector3.Distance(this.transform.position, currentPoint) < 3f) // Goal reached. Switch goal
            {
                //Debug.Log("HIT");
                //Debug.Log(currentPoint);

                goal = currentPoint; //NavMesh goal
                agent.destination = goal;

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
        }
    }
    
    void moveToPlayer()
    {
        if (Vector3.Distance(this.transform.position, lastSeenPosition) > stopRange)
        {
            if (canMove == true || timeToKeepLooking <= timeToStopLooking)
            {
                goal = lastSeenPosition; // NavMesh goal
                agent.destination = goal;
            }
        }
    }
}