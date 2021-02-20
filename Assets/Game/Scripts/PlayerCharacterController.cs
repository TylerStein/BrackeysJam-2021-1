using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public PauseManager pauseManager;
    public MovementController groundMovementController;

    public Transform spriteTransform;
    public SpriteRenderer spriteRenderer;
    public CharacterAnimator animator;
    public CharacterAudioController audioController;

    public bool canGrab = false;
    public ContactFilter2D grabFilter;
    public Rigidbody2D grabbedBody;
    public Vector3 grabbedBodyInitialOffset = Vector3.zero;
    public float grabDistance;
    public float grabForce = 100f;

    public bool isIndividuallyControlled = true;
    public int minFramesRising = 10;
    private int framesRising = 0;

    // Start is called before the first frame update
    void Start() {
        if (!pauseManager) pauseManager = FindObjectOfType<PauseManager>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (pauseManager.IsPaused) return;

        if (groundMovementController.Simulating && !grabbedBody) {
            spriteRenderer.flipX = (groundMovementController.LastDirection > 0f);
        }

        if (isIndividuallyControlled) {
            audioController.SetIsMoving(groundMovementController.IsGrounded && Mathf.Abs(groundMovementController.Velocity.x) != 0f);
            animator.SetGrounded(groundMovementController.IsGrounded);
            animator.SetBlocked(groundMovementController.IsHorizontalBlocked);

            if (groundMovementController.Velocity.y > 0.5f) {
                framesRising++;
                if (framesRising >= 10) animator.SetRising(true);
            } else {
                animator.SetRising(false);
                framesRising = 0;
            }
        }
    }

    public void SetPhysicsEnabled(bool enabled) {
        groundMovementController.SetSimulated(enabled);
    }

    public void Jump() {
        if (groundMovementController.Jump()) {
            animator.TriggerJump();
        }
    }

    public void HoldJump() {
        groundMovementController.HoldJump();
    }

    public void ReleaseJump() {
        groundMovementController.ReleaseJump();
    }

    public void StartGrab() {
        if (!canGrab) return;
        RaycastHit2D[] grabHits = new RaycastHit2D[3];
        int grabHitCount = Physics2D.Raycast(transform.position, Vector2.right * groundMovementController.LastDirection, grabFilter, grabHits, grabDistance);
        Debug.DrawLine(transform.position, transform.position + (Vector3.right * groundMovementController.LastDirection * grabDistance), Color.blue, 1.0f);
        if (grabHitCount > 0) {
            grabbedBody = grabHits[0].rigidbody;
            grabbedBody.isKinematic = true;
            grabbedBodyInitialOffset = grabbedBody.transform.position - transform.position;
        }
    }

    public void StopGrab() {
        if (grabbedBody) {
            grabbedBody.isKinematic = false;
            grabbedBody = null;
        }
    }

    public void Move(float horizontal, float deltaTime) {
        // if (grabbedBody) grabbedBody.AddForce(Vector2.right * horizontal * grabForce, ForceMode2D.Force);
        if (grabbedBody) {
            Vector3 adjustedPosition = transform.position + grabbedBodyInitialOffset + (Vector3.up * 0.01f);
            grabbedBody.position = adjustedPosition;
            // grabbedBody.velocity = new Vector2(groundMovementController.Velocity.x, 0f);
        }
        groundMovementController.Move(horizontal, deltaTime);
        animator.SetWalking(Mathf.Abs(horizontal) > 0.25f);
    }

    public void TeleportTo(Vector3 target) {
        transform.position = target;
        groundMovementController.SetVelocity(Vector2.zero);
        groundMovementController.SetFallFrames(0);
    }

    public void FlipSpriteX(bool flipX) {
        spriteRenderer.flipX = flipX;
    }

    internal void TeleportTo(Transform catRideAnchor) {
        throw new NotImplementedException();
    }

    public void SetIndividuallyControlled(bool value) {
        isIndividuallyControlled = value;
        if (!isIndividuallyControlled) {
            audioController.SetIsMoving(false);
            animator.SetBlocked(false);
            animator.SetGrounded(true);
            animator.SetRising(false);
            animator.SetWalking(false);
        } else {
            groundMovementController.SetVelocity(Vector2.zero);
        }
    }

    private void OnDrawGizmos() {
        if (grabbedBody) {
            Gizmos.DrawLine(transform.position, grabbedBody.position);
        }
    }
}
