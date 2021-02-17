using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public int startSceneIndex = 1;

    [Header("Main View")]
    public CanvasGroup mainGroup;
    public UISelectController mainController;
    public UISelectButton startButton;
    public UISelectButton optionsButton;
    
    [Header("Options View")]
    public CanvasGroup optionsGroup;
    public UISelectController optionsController;
    public UISelectButton backButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.button.onClick.AddListener(() => {
            SceneManager.LoadScene(startSceneIndex);
        });

        optionsButton.button.onClick.AddListener(() => {
            HideGroup(mainGroup);
            mainController.enabled = false;

            ShowGroup(optionsGroup);
            optionsController.enabled = true;
        });

        backButton.button.onClick.AddListener(() => {
            HideGroup(optionsGroup);
            optionsController.enabled = false;

            ShowGroup(mainGroup);
            mainController.enabled = true;
        });
    }

    void HideGroup(CanvasGroup group) {
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    void ShowGroup(CanvasGroup group) {
        group.alpha = 1f;
        group.blocksRaycasts = true;
        group.interactable = true;
    }
}
