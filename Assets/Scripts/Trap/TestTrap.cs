using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : GameTile {
	private bool sprung;
    override protected void Awake() {
        base.Awake();
        this.sprung = false;
    }
    public override void SetCharacter(Character character) {	
        base.SetCharacter(character);
        if(character.IsPlayer() && this.sprung == false){
        	Debug.Log("Trap time~");
        	this.sprung = true;
        }


    }
}