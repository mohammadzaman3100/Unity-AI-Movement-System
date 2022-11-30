using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path_Enemy_Controller : MonoBehaviour
{
    Transform target;

    public List<Vector3> patrolPoints;
    private Vector3 currentPoint;
    private int listMax;
    private int curIndex = 0;

    private Vector3 goal; //nav mesh goal position

    NavMeshAgent agent;


    void Start()
    {
        listMax = patrolPoints.Count;
        currentPoint = patrolPoints[0];
        
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal;
    }

    void FixedUpdate()
    {
        enemyPatroll();
    }


    void enemyPatroll()
    {
        this.transform.LookAt(currentPoint);

        if (Vector3.Distance(this.transform.position, currentPoint) > 3f) // Move to next goal 
        {
            goal = currentPoint; //NavMesh goal
            agent.destination = goal;
        }

        if (Vector3.Distance(this.transform.position, currentPoint) < 3f) // Goal reached. Switch goal
        {

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