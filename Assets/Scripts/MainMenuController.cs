using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{




    public void StartGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            StartGame();
        }
    }
}
