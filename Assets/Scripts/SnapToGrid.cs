using UnityEngine;
using System.Collections;
 
public class SnapToGrid : MonoBehaviour {
#if UNITY_EDITOR
    public bool snapToGrid = true;
    public float snapValue = 1f;
#endif
}