using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physical : MonoBehaviour {

    public void SnapToGrid() {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    public Vector2 GetCoodinates() {
        return transform.position;
    }
}
