using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    
     
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void level1 ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void level2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void level3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }


}
