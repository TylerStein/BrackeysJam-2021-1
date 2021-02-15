using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool LastInputMK { get; private set; }
    public Vector3 LastScreenMouse { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        LastScreenMouse = Input.mousePosition;
        UpdateInput();
    }

    // Update is called once per frame
    void Update() {
    }

    void UpdateInput() {
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 cLookInput = new Vector2(Input.GetAxis("CLookHorizontal"), Input.GetAxis("CLookVertical"));
        Vector3 screenMouse = Input.mousePosition;
        Vector3 mouseMove = LastScreenMouse - screenMouse;
        if (cLookInput.sqrMagnitude > Mathf.Epsilon) {
            LastInputMK = false;
        } else if (mouseMove.sqrMagnitude > Mathf.Epsilon) {
            LastInputMK = true;
        }

        LookInput = LastInputMK ? new Vector2(mouseMove.x, mouseMove.y).normalized : cLookInput;
    }
}
