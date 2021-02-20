using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isRobot = true;
    public bool catIsRiding = true;
    public bool catHasFoundRobot = true;

    public float catRideMinDistance = 1.25f;
    public float levelYBound = -10f;

    public Transform robotTrasnform;
    public Transform catTransform;
    public Transform catRideAnchor;
    public Transform cameraAnchor;

    public PlayerCharacterController robotController;
    public PlayerCharacterController catController;

    public CheckpointController checkpointController;
    public GameManager gameManager;

    public SpriteRenderer catRidingSprite;

    public CameraController cameraController;
    public float catCameraSize = 1.5f;
    public float robotCameraSize = 3.5f;

    private void Start() {
        if (!cameraController) cameraController = FindObjectOfType<CameraController>();
        if (!gameManager) gameManager = FindObjectOfType<GameManager>();
        gameManager.PauseEvent.AddListener((isPaused) => {
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
        if (gameManager.IsPaused) return;

        if (Input.GetButtonDown("Fire1")) {
            if (isRobot) {
                ControlCat();
            } else {
                ControlRobot();
            }
        }

        if (isRobot) {
            cameraAnchor.position = robotTrasnform.position;
            cameraController.targetProjectionSize = robotCameraSize;
            catRidingSprite.flipX = robotController.spriteRenderer.flipX;

            if (!catIsRiding) {
                float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
                if (catDist <= catRideMinDistance) {
                    SetCatRiding();
                }
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
                    SetCatRiding();
                }
            }

            if (Input.GetButtonDown("Jump")) catController.Jump();
            if (Input.GetButton("Jump")) catController.HoldJump();
            if (Input.GetButtonUp("Jump")) catController.ReleaseJump();
        }


        if (robotTrasnform.position.y < levelYBound || catTransform.position.y < levelYBound) {
            Respawn();
        }
    }

    private void FixedUpdate() {
        if (gameManager.IsPaused) return;

        float horizontal = Input.GetAxis("Horizontal");
        if (isRobot) robotController.Move(horizontal, Time.fixedDeltaTime);
        else catController.Move(horizontal, Time.fixedDeltaTime);
    }

    public void TeleportTo(Vector3 position) {
        if (!catIsRiding) SetCatRiding();
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
        catIsRiding = true;
        catRidingSprite.enabled = true;
        catController.groundMovementController.SetVelocity(Vector2.zero);
        catController.SetPhysicsEnabled(false);
        catController.TeleportTo(catRideAnchor.position);
        catController.gameObject.SetActive(false);
    }

    public void StopCatRiding() {
        catIsRiding = false;
        catRidingSprite.enabled = false;
        catController.gameObject.SetActive(true);
        catController.TeleportTo(catRideAnchor.position);
        catController.FlipSpriteX(robotController.spriteRenderer.flipX);
        catController.SetPhysicsEnabled(true);
    }

    public void Respawn() {
        if (catHasFoundRobot) {
            robotController.TeleportTo(checkpointController.GetRespawnTarget().position);
            SetCatRiding();
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
