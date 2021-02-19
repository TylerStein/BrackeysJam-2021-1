using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public Animator ElevatorAnimator;
    public List<GameObject> Objects;

    public ElevatorDoors EntranceDoors;
    public ElevatorDoors ExitDoors;

    public Transform PlayerAnchor;
    public PlayerController PlayerController;

    public string catTag = "Cat";
    public string robotTag = "Robot";

    public float liftTime = 5f;
    public bool liftOnEnter = true;
    public bool isMoving = false;
    public bool didLift = false;

    public bool requireCat = true;
    public bool requireRobot = true;
    public bool requireClear = true;

    private void Start() {
        PlayerController = FindObjectOfType<PlayerController>();
        ElevatorAnimator.SetBool("lift", !liftOnEnter);
    }

    public void CheckObjects() {
        if (isMoving || didLift) return;

        bool hasRobot = false;
        bool hasCat = false;
        bool isClear = true;
        for (int i = 0; i < Objects.Count; i++) {
            if (Objects[i].tag == robotTag) hasRobot = true;
            else if (Objects[i].tag == catTag) hasCat = true;
            else isClear = false;
        }

        if (requireCat && requireRobot) {
            if (hasRobot == true && PlayerController.catIsRiding == true) {
                MoveElevator();
                return;
            }
        }

        if (requireCat == true && hasCat == false) return;
        else if (requireRobot == true && hasRobot == false) return;
        else if (requireClear == true && isClear == false) return;
        else {
            MoveElevator();
        }
    }

    public void MoveElevator() {
        isMoving = true;
        PlayerController.TeleportTo(PlayerAnchor.position);

        EntranceDoors.SetOpen(false);
        EntranceDoors.CloseEvent.AddListener(() => {
            EntranceDoors.CloseEvent.RemoveAllListeners();
            ElevatorAnimator.SetBool("lift", true);
            StartCoroutine(LiftRoutine());
        });

        didLift = true;
        liftOnEnter = !liftOnEnter;
    }

    private IEnumerator LiftRoutine() {
        yield return new WaitForSeconds(liftTime);
        ExitDoors.SetOpen(true);
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Objects.Add(collision.gameObject);
        CheckObjects();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Objects.Remove(collision.gameObject);
        CheckObjects();
    }
}
