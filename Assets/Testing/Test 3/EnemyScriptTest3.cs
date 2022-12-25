using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyScriptTest3 : MonoBehaviour
{
    public Transform playerTransform; // The player's transform
    public NavMeshAgent navMeshAgent; // The NavMeshAgent component
    public GameObject weapon; // The weapon the enemy will use to attack the player

    // Initialize the NavMeshAgent and set its destination to the player's transform
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // If the distance is less than a certain threshold, set the NavMeshAgent's destination to the player's transform
        if (distance < 10.0f)
        {
            navMeshAgent.destination = playerTransform.position;
        }

        // If the distance is less than a certain threshold, instantiate the weapon game object and set its velocity towards the player
        if (distance < 5.0f)
        {
            GameObject weaponInstance = Instantiate(weapon, transform.position, Quaternion.identity);
            Rigidbody weaponRb = weaponInstance.GetComponent<Rigidbody>();
            weaponRb.velocity = (playerTransform.position - transform.position).normalized * 10.0f;
        }
    }
}
