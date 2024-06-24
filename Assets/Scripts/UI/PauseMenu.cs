using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool opened;
    public GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (opened)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0.05f;
        }
        opened = !opened;
    }

    public void ReloadScene(){
        SceneManager.LoadScene(0);
    }

    public void Exit(){
        Application.Quit();
    }
}
