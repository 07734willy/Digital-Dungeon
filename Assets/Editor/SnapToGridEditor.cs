using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[CustomEditor(typeof(SnapToGrid), true)]
[CanEditMultipleObjects]
public class SnapToGridEditor : Editor {

    private bool jDown = false;
    private bool reset = true;
    private UnityEngine.KeyCode boxFillKeyCode = KeyCode.Space;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        SnapToGrid actor = target as SnapToGrid;
        if (actor.snapToGrid && !EditorApplication.isPlaying) {
            foreach (Transform transform in Selection.transforms) {
                SnapToGrid snap = transform.gameObject.GetComponent<SnapToGrid>();
                if (snap != null && snap.snapToGrid) {
                    transform.position = RoundTransform(transform.position, snap.snapValue);
                }
            }
            actor.transform.position = RoundTransform(actor.transform.position, actor.snapValue);
        }
    }

    public void OnSceneGUI() {
        if (!EditorApplication.isPlaying) {
            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            switch (e.GetTypeForControl(controlID)) {
                case EventType.MouseDown:
                    if (jDown && reset && Camera.current != null) {
                        //GUIUtility.hotControl = controlID;
                        reset = false;
                        e.Use();
                        
                        InstantiatePrefabs(Selection.activeTransform.position, RoundTransform(HandleUtility.GUIPointToWorldRay(e.mousePosition).GetPoint(0), 1), Selection.activeGameObject);
                    }
                    break;
                case EventType.MouseUp:
                    //GUIUtility.hotControl = 0;
                    reset = true;
                    break;
                case EventType.MouseDrag:
                    break;
                case EventType.KeyDown:
                    if (e.keyCode == this.boxFillKeyCode) {
                        jDown = true;
                    }
                    break;
                case EventType.KeyUp:
                    if (e.keyCode == this.boxFillKeyCode) {
                        jDown = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    // The snapping code
    private Vector3 RoundTransform(Vector3 v, float snapValue) {
        return new Vector3(snapValue * Mathf.Round(v.x / snapValue), snapValue * Mathf.Round(v.y / snapValue), v.z);
    }

    private void InstantiatePrefabs(Vector2 start, Vector2 stop, GameObject prefab) {
        int i, j;
        for (i = 0; i <= (int)Mathf.Abs((int)stop.x - (int)start.x); i++) {
            for (j = 0; j <= (int)Mathf.Abs((int)stop.y - (int)start.y); j++) {
                int xDir = (int)stop.x >= (int)start.x ? 1 : -1;
                int yDir = (int)stop.y >= (int)start.y ? 1 : -1;
                int xPos = (int)start.x + i * xDir;
                int yPos = (int)start.y + j * yDir;
                if (Physics2D.OverlapBox(new Vector2(xPos, yPos), new Vector2(0.1f, 0.1f), 0)) {
                    continue;
                }
                GameObject go = Instantiate<GameObject>(prefab);
                GameObject actualPrefab;
                if ((actualPrefab = (GameObject)PrefabUtility.GetCorrespondingObjectFromSource(prefab)) != null) {
                    go = PrefabUtility.ConnectGameObjectToPrefab(go, actualPrefab);
                }
                go.transform.position = new Vector2(xPos, yPos);
                go.name = prefab.name;
            }
        }
    }
}
