using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    public LayerMask TriggerLayerMask;
    public CheckpointController CheckpointController;
    public Transform Target;

    private void Start() {
        CheckpointController = FindObjectOfType<CheckpointController>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            CheckpointController.SetCheckpoint(this);
        }
    }

    private bool TestLayer(int otherLayer) {
        return TriggerLayerMask == (TriggerLayerMask.value | (1 << otherLayer));
    }
}
