using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject gameEntities;
    public GameObject menuContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame()
    {
        Debug.Log("Activating entities");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameEntities.SetActive(true);
        menuContainer.SetActive(false);
    }

    public void ReloadScene()
    {
        Debug.Log("Reloading scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
