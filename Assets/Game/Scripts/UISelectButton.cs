using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectButton : UISelectable
{
    public Button button;
    public RectTransform selectIconAnchor;

    public void Awake() {
        //
    }

    public override RectTransform GetSelectIconAnchor() {
        return selectIconAnchor ?? GetComponent<RectTransform>();
    }

    public override void Enter(EventSystem eventSystem) {
        // ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.selectHandler);
        // highlightImage.enabled = true;
    }

    public override void Click(EventSystem eventSystem) {
        button.OnSubmit(new BaseEventData(eventSystem));
    }

    public override void Leave(EventSystem eventSystem) {
        // ExecuteEvents.Execute(button.gameObject, new BaseEventData(eventSystem), ExecuteEvents.cancelEvent);
        // highlightImage.enabled = false;
    }
}
