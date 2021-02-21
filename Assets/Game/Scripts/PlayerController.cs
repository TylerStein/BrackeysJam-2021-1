using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public bool isRobot = true;
    public bool catIsRiding = true;
    public bool catHasFoundRobot = true;

    public float catRideMinDistance = 1.25f;
    public float levelYBound = -10f;

    public bool disablePlayerInput = false;

    public Transform robotTrasnform;
    public Transform catTransform;
    public Transform catRideAnchor;
    public Transform cameraAnchor;

    public PlayerCharacterController robotController;
    public PlayerCharacterController catController;

    public CheckpointController checkpointController;
    public PauseManager pauseManager;
    public PlayerInput playerInput;

    public float minCatWarpFXDist = 0.5f;
    public TeleportFX catWarpFX;

    public SpriteRenderer catRidingSprite;
    public Collider2D catRideCollider;
    public ContactFilter2D catRideCheckFilter;
    [SerializeField] private Collider2D[] catRideCollisions = new Collider2D[1];

    public CameraController cameraController;
    public float catCameraSize = 1.5f;
    public float robotCameraSize = 3.5f;
    public bool wasPausedLastFrame = false;

    public UnityEvent CatJoinPlayerEvent = new UnityEvent();

    private void Start() {
        if (!cameraController) cameraController = FindObjectOfType<CameraController>();
        if (!pauseManager) pauseManager = FindObjectOfType<PauseManager>();
        if (!playerInput) playerInput = FindObjectOfType<PlayerInput>();
        pauseManager.PauseEvent.AddListener((isPaused) => {
            if (isPaused) {
                robotController.SetPhysicsEnabled(false);
                catController.SetPhysicsEnabled(false);
            } else {
                robotController.SetPhysicsEnabled(true);
                if (!catIsRiding) catController.SetPhysicsEnabled(true);
            }
        });
    }

    // Update is called once per frame
    void Update() {
        if (pauseManager.IsPaused) {
            wasPausedLastFrame = true;
            return;
        }

        if (wasPausedLastFrame) {
            wasPausedLastFrame = false;
            return;
        }

        if (!disablePlayerInput && playerInput.UseDown) {
            if (isRobot) {
                ControlCat();
            } else {
                ControlRobot();
            }
        }

        if (!disablePlayerInput && playerInput.SecondaryDown) {
            if (isRobot) robotController.StartGrab();
            else catController.StartGrab();
        }

        if (!disablePlayerInput && playerInput.SecondaryUp) {
            if (isRobot) robotController.StopGrab();
            else catController.StopGrab();
        }

        if (!isRobot && !catIsRiding) {
            if (!disablePlayerInput && playerInput.JumpDown) catController.Jump();
            if (!disablePlayerInput && playerInput.Jump) catController.HoldJump();
            if (playerInput.JumpUp) catController.ReleaseJump();
        }

        if (robotTrasnform.position.y < levelYBound || catTransform.position.y < levelYBound) {
            Respawn();
        }
    }

    private void FixedUpdate() {
        if (pauseManager.IsPaused || wasPausedLastFrame) return;

        if (isRobot) {
            cameraAnchor.position = robotTrasnform.position;
            cameraController.targetProjectionSize = robotCameraSize;
            catRidingSprite.flipX = robotController.spriteRenderer.flipX;

            if (!catIsRiding) {
                float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
                if (catDist <= catRideMinDistance) {
                    if (CollisionTestCatRiding()) {
                        SetCatRiding();
                    }
                }
            } else {
                catTransform.position = catRideAnchor.position;
            }
        } else {
            if (catController.groundMovementController.CheckStuck()) {
                catController.TeleportTo(catRideAnchor.position);
                return;
            }

            cameraAnchor.position = catTransform.position;
            cameraController.targetProjectionSize = catCameraSize;

            if (catIsRiding) {
                StopCatRiding();
            }

            if (catHasFoundRobot == false) {
                float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
                if (catDist <= catRideMinDistance) {
                    catHasFoundRobot = true;
                    ControlRobot();
                    if (CollisionTestCatRiding()) {
                        SetCatRiding();
                    }
                    CatJoinPlayerEvent.Invoke();
                }
            }
        }

        float horizontal = disablePlayerInput ? 0f : playerInput.MoveInput.x;
        if (isRobot) robotController.Move(horizontal, Time.fixedDeltaTime);
        else catController.Move(horizontal, Time.fixedDeltaTime);
    }

    public void SetControlled(bool canControl) {
        disablePlayerInput = !canControl;
    }

    public void TeleportTo(Vector3 position) {
        if (!catIsRiding && CollisionTestCatRiding()) {
            SetCatRiding();
        } else if (!catIsRiding) {
            catController.TeleportTo(catRideAnchor);
        }

        robotController.TeleportTo(position);
    }

    public void ControlRobot() {
        if (catHasFoundRobot == false) return;

        isRobot = true;
        robotController.SetIndividuallyControlled(true);
        catController.SetIndividuallyControlled(false);
    }

    public void ControlCat() {
        isRobot = false;
        if (catIsRiding) {
            StopCatRiding();
        }
        robotController.SetIndividuallyControlled(false);
        catController.SetIndividuallyControlled(true);
    }

    public void SetCatRiding() {
        float catWarpDist = Vector2.Distance(catTransform.position, catRideAnchor.position);
        if (catWarpDist > minCatWarpFXDist) {
            catWarpFX.PlayAt(catTransform.position);
        }
        catIsRiding = true;
        catRideCollider.isTrigger = false;
        catRidingSprite.enabled = true;
        catController.groundMovementController.SetVelocity(Vector2.zero);
        catController.SetPhysicsEnabled(false);
        catController.TeleportTo(catRideAnchor.position);
        catController.gameObject.SetActive(false);
    }

    public bool CollisionTestCatRiding() {
        int collisions = catRideCollider.OverlapCollider(catRideCheckFilter, catRideCollisions);
        return collisions == 0;
    }

    public void StopCatRiding() {
        catIsRiding = false;
        catRideCollider.isTrigger = true;
        catRidingSprite.enabled = false;
        catController.gameObject.SetActive(true);
        catController.TeleportTo(catRideAnchor.position);
        catController.FlipSpriteX(robotController.spriteRenderer.flipX);
        catController.SetPhysicsEnabled(true);
    }

    public void Respawn() {
        if (catHasFoundRobot) {
            robotController.TeleportTo(checkpointController.GetRespawnTarget().position);
            if (CollisionTestCatRiding()) {
                SetCatRiding();
            } else {
                catController.TeleportTo(catRideAnchor);
            }
        } else {
            catController.TeleportTo(checkpointController.GetRespawnTarget().position);
        }
    }

    public void OnHazard(Hazard hazard) {
        Respawn();
    }

    public void OnEnemyHazard(EnemyController enemy) {
        Respawn();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(new Vector3(transform.position.x - 25f, levelYBound), new Vector3(transform.position.x + 25f, levelYBound));
    }
}
