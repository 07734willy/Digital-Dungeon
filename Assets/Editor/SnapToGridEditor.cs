using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[CustomEditor(typeof(SnapToGrid), true)]
[CanEditMultipleObjects]
public class SnapToGridEditor : Editor {

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

    // The snapping code
    private Vector3 RoundTransform(Vector3 v, float snapValue) {
        return new Vector3(snapValue * Mathf.Round(v.x / snapValue), snapValue * Mathf.Round(v.y / snapValue), v.z);
    }
}
