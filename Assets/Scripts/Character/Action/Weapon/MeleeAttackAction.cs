using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : AttackAction {

    public MeleeAttackAction (Character character, Character target, float attackSpeed, bool instant) 
        : base(character, target, attackSpeed, instant) {
    }

    public override bool Check() {
        if ((target.GetCoordinates() - this.character.GetCoordinates()).magnitude == 1) {
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

        Weapon weapon = character.GetMeleeWeapon();
		Debug.Log(weapon);
        if (weapon == null) {
            target.ReceiveDamage(30);
        } else {
            target.ReceiveDamage(weapon.GetDamageDealt());
        }

        this.startTime = Time.time;
        return true;
    }
}
