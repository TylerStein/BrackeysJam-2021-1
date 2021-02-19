using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorDoors : MonoBehaviour
{
    public Animator animator;
    public bool isOpen = false;

    public float openTime = 1f;
    public float closeTime = 1f;

    public UnityEvent CloseEvent;
    public UnityEvent OpenEvent;

    private void Start() {
        Apply();
    }

    public void SetOpen(bool open) {
        isOpen = open;
        Apply();
    }

    private void Apply() {
        animator.SetBool("open", isOpen);

        if (isOpen) StartCoroutine(OpenRoutine());
        else StartCoroutine(CloseRoutine());
    }

    private IEnumerator OpenRoutine() {
        yield return new WaitForSeconds(openTime);
        OpenEvent.Invoke();
    }

    private IEnumerator CloseRoutine() {
        yield return new WaitForSeconds(closeTime);
        CloseEvent.Invoke();
    }
}
