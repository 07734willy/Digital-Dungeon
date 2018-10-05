using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AttackAction : TurnAction {

    public Character target;
    public bool intant;

    public AttackAction(Character character, Character target, float attackSpeed, bool instant) {
        this.character = character;
        this.target = target;
        this.duration = instant ? 0f : 1f / attackSpeed;
        this.gameManager = character.GetGameManager();
    }
}
