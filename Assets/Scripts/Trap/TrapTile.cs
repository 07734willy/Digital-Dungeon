﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {

    public int damage = 0;
    public bool resetting = true;
	private bool sprung;
    public string trapType;
    public string newLevel;
    private GameManager curGm;

    override protected void Awake() {
        base.Awake();
        this.sprung = false;
        curGm = FindObjectOfType<GameManager>();
    }

    public override void SetCharacter(Character character) {

        if (!this.sprung && this.character == null) {
            base.SetCharacter(character);

            Dialog dialog = this.GetComponent<Dialog>();
            if (dialog != null) {
                dialog.DisplayDialogMessage();
            }

            if (damage > 0) {
                character.ReceiveDamage(damage);
            }

            if (!resetting) {
                this.sprung = true;
            }

            if (newLevel != null){
            	curGm.loadNewLevel(newLevel);
                Debug.Log("weee");
            }
        }
    }
}