using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public GameManager gameManager;
    public MovementController groundMovementController;

    public Transform spriteTransform;
    public SpriteRenderer spriteRenderer;
    public CharacterAnimator animator;
    public CharacterAudioController audioController;

    public bool isIndividuallyControlled = true;
    public int minFramesRising = 10;
    private int framesRising = 0;

    // Start is called before the first frame update
    void Start() {
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (gameManager.IsPaused) return;

        if (groundMovementController.Simulating) {
            spriteRenderer.flipX = (groundMovementController.LastDirection > 0f);
        }

        if (isIndividuallyControlled) {
            audioController.SetIsMoving(groundMovementController.IsGrounded && Mathf.Abs(groundMovementController.Velocity.x) > 0.5f);
            animator.SetGrounded(groundMovementController.IsGrounded);

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

    public void Move(float horizontal, float deltaTime) {
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
}
