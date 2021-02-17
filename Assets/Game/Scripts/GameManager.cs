using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BoolEvent : UnityEvent<bool> { };

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; private set; }
    public BoolEvent PauseEvent = new BoolEvent();

    [SerializeField] private int mainMenuLevelIndex = 0;

    public void PauseGame() {
        IsPaused = true;
        PauseEvent.Invoke(IsPaused);
    }

    public void ResumeGame() {
        IsPaused = false;
        PauseEvent.Invoke(IsPaused);
    }

    public void QuitGame() {
        SceneManager.LoadScene(mainMenuLevelIndex);
    }

    public void Update() {
        if (Input.GetButtonDown("Pause")) {
            if (IsPaused) ResumeGame();
            else PauseGame();
        }
    }
}
