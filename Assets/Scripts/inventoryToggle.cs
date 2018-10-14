 using UnityEngine;
 using System.Collections;
 
 public class inventoryToggle : MonoBehaviour {
     private Canvas CanvasObject; // Assign in inspector
	
     void Start()
     {
         CanvasObject = GetComponent<Canvas> ();
		 CanvasObject.enabled = !CanvasObject.enabled;
     }
 
     void Update() 
     {
         if (Input.GetKeyUp(KeyCode.I)) 
         {
             CanvasObject.enabled = !CanvasObject.enabled;
         }
     }
 }
