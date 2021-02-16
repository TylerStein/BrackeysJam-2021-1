using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isRobot = true;
    public bool catIsRiding = true;

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

    private void Start() {
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
                isRobot = false;
                StopCatRiding();
            } else {
                isRobot = true;
            }
        }

        if (isRobot && catIsRiding == false) {
            float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
            if (catDist <= catRideMinDistance) {
                SetCatRiding();
            }
        }

        if (catIsRiding) {
            catTransform.position = catRideAnchor.position;
        }

        if (robotTrasnform.position.y < levelYBound || catTransform.position.y < levelYBound) {
            Respawn();
        }

        if (isRobot) {
            cameraAnchor.position = robotTrasnform.position;
            catController.FlipSpriteX(robotController.spriteRenderer.flipX);
        } else {
            cameraAnchor.position = catTransform.position;

            if (Input.GetButtonDown("Jump")) catController.Jump();
            if (Input.GetButton("Jump")) catController.HoldJump();
            if (Input.GetButtonUp("Jump")) catController.ReleaseJump();
        }
    }

    private void FixedUpdate() {
        if (gameManager.IsPaused) return;

        float horizontal = Input.GetAxis("Horizontal");
        if (isRobot) robotController.Move(horizontal, Time.fixedDeltaTime);
        else catController.Move(horizontal, Time.fixedDeltaTime);
    }

    public void SetCatRiding() {
        catIsRiding = true;
        isRobot = true;
        float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
        if (catDist <= catRideMinDistance) {
            catController.TeleportTo(catRideAnchor.position);
        }
        catController.SetPhysicsEnabled(true);
    }

    public void StopCatRiding() {
        catIsRiding = false;
        float catDist = Vector2.Distance(robotTrasnform.position, catTransform.position);
        if (catDist <= catRideMinDistance) {
            catController.TeleportTo(catRideAnchor.position);
        }

        catController.SetPhysicsEnabled(true);
    }

    public void Respawn() {
        robotTrasnform.position = checkpointController.GetRespawnTarget().position;
        SetCatRiding();
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
