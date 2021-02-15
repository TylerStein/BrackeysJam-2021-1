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

    public PlayerCharacterController robotController;
    public PlayerCharacterController catController;

    public CheckpointController checkpointController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            if (isRobot) {
                isRobot = false;
                catIsRiding = false;
                catController.SetPhysicsEnabled(true);
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

        if (isRobot == false && Input.GetButtonDown("Jump")) {
            catController.Jump();
        }

        float horizontal = Input.GetAxis("Horizontal");
        if (isRobot) robotController.Move(horizontal);
        else catController.Move(horizontal);

        if (catIsRiding) {
            catTransform.position = catRideAnchor.position;
        }

        if (robotTrasnform.position.y < levelYBound || catTransform.position.y < levelYBound) {
            Respawn();
        }
    }

    public void SetCatRiding() {
        catIsRiding = true;
        catController.SetPhysicsEnabled(false);
        isRobot = true;
        catTransform.position = catRideAnchor.position;
    }

    public void Respawn() {
        robotTrasnform.position = checkpointController.GetRespawnTarget().position;
        SetCatRiding();
    }

    public void OnHazard(Hazard hazard) {
        Respawn();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(new Vector3(transform.position.x - 25f, levelYBound), new Vector3(transform.position.x + 25f, levelYBound));
    }
}
