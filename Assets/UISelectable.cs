using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectable : MonoBehaviour
{
    public UISelectable nextUp;
    public UISelectable nextDown;
    public UISelectable nextLeft;
    public UISelectable nextRight;

    public virtual void Enter(EventSystem eventSystem) {
        //
    }

    public virtual void Click(EventSystem eventSystem) {
        //
    }

    public virtual void Leave(EventSystem eventSystem) {
        //
    }

    public void OnDrawGizmosSelected() {
        if (nextDown) Gizmos.DrawLine(transform.position, nextDown.transform.position);
        if (nextUp) Gizmos.DrawLine(transform.position, nextUp.transform.position);
        if (nextRight) Gizmos.DrawLine(transform.position, nextRight.transform.position);
        if (nextLeft) Gizmos.DrawLine(transform.position, nextLeft.transform.position);
    }
}
