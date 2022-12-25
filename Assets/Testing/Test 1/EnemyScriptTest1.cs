using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptTest1 : MonoBehaviour
{
    // Movement speed of the enemy
    public float movementSpeed = 1.0f;

    // Attack range of the enemy
    public float attackRange = 2.0f;

    // Weapon or ability prefabs for the enemy to use
    public GameObject[] weapons;

    // Reference to the player
    private GameObject player;

    void Start()
    {
        // Find the player game object
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // Move the enemy towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);

        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            // Play the attack animation
            GetComponent<Animation>().Play("Attack");

            // Use the first weapon in the weapons array
            GameObject weapon = Instantiate(weapons[0], transform.position, transform.rotation);
            weapon.GetComponent<WeaponScriptTest1>().Fire(player.transform.position);
        }
    }
}
