using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAction : TurnAction {

    public Pickup pickup;

    public PickupAction(Character character, Pickup pickup) {
        this.character = character;
        this.pickup = pickup;
        this.gameManager = character.GetGameManager();
    }

    public override void Animate() {
        isComplete = true;
    }

    public override bool Execute() {
        //Item item = pickup.GetItem();
        character.RefreshInventory();
        if (character.SpareInventoryCapacity() <= 0) {
            return false;
        }
        gameManager.GetTile(pickup.GetCoodinates()).RemovePickup(pickup);
        pickup.transform.parent = character.transform;
        pickup.GetComponent<SpriteRenderer>().enabled = false;
        //pickup.transform.position = Vector2.zero;
        // might need to set transform.position to Vector2.zero <- I'm not sure
        this.startTime = Time.time;
        return true;
    }
}
