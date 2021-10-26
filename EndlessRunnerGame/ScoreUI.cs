using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text lifesText;
    
    void Start()
    {
        scoreText.text = GameManager.manager.sessionScore.ToString();
        lifesText.text = GameManager.manager.lifes.ToString();
    }

    public void UpdateScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
    }

    public void UpdateLifes(int currentLifes)
    {
        lifesText.text = currentLifes.ToString();
    }
}
