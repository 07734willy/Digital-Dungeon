using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : TurnAction {

    public Vector2 target;
    public int damage;
    public float accuracy;
    public bool intant;

    public AttackAction(Character character, Vector2 target, int damage, float accuracy, float attackSpeed, bool instant) {
        this.character = character;
        this.target = target;
        this.damage = damage;
        this.accuracy = accuracy;
        this.duration = instant ? 0f : 1f / attackSpeed;
        this.gameManager = character.GetGameManager();
    }

    public override bool Check() {
        // TODO
        return true;
    }

    public override void Animate() {
        isComplete = true;
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }
        this.startTime = Time.time;
        return true;
    }
}
