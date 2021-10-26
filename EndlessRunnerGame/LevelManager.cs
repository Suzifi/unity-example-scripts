using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;

    [Header("Level settings")]
    public float runningSpeed;
    [SerializeField] private float runningSpeedAtStart;
    [SerializeField] private float speedIncreasePerSecond;
    public float objectSpawnPosZ;
    public float objectResetPosZ;

    [Header("UI objects")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private InputField playerNameInput;

    [Header("Sounds and Anims")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private AudioSource soundEffects;
    [SerializeField] private AudioClip stunnedSound;

    [Header("Level events")]
    private bool gameOver;
    public bool stunned;
    public UnityEvent OnStunned; // When the player hits something and gets stunned

    private void Awake()
    {
        levelManager = this;
    }

    void Start()
    {
        GameManager.manager.StartLevel();
        runningSpeed = runningSpeedAtStart;
        gameOver = false;
    }

    
    void Update()
    {
        if(!gameOver && !stunned)
        {
            // Increase the running speed over time
            runningSpeed += speedIncreasePerSecond * Time.deltaTime;
            playerAnim.speed = runningSpeed / 10f; // Match the player animation speed with running speed
        }
        
    }

    public void RunningInterrupted(float duration)
    {
        StartCoroutine(StopRunning(duration));
    }

    IEnumerator StopRunning(float duration)
    {
        OnStunned?.Invoke();

        stunned = true;
        float runningSpeedNow = runningSpeed; // Save the current running speed

        soundEffects.PlayOneShot(stunnedSound);

        // Stop running and play stun animation
        runningSpeed = 0f;
        playerAnim.SetTrigger("stunned");

        yield return new WaitForSeconds(duration);

        // Start running again, increase running speed smoothly back to the speed before getting stunned
        runningSpeed = Mathf.Lerp(0f, runningSpeedNow, 2f);
        stunned = false;
    }

    public void GameOver()
    {
        gameOver = true;

        // Show game over UI
        gameOverPanel.SetActive(true);
        gameOverScoreText.text += GameManager.manager.sessionScore.ToString(); // Show the round's score at the end

        // Stop running
        runningSpeed = 0f;
        playerAnim.SetBool("isRunning", false);

        // Check if the score is high enough to make it to the top 10 list and save game
        CheckHighScore();
        GameManager.manager.SaveGame();
    }

    public void CheckHighScore()
    {
        // Get the 10th score on high score list
        int currentLowestHighscore = GameManager.manager.GetHighScore(9);

        // Update and save the new high score if got a new record
        if (GameManager.manager.sessionScore > currentLowestHighscore)
        {
            gameOverText.text = "New high score!\nWhat's your name?"; // Change the default game over text
            highScorePanel.SetActive(true); // Activate the panel to ask player's name
        }
    }

    // Save the score with given name after clicking SAVE button
    public void SaveScore()
    {
        string winnerName = "";

        winnerName = playerNameInput.text.ToString(); // Use given name from the input field
        highScorePanel.SetActive(false);

        string currentDate = System.DateTime.Now.ToString("dd/MM/yyyy"); // Add current date to the high score

        // Use a default name if the input field was empty
        if (winnerName == "")
        {
            winnerName = "Anonymous";
        }

        // Save the new score, save the game, and go back to the main menu
        GameManager.manager.SetHighScore(GameManager.manager.sessionScore, winnerName, currentDate);
        GameManager.manager.SaveGame();
        ToMainMenu();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
