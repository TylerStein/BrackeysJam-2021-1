using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolEvent : UnityEvent<bool> { };

public class PauseManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public bool IsPaused { get => _isPausedMenu || _isPausedDialog; }
    public bool IsInPauseMenu { get => _isPausedMenu; }
    public BoolEvent PauseEvent = new BoolEvent();

    private bool _isPausedMenu = false;
    private bool _isPausedDialog = false;

    private void Start() {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update() {
        if (playerInput.PauseDown) {
            if (_isPausedMenu) OnPauseMenuClose();
            else OnPauseMenuOpen();
        }
    }

    public void OnPauseMenuOpen() {
        _isPausedMenu = true;
        PauseEvent.Invoke(IsPaused);
    }
    public void OnPauseMenuClose() {
        _isPausedMenu = false;
        PauseEvent.Invoke(IsPaused);
    }

    public void OnPauseDialogOpen() {
        _isPausedDialog = true;
        PauseEvent.Invoke(IsPaused);
    }

    public void OnPauseDialogClose() {
        _isPausedDialog = false;
        PauseEvent.Invoke(IsPaused);
    }
}
