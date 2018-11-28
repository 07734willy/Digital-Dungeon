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
        character.RefreshInventory();
    }
	
	public override Pickup Clone () {
		//Debug.Log("HealthPotion cloning");
		
		HealthPotion health = Instantiate<GameObject>(this.gameObject).GetComponent<HealthPotion>();

		return health;
	}
	
	public override string GetStats () {
		//Debug.Log("Health Potion stats");
		string message = "Cost: " + this.cost + "  \tLevel Required: " + this.GetBaseLevel() + "\nSell: " + (int)(this.cost*0.8) + "  \tHeals: " + this.healValue;
		return message;
	}
}
