using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public DynamicLift Lift;

    public Collider2D PlayerTrigger;
    public Collider2D ObstacleTrigger;

    public ElevatorDoors EntranceDoors;
    public ElevatorDoors ExitDoors;

    public Transform PlayerAnchor;
    public PlayerController PlayerController;

    public string catTag = "Cat";
    public string robotTag = "Robot";

    public bool requireCat = true;
    public bool requireRobot = true;
    public bool requireClear = true;

    public bool canTrigger = true;
    public bool lockPlayer = false;
    public int checkFrameFrequency = 15;

    [SerializeField] private Collider2D[] _colliderContacts = new Collider2D[3];
    [SerializeField] private int _checkFrame = 0;

    private void Start() {
        PlayerController = FindObjectOfType<PlayerController>();
    }

    public void Update() {
        _checkFrame++;
        if (_checkFrame >= checkFrameFrequency) {
            _checkFrame = 0;
            CheckObjects();
        }

        if (lockPlayer) {
            PlayerController.TeleportTo(PlayerAnchor.position);
        }
    }

    public void CheckObjects() {
        if (Lift.MoveDirection != 0 || canTrigger == false) return;

        if (requireClear) {
            _colliderContacts = new Collider2D[3];
            int obstacleColliderCount = ObstacleTrigger.GetContacts(_colliderContacts);
            for (int i = 0; i < obstacleColliderCount; i++) {
                if (_colliderContacts[i].tag != robotTag && _colliderContacts[i].tag != catTag) return;
            }
        }

        bool hasRobot = false;
        bool hasCat = false;

        _colliderContacts = new Collider2D[3];
        int playerColliderCount = PlayerTrigger.GetContacts(_colliderContacts);
        for (int i = 0; i < playerColliderCount; i++) {
            if (_colliderContacts[i].tag == robotTag) hasRobot = true;
            else if (_colliderContacts[i].tag == catTag) hasCat = true;
        }

        if (requireCat && requireRobot) {
            if (hasRobot == true && PlayerController.catIsRiding == true) {
                MoveElevator();
                return;
            }
        }

        if (requireCat == true && hasCat == false) return;
        else if (requireRobot == true && hasRobot == false) return;
        else {
            MoveElevator();
        }
    }

    public void MoveElevator() {
        lockPlayer = true;
        PlayerController.SetControlled(false);
        PlayerController.TeleportTo(PlayerAnchor.position);

        EntranceDoors.SetOpen(false);
        EntranceDoors.CloseEvent.AddListener(() => {
            EntranceDoors.CloseEvent.RemoveAllListeners();
            Lift.MoveDirection = 1;
            Lift.PointBEvent.AddListener(() => {
                Lift.PointBEvent.RemoveAllListeners();
                ExitDoors.SetOpen(true);
                PlayerController.SetControlled(true);
                lockPlayer = false;
            });
        });

        canTrigger = false;
    }
}
