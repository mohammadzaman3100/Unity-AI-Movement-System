using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptTest4 : MonoBehaviour
{
       // Transform of the enemy
       Transform enemy;
   
       // Animator component to control animations
       Animator animator;
   
       // Rigidbody component for movement
       Rigidbody rb;
   
       // Speed of movement
       public float speed = 5f;
   
       // Range at which the player can attack
       public float attackRange = 1f;
   
       // Attack damage
       public int attackDamage = 2;
       
       
   
       void Start()
       {
           // Get the enemy's transform
           enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
           // Get the Animator component
           animator = GetComponent<Animator>();
           // Get the Rigidbody component
           rb = GetComponent<Rigidbody>();
       }
   
       void Update()
       {
           // Check if the player is within range of the enemy
           if (Vector3.Distance(transform.position, enemy.position) < attackRange)
           {
               // Play the attack animation
               animator.SetTrigger("Attack");
               // Call the TakeDamage function on the enemy and pass in the player's attack damage
               enemy.GetComponent<EnemyScriptTest4>().TakeDamage(attackDamage);
           }
           
       }
   
       void FixedUpdate()
       {
           // Get input for horizontal and vertical movement
           float moveHorizontal = Input.GetAxis("Horizontal");
           float moveVertical = Input.GetAxis("Vertical");
   
           // Create a movement vector based on the input
           Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
   
           // Set the Rigidbody's velocity based on the movement vector and the speed
           rb.velocity = movement * speed;
       }
       
}