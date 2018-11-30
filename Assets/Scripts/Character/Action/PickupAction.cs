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
			Debug.Log("Failing check!");
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
        if (pickup is Weapon){
            ((Player)character).completeAchievement("First Weapon");
        }


		if (pickup.IsPurchasable()){
			if(character.GetGold() < pickup.GetCost() || character.GetLevel() < pickup.GetBaseLevel()) {
                string notEnough = string.Format("Innsufficent gold! This item costs: {0}", pickup.GetCost());
                gameManager.GetPlayer().SetDialogMessage(notEnough);
				return false;
			}
			else {
                ((Player)character).completeAchievement("First Purchase");
				character.SetGold(character.GetGold() - pickup.GetCost());
				Pickup newPickup = pickup.Clone();
				gameManager.GetTile(pickup.GetCoordinates()).AddPickup(newPickup);
				pickup.isPurchasable = false;
			}
		}
			
        gameManager.GetTile(pickup.GetCoordinates()).RemovePickup(pickup);
        //pickup.transform.parent = character.transform
		
		//Find index of first dummy item in inventory
		Pickup[] inventory = GameObject.Find("InventoryInven").GetComponentsInChildren<Pickup>();
		int index = 0;
		for(int i = 0; i < inventory.Length; i++){
			if(inventory[i].name == "invenDummy"){
				index = i; 
				break;
			}
		}
		//REplace dummy item with pickup
		pickup.transform.parent = GameObject.Find("InventoryInven").transform;
		pickup.transform.SetSiblingIndex(index);
		UnityEngine.Object.Destroy(GameObject.Find("InventoryInven").transform.GetChild(index+1).gameObject);

        //pickup.transform.parent = character.transform;
		//pickup.transform.parent = GameObject.Find("InventoryInven").transform;

        pickup.GetComponent<SpriteRenderer>().enabled = false;
        Dialog dialog = pickup.GetComponent<Dialog>();
        if (dialog != null) {
            gameManager.GetPlayer().SetDialogMessage(dialog.message);
        }
	    pickup.SetCharacter(this.character);
        if (pickup is ConsumeNow) {
            ((ConsumeNow)pickup).Consume();
        } else if (pickup is Consumable) {
            pickup.Select();
        }
        //pickup.transform.position = Vector2.zero;
        // might need to set transform.position to Vector2.zero <- I'm not sure
        this.startTime = Time.time;
        return true;
    }
}
