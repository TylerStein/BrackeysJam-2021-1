using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolEvent : UnityEvent<bool> { };

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; private set; }
    public BoolEvent PauseEvent = new BoolEvent();

    public void PauseGame() {
        IsPaused = true;
        PauseEvent.Invoke(IsPaused);
    }

    public void ResumeGame() {
        IsPaused = false;
        PauseEvent.Invoke(IsPaused);
    }
}
