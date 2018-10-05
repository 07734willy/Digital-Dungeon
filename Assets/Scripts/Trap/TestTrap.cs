using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : GameTile {
	private bool sprung;
    public string trapType;
    override protected void Awake() {
        base.Awake();
        this.sprung = false;
    }
    public override void SetCharacter(Character character) {	
        if(this.sprung == false && this.character != character){
            base.SetCharacter(character);
            switch (trapType)
            {
                case "Damage":
                character.ReceiveDamage(100);
                this.sprung = true;
                break; 
                case "Resetting":
                character.ReceiveDamage(40);
                break;
                case "Tutorial":
                this.GetComponent<Dialog>().DisplayDialogMessage();
                break;
                //case "Move":
                //Vector2 movement = Vector2.left; 
                //new MovementAction(character, character.GetCoordinates() + movement, character.movementSpeed ,false);
                //this.sprung = true;
                //break;
                default:
                Debug.Log("Invalid Trap Type");
                break;
            }
        }
    }
}