using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControlScript : MonoBehaviour
{
    public static bool gamePaused;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused;
            PauseGame();
        }
    }
    void PauseGame()
    {
        if (gamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
