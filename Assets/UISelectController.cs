using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectController : MonoBehaviour
{
    public EventSystem eventSystem;

    public UISelectable defaultSelection;
    public UISelectable currentSelection;

    public float axisThreshold = 0.75f;
    public bool axisHorizontalUsed = false;
    public bool axisVerticalUsed = false;

    public float lastHorizontal = 0f;
    public float lastVertical = 0f;

    private void Start() {
        SetSelection(currentSelection ?? defaultSelection);
        if (!eventSystem) eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        lastHorizontal = Input.GetAxis("Horizontal");
        float hAbs = Mathf.Abs(lastHorizontal);
        if (hAbs > axisThreshold) {
            if (!axisHorizontalUsed) {
                if (lastHorizontal > 0f) {
                    // Go right
                    if (currentSelection?.nextRight) {
                        SetSelection(currentSelection.nextRight);
                    }
                } else {
                    // Go left
                    if (currentSelection?.nextLeft) {
                        SetSelection(currentSelection.nextLeft);
                    }
                }
                axisHorizontalUsed = true;
                return;
            }
        } else {
            axisHorizontalUsed = false;
        }


        lastVertical = Input.GetAxis("Vertical");
        float vAbs = Mathf.Abs(lastVertical);
        if (vAbs > axisThreshold) {
            if (!axisVerticalUsed) {
                if (lastVertical > 0) {
                    // Go up
                    if (currentSelection?.nextUp) {
                        SetSelection(currentSelection.nextUp);
                    }
                } else {
                    // Go down
                    if (currentSelection?.nextDown) {
                        SetSelection(currentSelection.nextDown);
                    }
                }
                axisVerticalUsed = true;
                return;
            }
        } else {
            axisVerticalUsed = false;
        }

        if (Input.GetButtonDown("Submit")) {
            currentSelection?.Click(eventSystem);
            return;
        }
    }

    public void SetSelection(UISelectable selection) {
        if (currentSelection) {
            currentSelection.Leave(eventSystem);
        }

        currentSelection = selection;
        if (selection) {
            currentSelection.Enter(eventSystem);
        }
    }
}
