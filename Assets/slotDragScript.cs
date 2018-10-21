using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class slotDragScript : MonoBehaviour, IDropHandler {

	public void OnDrop (PointerEventData eventData){
		//temporarily set item a new parent
		DragHandler.itemBeingDragged.transform.SetParent(transform);
		
		//Get indexes to switch in the inventory array
		int startIndex = int.Parse(Regex.Match(DragHandler.startParent.name, @"\(([^)]*)\)").Groups[1].Value);
		//use new parent to get the index
		int endIndex = int.Parse(Regex.Match(DragHandler.itemBeingDragged.transform.parent.name, @"\(([^)]*)\)").Groups[1].Value);
		Transform b = GameObject.Find("Player").transform.GetChild(startIndex + 3);
		Transform c = GameObject.Find("Player").transform.GetChild(endIndex + 3);
			
		b.SetSiblingIndex(endIndex+3);

		//Restore original parent and starting position
		DragHandler.itemBeingDragged.transform.position = DragHandler.startPosition;
		DragHandler.itemBeingDragged.transform.SetParent(DragHandler.startParent);
	}
}
