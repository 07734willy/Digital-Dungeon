using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AttackAction : TurnAction {

    public Character target;
    public int damage;
    public float accuracy;
    public bool intant;

    public AttackAction(Character character, Character target, int damage, float accuracy, float attackSpeed, bool instant) {
        this.character = character;
        this.target = target;
        this.damage = damage;
        this.accuracy = accuracy;
        this.duration = instant ? 0f : 1f / attackSpeed;
        this.gameManager = character.GetGameManager();
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        if (Random.Range(0, 1000) < 1000 * accuracy) {
            target.ReceiveDamage(this.damage);
        }
        this.startTime = Time.time;
        return true;
    }
}
