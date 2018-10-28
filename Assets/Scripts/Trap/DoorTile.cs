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
				//need inventory, temp "has key to test dialog"
				if(curPlayer.keys > 0){
					curPlayer.SetDialogMessage("Key used! Door unlocked.");
					curPlayer.keys--;
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