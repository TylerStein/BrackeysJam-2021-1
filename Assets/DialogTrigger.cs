using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogManager dialogManager;
    public List<DialogSequence> dialogSequence;
    public bool onlyOnce = true;
    public bool useTrigger = true;
    public LayerMask triggerMask = new LayerMask();

    private bool hasBeenTriggered = false;

    private void Start() {
        if (!dialogManager) dialogManager = FindObjectOfType<DialogManager>();
    }

    public void TriggerDialog() {
        if (hasBeenTriggered && onlyOnce) return;
        hasBeenTriggered = true;
        dialogManager.StartDialog(dialogSequence);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (hasBeenTriggered && onlyOnce) return;
        if (TestLayer(collision.gameObject.layer)) {
            hasBeenTriggered = true;
            dialogManager.StartDialog(dialogSequence);
        }
    }

    private bool TestLayer(int otherLayer) {
        return triggerMask == (triggerMask.value | (1 << otherLayer));
    }
}
