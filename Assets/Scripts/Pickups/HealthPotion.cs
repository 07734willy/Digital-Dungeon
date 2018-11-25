using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable {

    public int healValue = 30;

    public override void Select() {
        RefreshStatus();
        if (IsEquipped()) {
            character.Heal(healValue);
            Destroy(this.gameObject);
        }
		this.gameManager.GetPlayer().shiftLogBox();
			this.gameManager.GetPlayer().logs[0]="Restored " + healValue + " health";
        character.RefreshInventory();
    }
	
	public override Pickup Clone () {
		Debug.Log("HealthPotion cloning");
		
		HealthPotion health = Instantiate<GameObject>(this.gameObject).GetComponent<HealthPotion>();

		return health;
	}
	
	public override string GetStats () {
		Debug.Log("Health Potion stats");
		string message = "Cost: " + this.cost + "  \tLevel Required: " + this.GetBaseLevel() + "\nHeals: " + this.healValue;
		return message;
	}
}
