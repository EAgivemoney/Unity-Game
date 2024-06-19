using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string levelName;

    public void loadLevel()
    {
        PlayerController.lives = 5;

        SceneManager.LoadScene(levelName);
    }
}
