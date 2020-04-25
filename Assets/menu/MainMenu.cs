using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{



    public void PlayGame()

    {

        SceneManager.LoadScene(1);
        GameState.score = 0;

    }


    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);

    }



    public void QuitGame()

    {

        Debug.Log("QUIT!");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }



}