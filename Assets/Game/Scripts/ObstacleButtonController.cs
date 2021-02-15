using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ObstacleButtonController : MonoBehaviour
{
    public Animator AnimationPlayer;

    public bool canDeactivate = true;
    public bool isHeld = false;
    public bool isObstacleOpen = false;

    public LayerMask TriggerLayerMask;

    public void Update() {
        if (isHeld && !isObstacleOpen) {
            AnimationPlayer.SetBool("active", true);
            isObstacleOpen = true;
        } else if (!isHeld && isObstacleOpen && canDeactivate) {
            AnimationPlayer.SetBool("active", false);
            isObstacleOpen = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            isHeld = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            isHeld = false;
        }
    }

    private bool TestLayer(int otherLayer) {
        return TriggerLayerMask == (TriggerLayerMask.value | (1 << otherLayer));
    }
}
