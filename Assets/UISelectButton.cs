using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectButton : UISelectable
{
    public Button button;
    public Image highlightImage;

    public void Awake() {
        highlightImage.enabled = false;
    }

    public override void Enter(EventSystem eventSystem) {
        // ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
        highlightImage.enabled = true;
    }

    public override void Click(EventSystem eventSystem) {
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
    }

    public override void Leave(EventSystem eventSystem) {
        // ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.cancelEvent);
        highlightImage.enabled = false;
    }
}
