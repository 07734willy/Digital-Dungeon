using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : GameTile {

	private GameManager curGm;
	private Player curPlayer;
	public Sprite openSprite;
	private bool isOpen;
	public bool isLockedDoor = false;
	//temp for testing dialog DELETE THIS 
	public bool playerHasKey = false;
	//DELETE THIS DELETE THIS

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
				//need inventory, temp "has key to test dialog"
				if(this.playerHasKey){
					curPlayer.SetDialogMessage("Key used! Door unlocked.");
					this.OpenDoor();
				}else{
					curPlayer.SetDialogMessage("Door is locked! Find a Key.");
				}
			}else{
				curPlayer.SetDialogMessage("Door opened!");
				this.OpenDoor();
			}
		}

	}

	private void OpenDoor(){
		this.isWalkable = true;
		SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
		renderer.sprite = openSprite;
		this.isOpen = true;
	}

	public bool IsOpen(){
		return this.isOpen;
	}

}