using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public GameObject credits;
    public GameObject mainTitle;
    public GameObject tutorial;

    private float timeToCanStart = 1f;

    void Update()
    {
        if(tutorial.gameObject.activeInHierarchy){

            timeToCanStart -= Time.deltaTime;
            if(timeToCanStart <= 0)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    GoToGame();
                }
            }
        }
    }

    public void GoToCredits()
    {
        mainTitle.SetActive(false);
        credits.SetActive(true);
    }

    public void GoToTitle()
    {
        mainTitle.SetActive(true);
        credits.SetActive(false);
    }

    public void GoToTutorial()
    {
        mainTitle.SetActive(false);
        tutorial.gameObject.SetActive(true);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
