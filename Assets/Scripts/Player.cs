using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    override protected void Awake() {
        base.Awake();
        this.isPlayer = true;
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();

        if (currentAction.isComplete) {
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // If both controls are held- they "cancel" just like up+down or left+right would.
            if (movement.x != 0 && movement.y != 0) {
                movement = Vector2.zero;
            }
            if (movement != Vector2.zero) {
                movement.Normalize();
                pendingAction = new MovementAction(this, movement);
            }
        }
    }

    public override TurnAction RequestAction() {
        /*TurnAction action;
        if (lastAction.animating) {
            action = new TurnAction {
                action = TurnAction.ActionType.Animation
            };
            return action;
        }
        // We want to reset the pending action, so it doesn't loop
        pendingAction.coordinates = GetCoodinates();
        action = pendingAction;
        pendingAction = new TurnAction();
        pendingAction.action = TurnAction.ActionType.None;
        return action;*/
        Debug.Assert(currentAction.isComplete);
        currentAction = pendingAction;
        pendingAction = new NullAction(this);
        return currentAction;
    }
}
