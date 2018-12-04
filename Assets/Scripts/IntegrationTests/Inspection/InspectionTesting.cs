using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public abstract class InspectionTesting : IntegrationTest, IPointerClickHandler {

	public string[] GetDialogText() {
        return GameObject.Find("Log").GetComponentsInChildren<Text>().Select(x => x.text).ToArray<string>();
    }

    public bool CompareText(string[] textA, string[] textB) {
        Debug.Assert(textA.Length == textB.Length);
        int i;
        for (i = 0; i < textA.Length; i++) {
            if (!textA[i].Equals(textB[i])) {
                return false;
            }
        }
        return true;
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

    public void PrintText(string[] text) {
        Debug.Log("[" + string.Join(", ", text) + "]");
    }
}
