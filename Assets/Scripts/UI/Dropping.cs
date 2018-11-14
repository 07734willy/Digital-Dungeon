using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class Dropping : MonoBehaviour, IPointerDownHandler
{
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
 
    public void OnPointerDown(PointerEventData data)
    {
		GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		Character character = gameManager.GetPlayer();
		GameTile gameTile = gameManager.GetTile(character.GetCoordinates());
		
		
        clicked++;
        if (clicked == 1) clicktime = Time.time;
 
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
			Debug.Log(this.name);
            int startIndex = int.Parse(Regex.Match(this.name, @"\(([^)]*)\)").Groups[1].Value);
			
			bool shop = false;
			GameObject gameObject = GameObject.Find("InventoryInven").transform.GetChild(startIndex).gameObject;
			Debug.Log(gameObject);
			foreach (Pickup pickup in gameTile.GetPickups()) {
				if(pickup.IsPurchasable()){
					shop = true;
					if(gameObject.GetComponent<Pickup>().GetStats() == pickup.GetStats()){
						character.SetGold(character.GetGold() + (int)(pickup.GetCost()*0.8));
						Destroy(gameObject);
						//Debug.Log("Destroy");
					}
				}
			}
			if(!shop){
				Transform b = GameObject.Find("InventoryInven").transform.GetChild(startIndex);
				b.transform.parent = null;
				b.transform.GetComponent<SpriteRenderer>().enabled = true;
				b.position = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer().GetCoordinates();
				//Debug.Log("Dropped");
			}
			
 
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
 
    }
 
 
 
}