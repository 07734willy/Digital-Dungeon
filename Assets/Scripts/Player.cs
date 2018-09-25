using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    override protected void Awake() {
        base.Awake();
        pendingAction = new TurnAction {
            action = TurnAction.ActionType.None
        };
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // If both controls are held- they "cancel" just like up+down or left+right would.
        if (movement.x != 0 && movement.y != 0) {
            movement = Vector2.zero;
        }
        if (movement != Vector2.zero) {
            movement.Normalize();
            pendingAction.action = TurnAction.ActionType.Movement;
            pendingAction.movement = movement;
        }
    }

    public override TurnAction RequestAction() {
        TurnAction action;
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
        return action;
    }
}
