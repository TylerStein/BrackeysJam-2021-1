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
        float maxY = boxCollider.bounds.max.y;
        float minY = boxCollider.bounds.min.y;
        for (int i = 0; i < activeTargets.Count; i++) {
            if (enableFalloff) {
                float yDist = activeTargets[i].position.y - minY;
                float pct = yDist / maxY;
                if (pct <= 1f) {
                    activeTargets[i].AddForce(boostVelocity * (1f - pct) * Time.fixedDeltaTime);
                }
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
