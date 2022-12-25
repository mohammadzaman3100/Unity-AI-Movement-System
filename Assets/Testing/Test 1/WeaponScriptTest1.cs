using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScriptTest1 : MonoBehaviour
{
    // Attack power of the weapon
    public int attackPower = 1;

    // Range of the weapon
    public float range = 10.0f;

    // Fire the weapon at the specified target
    public void Fire(Vector3 target)
    {
        // Find the game object at the target position
        GameObject targetObject = Physics.OverlapSphere(target, 0.1f)[0].gameObject;

        // Calculate the distance to the target
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        // Check if the target is within range
        if (distance <= range)
        {
            // Attack the target
            targetObject.GetComponent<PlayerScriptTest1>().TakeDamage(attackPower);
        }
    }
}
