using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLift : MonoBehaviour
{
    public Transform MoveObject;

    public Transform PointA;
    public Transform PointB;

    public float MoveSpeed = 1f;
    public int MoveDirection = 0;
    public float MovePercent = 0f;

    private void Start() {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveDirection > 0) {
            if (MovePercent >= 1f) {
                MovePercent = 1f;
                MoveDirection = 0;
                return;
            }

            UpdatePosition();
            MovePercent += MoveSpeed * Time.deltaTime;
        } else if (MoveDirection < 0) {
            if (MovePercent <= 0f) {
                MovePercent = 0f;
                MoveDirection = 0;
                return;
            }

            UpdatePosition();
            MovePercent -= MoveSpeed * Time.deltaTime;
        }
    }

    public void UpdatePosition() {
        MoveObject.position = Vector3.Lerp(PointA.position, PointB.position, MovePercent);
    }

    public void MoveToB() {
        MoveDirection = 1;
    }

    public void MoveToA() {
        MoveDirection = -1;
    }
}
