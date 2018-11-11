using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[CanEditMultipleObjects]
public class FillArea : Editor {

    private bool jDown = false;
    private bool reset = true;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (!EditorApplication.isPlaying) {
            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            switch (e.GetTypeForControl(controlID)) {
                case EventType.MouseDown:
                    Debug.Log("Mouse down");
                    if (jDown && reset) {
                        Debug.Log("activating");
                        //GUIUtility.hotControl = controlID;
                        reset = false;
                        e.Use();

                        InstantiatePrefabs(Selection.activeTransform.position, e.mousePosition, Selection.activeGameObject);
                    }
                    break;
                case EventType.MouseUp:
                    //GUIUtility.hotControl = 0;
                    reset = true;
                    break;
                case EventType.MouseDrag:
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.J) {
                        Debug.Log("J down");
                        jDown = true;
                    }
                    break;
                case EventType.KeyUp:
                    if (e.keyCode == KeyCode.J) {
                        jDown = false;
                    }
                    break;
                default:
                    Debug.Log("Other");
                    break;
            }
        }
    }

    private void InstantiatePrefabs(Vector2 start, Vector2 stop, GameObject prefab) {
        int i, j;
        for (i = (int)start.x; i <= (int)stop.x; i += (((int)stop.x - (int)start.x) >= 0 ? 1 : -1)) {
            for (j = (int)start.y; j <= (int)stop.y; j += (((int)stop.y - (int)start.y) >= 0 ? 1 : -1)) {
                if (i == (int)start.x && j == (int)start.y) {
                    continue;
                }
                GameObject go = Instantiate<GameObject>(prefab);
                go.transform.position = new Vector2(i, j);
            }
        }
    }
    
}
