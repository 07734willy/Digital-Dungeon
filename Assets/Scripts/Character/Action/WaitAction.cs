using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : TurnAction {

    public WaitAction(Character character) {
        this.character = character;
        this.isComplete = true;
    }

    public override bool Check() {
        return true;
    }

    public override void Animate() {
        this.isComplete = true;
        return;
    }

    public override bool Execute() {
        return true;
    }
}
