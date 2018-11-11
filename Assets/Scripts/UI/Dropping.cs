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
	public void addDummyItem(int index){
		GameObject player;
        player = new GameObject();
		player.name = "invenDummy";
		player.AddComponent<Pickup>();
		player.AddComponent<SpriteRenderer>();
		player.transform.parent = GameObject.Find("InventoryInven").transform;
		player.transform.SetSiblingIndex(index);
	}
    public void OnPointerDown(PointerEventData data)
    {
        clicked++;
        if (clicked == 1) clicktime = Time.time;
 
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
			Debug.Log(this.name);
            int startIndex = int.Parse(Regex.Match(this.name, @"\(([^)]*)\)").Groups[1].Value);
			Transform b = GameObject.Find("InventoryInven").transform.GetChild(startIndex);
			b.transform.parent = null;
			addDummyItem(startIndex);
			b.transform.GetComponent<SpriteRenderer>().enabled = true;
			b.position = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer().GetCoordinates();
			Debug.Log("Dropped");
 
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
 
    }
 
 
 
}