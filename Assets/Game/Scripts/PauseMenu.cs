using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public PauseManager pauseManager;
    // Start is called before the first frame update
    void Start()
    {
        HideMenu();
        if (!pauseManager) pauseManager = FindObjectOfType<PauseManager>();
        pauseManager.PauseEvent.AddListener((isPaused) => {
            if (pauseManager.IsInPauseMenu) ShowMenu();
            else HideMenu();
        });
    }

    public void ShowMenu() {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideMenu() {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
