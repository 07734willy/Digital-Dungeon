 using UnityEngine;
 using System.Collections;
 
 public class Inventory : MonoBehaviour {

    void Start() {
        ToggleVisibility();
    }

    void Update() {
         if (Input.GetKeyUp(KeyCode.I)) {
            ToggleVisibility();
         }
     }

    public void ToggleVisibility() {
        foreach (Transform child in this.transform) {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
 }
