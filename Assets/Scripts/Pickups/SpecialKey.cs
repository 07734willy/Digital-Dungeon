using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialKey : Consumable {

    public DoorTile door;
	public string tag = "Special_Key";

    public override void Select() {
        RefreshStatus();
        if (IsEquipped()) {
            door.OpenDoor();
            Destroy(this.gameObject);
        }
        character.RefreshInventory();
    }

    public override Pickup Clone() {
        //Debug.Log("HealthPotion cloning");

        SpecialKey specialKey = Instantiate<GameObject>(this.gameObject).GetComponent<SpecialKey>();

        return specialKey;
    }

    public override string GetStats() {
        //Debug.Log("Health Potion stats");
        Debug.LogError("This shouldn't be purchasable");
        string message = "Not able to be purchased";
        return message;
    }
}