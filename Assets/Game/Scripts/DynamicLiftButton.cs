using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class DynamicLiftButton : MonoBehaviour
{
    public Animator buttonAnimator;
    public DynamicLift lift;
    public bool canDeactivate = true;

    public LayerMask TriggerLayerMask;

    public void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            buttonAnimator.SetBool("pressed", true);
            lift.MoveToB();
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (!canDeactivate) return;

        if (TestLayer(collision.gameObject.layer)) {
            buttonAnimator.SetBool("pressed", false);
            lift.MoveToA();
        }
    }

    private bool TestLayer(int otherLayer) {
        return TriggerLayerMask == (TriggerLayerMask.value | (1 << otherLayer));
    }
}
