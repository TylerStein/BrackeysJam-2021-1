using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private int mainMenuLevelIndex = 0;

    public void QuitGame() {
        SceneManager.LoadScene(mainMenuLevelIndex);
    }
}
