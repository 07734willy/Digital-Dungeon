using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InspectionTesting : IntegrationTest, IPointerClickHandler {

	public string GetDialogText() {
        return GameObject.Find("DialogBox").GetComponentInChildren<Text>().text;
    }

    public void LeftClickTile(GameTile tile) {
        PointerEventData data = new PointerEventData(GameObject.FindObjectOfType<EventSystem>()) {
            button = PointerEventData.InputButton.Left
        };
        tile.OnPointerClick(data);
    }

    public void RightClickTile(GameTile tile) {
        PointerEventData data = new PointerEventData(GameObject.FindObjectOfType<EventSystem>()) {
            button = PointerEventData.InputButton.Right
        };
        tile.OnPointerClick(data);
    }

    public void OnPointerClick(PointerEventData eventData) {
        return;
    }
}
