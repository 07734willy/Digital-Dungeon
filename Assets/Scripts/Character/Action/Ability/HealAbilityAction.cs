using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityAction : TurnAction {

    public HealAbilityAction(Character character) {
        this.character = character;
    }

    public override bool Check() {
       return true;
    }

    public override void Animate() {
        isComplete = true;
        character.SnapToGrid();
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        character.health += 10;
        if (character.health > character.maxHealth) {
            character.health = character.maxHealth;
        }

        this.startTime = Time.time;
        return true;
    }
}
