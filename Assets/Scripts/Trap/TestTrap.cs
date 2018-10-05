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
        	Debug.Log("Trap time~");
            switch (trapType)
            {
                case "Damage":
                Debug.Log("Damage Trap");
                character.ReceiveDamage(200);
                this.sprung = true;
                break; 
                case "Resetting":
                Debug.Log("Resetting Trap");
                character.ReceiveDamage(100);
                break;
                case "Move":
                Debug.Log("Move Trap");
                Vector2 movement = Vector2.left; 
                new MovementAction(character, character.GetCoordinates() + movement, character.movementSpeed ,false);
                this.sprung = true;
                break;
                default:
                Debug.Log("Invalid Trap Type");
                break;
            }
        }
    }
}