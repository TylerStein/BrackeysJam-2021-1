using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelChangeTrigger : MonoBehaviour
{
    public LayerMask playerLayerMask;
    public string targetLevelName = "";

    public void ChangeLevel() {
        SceneManager.LoadScene(targetLevelName);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            ChangeLevel();
        }
    }
    private bool TestLayer(int otherLayer) {
        return playerLayerMask == (playerLayerMask.value | (1 << otherLayer));
    }
}
