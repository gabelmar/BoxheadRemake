using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private bool isPaused = false;

    public static event Action OnPause;
    public static event Action OnResume;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.L))
            Time.timeScale = 0.1f;
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (!isPaused)
            {
                Time.timeScale = 0.0f;
                playerController.enabled = false;
                isPaused = true;
                OnPause?.Invoke();
            }
            else 
            {
                Time.timeScale = 1.0f;
                playerController.enabled = true;
                isPaused = false;
                OnResume.Invoke();
            }
        }   
    }
}
