using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreObject : MonoBehaviour
{
    [SerializeField] private int scoreValue;
    private AudioSource collectSound;

    void Start()
    {
        collectSound = GetComponent<AudioSource>();
    }

    public void Collected()
    {
        GameManager.manager.AddScore(scoreValue);
        collectSound.Play();
        StartCoroutine(HideTemporarily());
    }

    IEnumerator HideTemporarily()
    {
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitUntil(() => transform.position.z > 12f);

        GetComponent<MeshRenderer>().enabled = true;
    }
}
