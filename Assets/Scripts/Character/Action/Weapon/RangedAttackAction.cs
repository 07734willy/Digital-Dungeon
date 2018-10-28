﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackAction : AttackAction {
public RangedAttackAction (Character character, Character target, float attackSpeed, bool instant) 
        : base(character, target, attackSpeed, instant) {

    }

    public override bool Check() {
        if ((target.GetCoordinates() - this.character.GetCoordinates()).magnitude <= character.rangedWeapon.range) {
            // other checks in here
            return true;
        }
        return false;
    }

    public override void Animate() {
        isComplete = true;
        character.SnapToGrid();
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }
		Debug.Log("Ranged attack bitches!");
        Weapon weapon = character.GetWeapon();
        if (weapon == null) {
            target.ReceiveDamage(30);
        } else {
            target.ReceiveDamage(weapon.GetDamageDealt());
        }

        this.startTime = Time.time;
        return true;
    }
}
