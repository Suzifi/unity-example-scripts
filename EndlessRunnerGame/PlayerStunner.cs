using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this component to objects that are supposed to stun the player for a while

public class PlayerStunner : MonoBehaviour
{
    private float stunTime = 1f;
    private float restoreComponentPos = 12f;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);
        if(collision.gameObject.CompareTag("Player"))
        {
            // Stun the player on collision
            LevelManager.levelManager.RunningInterrupted(stunTime);

            // Disable triggers and colliders to prevent multiple collisions in a row
            if (GetComponent<PlayerTrigger>() != null)
            {
                StartCoroutine(DisableTrigger());
            }
            if(GetComponent<CapsuleCollider>() != null)
            {
                StartCoroutine(DisableCollider());
            }
            
        }
    }
    // Disable collider until the position resets
    // To avoid multiple collisions in a row
    IEnumerator DisableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitUntil(() => transform.position.z > restoreComponentPos);

        GetComponent<CapsuleCollider>().enabled = true;
    }

    // Disable player trigger until the position resets
    // To avoid multiple collisions in a row
    IEnumerator DisableTrigger()
    {
        GetComponent<PlayerTrigger>().enabled = false;

        yield return new WaitUntil(() => transform.position.z > restoreComponentPos);

        GetComponent<PlayerTrigger>().enabled = true;
    }
}
