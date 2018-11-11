using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expBarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Player player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
		double playerExperience = (double)player.totalExperience;
		double playerMilestone = (double)player.expMilestone;
		double percent = playerExperience / playerMilestone * 184.5;
		RectTransform rt = this.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2((int)percent, (int)180.24);
	}
}
