using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        GameManager.manager.LifeLost();
        anim.SetTrigger("attack");

        // Disable player stunner after attacking to prevent multiple stuns in a row
        StartCoroutine(DisableStun());
    }

    IEnumerator DisableStun()
    {
        yield return new WaitForSeconds(1f);

        GetComponent<PlayerStunner>().enabled = false;

        yield return new WaitUntil(() => transform.position.z > 12f);

        GetComponent<PlayerStunner>().enabled = true;
    }
}
