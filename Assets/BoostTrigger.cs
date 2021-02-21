using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoostTrigger : MonoBehaviour
{
    [SerializeField] public Vector2 boostVelocity = Vector2.up * 20f;
    [SerializeField] public LayerMask targetMask = new LayerMask();
    [SerializeField] public List<Rigidbody2D> activeTargets = new List<Rigidbody2D>();
    [SerializeField] public bool enableFalloff = true;
    [SerializeField] public BoxCollider2D boxCollider;

    public void Start() {
        if (!boxCollider) boxCollider = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate() {
        float minY = boxCollider.bounds.min.y;
        float height = boxCollider.bounds.max.y - minY;
        for (int i = 0; i < activeTargets.Count; i++) {
            if (enableFalloff) {
                float forcePercent = 1f - (activeTargets[i].position.y - minY) / height;
                if (forcePercent > 1f) forcePercent = 1f;
                activeTargets[i].AddForce(boostVelocity *  forcePercent * Time.fixedDeltaTime);
            } else {
                activeTargets[i].AddForce(boostVelocity * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            activeTargets.Add(collision.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (TestLayer(collision.gameObject.layer)) {
            activeTargets.Remove(collision.attachedRigidbody);
        }
    }

    private bool TestLayer(int otherLayer) {
        return targetMask == (targetMask.value | (1 << otherLayer));
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < activeTargets.Count; i++) {
            Gizmos.DrawLine(transform.position, activeTargets[i].position);
        }
    }
}
