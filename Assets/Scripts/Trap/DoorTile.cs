using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : GameTile {

	private GameManager curGm;
	private Player curPlayer;
	public Sprite openSprite;
	private bool isOpen;
	public bool isLockedDoor = false;

	override protected void Awake() {
		base.Awake();
		this.isWalkable = false;
		this.isOpen = false;
		curGm = FindObjectOfType<GameManager>();	
	}

	private void OnMouseDown() {
		this.curPlayer = curGm.GetPlayer();
		if((this.GetCoordinates() - curPlayer.GetCoordinates()).magnitude == 1 && this.isOpen == false){
			if(this.isLockedDoor){
				Debug.Log("Locked!");
				this.OpenDoor();
			}else{
				Debug.Log("Not Locked!");
				this.OpenDoor();
			}
		}

	}

	private void OpenDoor(){
		this.isWalkable = true;
		SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
		renderer.sprite = openSprite;
		this.isOpen = true;
		Debug.Log("Hello");
	}
}