using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    public PlayerController PlayerController;
    public LayerMask HazardLayerMask;

    public void Start() {
        if (!PlayerController) PlayerController = FindObjectOfType<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            PlayerController.OnHazard(this);
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            PlayerController.OnHazard(this);
        }
    }

    private bool TestLayer(int otherLayer) {
        return HazardLayerMask == (HazardLayerMask.value | (1 << otherLayer));
    }
}
