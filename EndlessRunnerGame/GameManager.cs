using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [Header("High scores from saved data")]
    [SerializeField] private HighScore[] highScores;

    [Header("Session specific")]
    public int sessionScore;
    public int lifes;
    public int lifesAtStart;

    private ScoreUI scoreUI;

    void Awake()
    {
        CreateSingleton(); // There can only be 1 GameManager, create a singleton
        LoadGame(); // Load saved data
    }

    private void Start()
    {        
        lifes = lifesAtStart;

        // Immediately go to the main menu
        SceneManager.LoadScene("Menu");

        // Fill the high score list with placeholder values if it's empty
        if (highScores.Length == 0)
        {
            highScores = new HighScore[10];
            FillHighScoreList();
        }
    }

    void CreateSingleton()
    {
        // If there is no manager yet - make this a manager
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else // If there already is a manager and another one is trying to instantiate. This should not happen because the manager is instatiated in loading scene.
        {
            Destroy(gameObject);
        }
    }

    // Increase session score and update UI text
    public void AddScore(int score)
    {
        sessionScore += score;
        scoreUI.UpdateScore(sessionScore);
    }

    // Decrease session lifes and update UI text
    public void LifeLost()
    {
        lifes--;
        scoreUI.UpdateLifes(lifes);

        // Activate game over if there are no more lifes
        if(lifes == 0)
        {
            LevelManager.levelManager.GameOver();
        }
    }

    // Reset all the values when starting a level
    public void StartLevel()
    {
        lifes = lifesAtStart;
        sessionScore = 0;
        scoreUI = FindObjectOfType<ScoreUI>();        
    }

    
    void Update()
    {
        // Go back to menu by pressing ESC anywhere
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }        
    }

    // Create a list of high score objects with placeholder values
    public void FillHighScoreList()
    {
        int score = 0;
        string name = "";
        string date = DateTime.Now.ToString("dd/MM/yyyy");

        for (int i = 0; i < 10; i++)
        {
            highScores[i] = new HighScore(score, name, date);
        }
    }

    // Get the high score int value at given index
    public int GetHighScore(int placement)
    {
        if (placement < highScores.Length)
        {
            int scoreToReturn = highScores[placement].Score;
            return scoreToReturn;
        }
        else
        {
            return 0;
        }
    }

    // Get the high score string at given index
    public string GetHighScoreAt(int placement)
    {
        if (placement < highScores.Length)
        {
            string highScoreString = (placement + 1).ToString() + " \u2605 " + highScores[placement].ToString();
            return highScoreString;
        }
        else return null;
    }

    // Create a new HighScore object and add it to the top 10 list
    public void SetHighScore(int score, string name, string date)
    {
        if (score > GetHighScore(highScores.Length - 1))
        {
            highScores[highScores.Length - 1] = new HighScore(score, name, date);
            // Sort the list after adding new score
            Array.Sort(highScores, (x, y) => y.Score.CompareTo(x.Score));
        }
    }

    public void ResetHighScore()
    {
        Array.Clear(highScores, 0, highScores.Length);
    }

    public void SaveGame()
    {
        // Save the game data to a file, serialize using binary formatter
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameStats.dat"); // Create a save file
        GameData data = new GameData();

        // Data to be saved
        data.highScores = highScores;

        bf.Serialize(file, data); // Serialize data to file
        file.Close(); // Close the file so that no one can access it
    }

    public void LoadGame()
    {
        // If there is a save file for the game, deserialize and get the saved data
        if (File.Exists(Application.persistentDataPath + "/gameStats.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameStats.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file); // Deserialize the file as GameData
            file.Close(); // Close the file after looking at it

            // Assing the saved values
            highScores = data.highScores;
        }
    }
}

// Class to save high scores
[Serializable]
class HighScore
{
    public int Score { get; set; }
    public string Name { get; set; }

    public string Date { get; set; }

    public HighScore(int score, string name, string date)
    {
        Score = score;
        Name = name;
        Date = date;
    }

    public override string ToString()
    {
        if(Score == 0)
        {
            return ""; // Return placeholders (score 0) as empty strings
        }

        return Score.ToString() + " points \u2605 " + Name + " \u2605 " + Date;
    }
}

// Class to save the game
[Serializable]
class GameData
{
    public HighScore[] highScores;
}
