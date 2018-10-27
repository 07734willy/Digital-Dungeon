using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFinisher : ActionFinisher {

    protected Character character;
    protected Character.AbilityClass abilityClass;

    public AbilityFinisher(Character character, Character.AbilityClass abilityClass, int turns) {
        this.character = character;
        this.abilityClass = abilityClass;
        this.finishTurn = character.GetTurnNumber() + turns;
    }

    public override bool Execute() {
        this.character.SetOnCooldown(this.abilityClass, false);
        return true;
    }
}

