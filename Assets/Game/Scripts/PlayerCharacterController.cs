using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    // public GameStateController gameStateController;
    public MovementController groundMovementController;
    // public GroundMovementController2 groundMovementController;
    // public CameraController cameraController;
    // public MusicController musicController;
    // public Animator animator;

    public Transform spriteTransform;
    public SpriteRenderer spriteRenderer;
    public CharacterAnimator animator;

    // Start is called before the first frame update
    void Start() {
        // if (!gameStateController) gameStateController = FindObjectOfType<GameStateController>();
        // if (!musicController) musicController = FindObjectOfType<MusicController>();
    }

    // Update is called once per frame
    void Update() {
        // if (gameStateController.IsPaused) return;

        if (groundMovementController.Simulating) {
            spriteRenderer.flipX = (groundMovementController.LastDirection > 0f);
        }

        animator.SetGrounded(groundMovementController.IsGrounded);
        animator.SetRising(groundMovementController.Velocity.y > 0.1f);
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
    }

    public void FlipSpriteX(bool flipX) {
        spriteRenderer.flipX = flipX;
    }
}
