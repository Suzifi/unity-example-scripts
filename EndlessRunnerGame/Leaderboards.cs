using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboards : MonoBehaviour
{
    [SerializeField] private Text[] scorePlacements = new Text[10];

    void Start()
    {
        // Fill the list with child obejcts
        for (int i = 0; i < scorePlacements.Length; i++)
        {
            scorePlacements[i] = transform.GetChild(i).GetComponent<Text>();
        }

        UpdateScoreList();
    }

    private void UpdateScoreList()
    {
        // Get high scores from the GameManager and update the list
        for (int i = 0; i < scorePlacements.Length; i++)
        {
            scorePlacements[i].text = GameManager.manager.GetHighScoreAt(i);
        }
    }

    public void GoToMenu()
    {
        transform.parent.gameObject.SetActive(false);
    }
}