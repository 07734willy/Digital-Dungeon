using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullAction : TurnAction {

    public NullAction(Character character) {
        this.character = character;
        this.isComplete = true;
    }

    public override void Animate() {
        /*Debug.LogError("Tried to animate NullAction");
        throw new System.NotImplementedException();*/
        this.isComplete = true;
        return;
    }

    public override bool Execute() {
        /*Debug.LogError("Tried to execute NullAction");
        throw new System.NotImplementedException();*/
        return true;
    }
}
