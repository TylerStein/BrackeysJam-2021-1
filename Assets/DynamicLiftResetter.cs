using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicLift))]
public class DynamicLiftResetter : MonoBehaviour
{
    public DynamicLift target;

    public float movePercent = 0f;

    // Start is called before the first frame update
    void Start()
    {
        movePercent = target.MovePercent;
    }

    public void ResetLift() {
        target.SnapPosition(movePercent);
        target.ResetUsed();
    }
}
