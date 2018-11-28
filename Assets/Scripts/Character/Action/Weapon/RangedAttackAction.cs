using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackAction : AttackAction {
public RangedAttackAction (Character character, Character target, float attackSpeed, bool instant) 
        : base(character, target, attackSpeed, instant) {

    }

    public override bool Check() {
        if (character.GetRangedWeapon() == null) {
            return false;
        }
        if ((target.GetCoordinates() - this.character.GetCoordinates()).magnitude <= character.GetRangedWeapon().range) {
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
		Debug.Log("Ranged execute function check successful");
        Weapon weapon = character.GetRangedWeapon();
        if (weapon == null) {
            Debug.LogError("This shouldn't execute-  if the player has no ranged weapon, don't attack");
        } else {
            target.ReceiveDamage(weapon.GetDamageDealt());
        }

        this.startTime = Time.time;
        return true;
    }
}
