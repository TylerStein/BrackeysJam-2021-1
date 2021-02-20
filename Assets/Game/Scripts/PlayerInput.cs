using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;
    [SerializeField] private bool lastInputMK;
    [SerializeField] private Vector3 lastScreenMouse;
    [SerializeField] private bool pauseDown;
    [SerializeField] private bool jumpDown;
    [SerializeField] private bool jump;
    [SerializeField] private bool jumpUp;
    [SerializeField] private bool useDown;

    public Vector2 MoveInput { get => moveInput; private set => moveInput = value; }
    public Vector2 LookInput { get => lookInput; private set => lookInput = value; }

    public bool LastInputMK { get => lastInputMK; private set => lastInputMK = value; }
    public Vector3 LastScreenMouse { get => lastScreenMouse; private set => lastScreenMouse = value; }

    public bool PauseDown { get => pauseDown; private set => pauseDown = value; }
    public bool JumpDown { get => jumpDown; private set => jumpDown = value; }
    public bool Jump { get => jump; private set => jump = value; }
    public bool JumpUp { get => jumpUp; private set => jumpUp = value; }
    public bool UseDown { get => useDown; private set => useDown = value; }

    // Start is called before the first frame update
    void Start() {
        LastScreenMouse = Input.mousePosition;
        UpdateInput();
    }

    private void Update() {
        UpdateInput();
    }

    void UpdateInput() {
        UseDown = Input.GetButtonDown("Fire1");

        JumpDown = Input.GetButtonDown("Jump");
        Jump = Input.GetButton("Jump");
        JumpUp = Input.GetButtonUp("Jump");

        PauseDown = Input.GetButtonDown("Pause");
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 cLookInput = new Vector2(Input.GetAxis("CLookHorizontal"), Input.GetAxis("CLookVertical"));
        Vector3 screenMouse = Input.mousePosition;
        Vector3 mouseMove = LastScreenMouse - screenMouse;
        if (cLookInput.sqrMagnitude > Mathf.Epsilon) {
            LastInputMK = false;
        } else if (mouseMove.sqrMagnitude > Mathf.Epsilon) {
            LastInputMK = true;
        }
        lastScreenMouse = screenMouse;

        LookInput = LastInputMK ? new Vector2(mouseMove.x, mouseMove.y).normalized : cLookInput;
    }
}
