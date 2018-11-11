using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bars : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Player player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
		double playerHealth = (double)player.health;
		double playerMaxHealth = (double)player.maxHealth;
		double percent = playerHealth / playerMaxHealth * 184.5;
		RectTransform rt = this.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2((int)percent, (int)180.24);
	}
}
