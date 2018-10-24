using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : GameTile {

	private GameManager = curGm;
	override protected void Awake() {
		base.Awake();
		this.isWalkable = false;
		curGm = FindObjectOfType<GameManager>();	
	}

}