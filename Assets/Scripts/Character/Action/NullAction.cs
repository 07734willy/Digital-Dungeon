using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullAction : TurnAction {

    public NullAction(Character character) {
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
        return false;
    }
}
