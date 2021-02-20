using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool useBounds = false;
    public bool trackingBounds = false;
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

    public Transform targetTransform;
    public Vector3 targetOffset = new Vector3(0f, 0f, -10f);

    public Camera targetCamera;
    public float targetProjectionSize = 4f;

    public float xTime = 0.6f;
    public float xMaxSpeed = 10f;

    public float yTime = 0.6f;
    public float yMaxSpeed = 8f;

    public float zTime = 0.6f;
    public float zMaxSpeed = 12f;

    public float pTime = 0.6f;
    public float pMaxSpeed = 24f;

    [SerializeField] private float _xVelocity = 0f;
    [SerializeField] private float _yVelocity = 0f;
    [SerializeField] private float _zVelocity = 0f;
    [SerializeField] private float _pVelocity = 0f;

    public PauseManager pauseManager;

    // Update is called once per frame
    private void Start() {
        if (!pauseManager) pauseManager = FindObjectOfType<PauseManager>();
        if (targetCamera == null) targetCamera = Camera.main;
        TeleportToTarget();
    }

    private void FixedUpdate() {
        if (pauseManager.IsPaused) return;

        Vector3 target = GetTarget();
        Vector3 targetPosition = target + targetOffset;

        float x = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref _xVelocity, xTime, xMaxSpeed, Time.fixedDeltaTime);
        float y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref _yVelocity, yTime, yMaxSpeed, Time.fixedDeltaTime);
        float z = Mathf.SmoothDamp(transform.position.z, targetPosition.z, ref _zVelocity, zTime, zMaxSpeed, Time.fixedDeltaTime);

        float size = Mathf.SmoothDamp(targetCamera.orthographicSize, targetProjectionSize, ref _pVelocity, pTime, pMaxSpeed, Time.fixedDeltaTime);

        Vector3 newPosition = new Vector3(x, y, z);
        Bounds newBounds = bounds;
        if (useBounds) {
            if (trackingBounds) {
                newBounds = new Bounds(bounds.center + targetPosition, bounds.size);
            }

            if (newBounds.Contains(newPosition) == false) {
                newPosition = newBounds.ClosestPoint(newPosition);
            }
        }

        transform.position = newPosition;
        targetCamera.orthographicSize = size;
    }

    public Vector3 GetTarget() {
        return targetTransform.position;
    }

    public void TeleportToTarget() {
        Vector3 targetPosition = GetTarget() + targetOffset;
        transform.position = targetPosition;
        targetCamera.orthographicSize = targetProjectionSize;
        ResetVelocity();
    }

    public void TeleportTo(Vector3 point, float pSize) {
        Vector3 targetPosition = point + targetOffset;
        transform.position = targetPosition;
        targetCamera.orthographicSize = targetProjectionSize;
        ResetVelocity();
    }

    public void ResetVelocity() {
        _xVelocity = 0f;
        _yVelocity = 0f;
        _zVelocity = 0f;
        _pVelocity = 0f;
    }

    public void OnDrawGizmosSelected() {
        if (useBounds) {
            Bounds newBounds = bounds;
            if (trackingBounds) newBounds = new Bounds(targetTransform.position + targetOffset + bounds.center, bounds.size);
            Gizmos.color = Color.yellow;
            Vector3 bottomRight = newBounds.min + (Vector3.right * newBounds.size.x);
            Vector3 topLeft = newBounds.min + (Vector3.up * newBounds.size.y);
            Gizmos.DrawLine(newBounds.min, bottomRight);
            Gizmos.DrawLine(newBounds.min, topLeft);
            Gizmos.DrawLine(bottomRight, newBounds.max);
            Gizmos.DrawLine(topLeft, newBounds.max);

        }
    }
}
