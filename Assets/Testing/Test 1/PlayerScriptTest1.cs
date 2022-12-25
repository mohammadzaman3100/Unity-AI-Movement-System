using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptTest1 : MonoBehaviour
{
    // Maximum hit points of the player
    public int maxHitPoints = 10;

    // Current hit points of the player
    private int hitPoints;

    void Start()
    {
        // Initialize the player's hit points
        hitPoints = maxHitPoints;
    }

    // Inflict damage on the player
    public void TakeDamage(int damage)
    {
        // Reduce the player's hit points
        hitPoints -= damage;

        // Check if the player is dead
        if (hitPoints <= 0)
        {
            // Play the death animation
            GetComponent<Animation>().Play("Death");

            // Disable the player's collider to allow the death animation to play
            GetComponent<Collider>().enabled = false;

            // Destroy the player game object after the death animation finishes
            Destroy(gameObject, GetComponent<Animation>()["Death"].length);
        }
    }
}
