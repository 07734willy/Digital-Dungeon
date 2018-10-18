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

    public override bool Check() {
        character.RefreshInventory();
        if (character.SpareInventoryCapacity() <= 0) {
            return false;
        }
        return true;
    }

    public override void Animate() {
        isComplete = true;
    }

    public override bool Execute() {
        //Item item = pickup.GetItem();
        if (!Check()) {
            return false;
        }
		
        gameManager.GetTile(pickup.GetCoordinates()).RemovePickup(pickup);
        pickup.transform.parent = character.transform;
        pickup.GetComponent<SpriteRenderer>().enabled = false;
        Dialog dialog = pickup.GetComponent<Dialog>();
        if (dialog != null) {
            gameManager.GetPlayer().SetDialogMessage(dialog.message);
        }
        //pickup.transform.position = Vector2.zero;
        // might need to set transform.position to Vector2.zero <- I'm not sure
        this.startTime = Time.time;
        return true;
    }
}
