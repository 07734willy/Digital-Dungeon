using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : AttackAction {

    public MeleeAttackAction (Character character, Character target, int damage, float accuracy, float attackSpeed, bool instant) 
        : base(character, target, damage, accuracy, attackSpeed, instant) {

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
}
