using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class slotDragScript : MonoBehaviour, IDropHandler {

	public void OnDrop (PointerEventData eventData){
		//temporarily set item a new parent
		GameObject item = DragHandler.itemBeingDragged;
		item.transform.SetParent(transform);
		
		if(item.transform.parent.parent.parent.name == "Inventory" && DragHandler.startParent.parent.parent.name == "Inventory"){
			//Get indexes to switch in the inventory array
			int startIndex = int.Parse(Regex.Match(DragHandler.startParent.name, @"\(([^)]*)\)").Groups[1].Value);
			//use new parent to get the index
			int endIndex = int.Parse(Regex.Match(DragHandler.itemBeingDragged.transform.parent.name, @"\(([^)]*)\)").Groups[1].Value);
			Transform b = GameObject.Find("InventoryInven").transform.GetChild(startIndex);
			Transform c = GameObject.Find("InventoryInven").transform.GetChild(endIndex);
				
			b.SetSiblingIndex(endIndex);
			c.SetSiblingIndex(startIndex);
			
		}
		else if(DragHandler.startParent.parent.parent.name == "Inventory" && item.transform.parent.parent.parent.name == "Equipped"){
			int startIndex = int.Parse(Regex.Match(DragHandler.startParent.name, @"\(([^)]*)\)").Groups[1].Value);
			Transform b = GameObject.Find("InventoryInven").transform.GetChild(startIndex);
			b.transform.SetParent(GameObject.Find("EquippedInven").transform);
			if(b.gameObject.GetComponent<Pickup>().isWeapon){
Debug.Log("SUP2");
				GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer().SetWeapon(b.gameObject.GetComponent<Weapon>());
			}
			
		}
		else if(DragHandler.startParent.parent.parent.name == "Equipped" && item.transform.parent.parent.parent.name == "Inventory"){
			int startIndex = int.Parse(Regex.Match(DragHandler.startParent.name, @"\(([^)]*)\)").Groups[1].Value);
			Transform b = GameObject.Find("EquippedInven").transform.GetChild(startIndex);
			b.transform.SetParent(GameObject.Find("InventoryInven").transform);
			if(b.gameObject.GetComponent<Pickup>().isWeapon){
				GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer().SetWeapon(null);
			}
			
		}
		//Restore original parent and starting position
			DragHandler.itemBeingDragged.transform.position = DragHandler.startPosition;
			DragHandler.itemBeingDragged.transform.SetParent(DragHandler.startParent);
	}
}
