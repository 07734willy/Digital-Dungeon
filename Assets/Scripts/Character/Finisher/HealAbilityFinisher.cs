using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityFinisher : ActionFinisher {

    protected Character character;
    protected GameManager gameManager;

    public HealAbilityFinisher (Character character, int turns) {
        this.character = character;
        this.finishTurn = character.GetTurnNumber() + turns;
    }

	public override bool Execute() {
        this.character.SetOnCooldown(Character.AbilityClass.Heal, false);
        return true;
    }
}
