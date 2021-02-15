using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent TriggerEnterEvent;
    public UnityEvent TriggerExitEvent;

    public LayerMask TriggerLayerMask;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            TriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            TriggerExitEvent.Invoke();
        }
    }

    private bool TestLayer(int otherLayer) {
        return (((1 << TriggerLayerMask.value) & otherLayer) != 0);
    }
}
