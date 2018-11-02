using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeNow : Pickup {

	// Use this for initialization

	public enum Type{
		Gold,
		Key,
		Arrows
	};

	public Type type;

	public int numGold = 0;

	public void Awake () {
		this.isConsumeNow = true;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public void Consume(){
		this.character = gameManager.GetPlayer();
		switch (this.type)
		{
			case Type.Gold:
			this.character.gold += numGold;
			break;
			case Type.Key:
			this.character.keys++;
			break;
			case Type.Arrows:
			this.character.arrows += 50;
			break;
			default:
			break;
		}
		Destroy(this.gameObject);
	}
	
	public override Pickup Clone () {
		Debug.Log("ConsumeNow cloning");
		
		ConsumeNow consumeNow = Instantiate<GameObject>(this.gameObject).GetComponent<ConsumeNow>();

		return consumeNow;
	}
}
