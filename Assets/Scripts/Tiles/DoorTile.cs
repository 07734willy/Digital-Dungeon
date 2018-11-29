using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : GameTile {

	private GameManager curGm;
	private Player curPlayer;
	public Sprite openSprite;
    private Sprite closedSprite;
	public bool isOpen = false;
	public bool isLockedDoor = false;
    public bool hasKey = true;
    public int lightLocks = 0;

    override protected void Awake() {
		base.Awake();
		this.isWalkable = false;
        this.closedSprite = this.GetComponent<SpriteRenderer>().sprite;
        if (isOpen) {
            OpenDoor();
        }
        curGm = FindObjectOfType<GameManager>();	
	}

	private void OnMouseDown() {
		this.curPlayer = curGm.GetPlayer();
		if((this.GetCoordinates() - curPlayer.GetCoordinates()).magnitude == 1 && this.isOpen == false && this.hasKey && this.lightLocks == 0){
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

	public void OpenDoor(){
		this.isWalkable = true;
		SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
		renderer.sprite = openSprite;
		this.isOpen = true;
	}

    public void CloseDoor() {
        this.isWalkable = false;
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = closedSprite;
        this.isOpen = false;
    }

    public bool IsOpen() {
        return this.isOpen;
    }
}