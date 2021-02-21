using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    public LayerMask TriggerLayerMask;
    public CheckpointController CheckpointController;
    public Transform Target;
    public bool hasBeenUsed = false;

    private void Start() {
        CheckpointController = FindObjectOfType<CheckpointController>();
    }

    public void OnTriggerStay2D(Collider2D collision) {
        if (hasBeenUsed) return;
        if (TestLayer(collision.gameObject.layer)) {
            CheckpointController.SetCheckpoint(this);
            hasBeenUsed = true;
        }
    }

    private bool TestLayer(int otherLayer) {
        return TriggerLayerMask == (TriggerLayerMask.value | (1 << otherLayer));
    }
}
