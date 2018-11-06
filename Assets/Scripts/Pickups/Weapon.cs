using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Pickup {
    public int damage = 100;
    public float accuracy = 1f;
	public bool isRanged;
	public int range;

    public int GetDamageDealt() {
        return Random.Range(0, 1000) < 1000 * accuracy ? this.damage : 0;
    }

    public override void Select() {
        RefreshStatus();
        if (IsEquipped() && this.character != null && !this.isRanged) {
            Debug.Assert(character.GetMeleeWeapon() == this);
            character.SetMeleeWeapon(this);
        }
		else if(IsEquipped() && this.character != null && this.isRanged) {
            Debug.Assert(character.GetRangedWeapon() == this);
            character.SetRangedWeapon(this);
		}
    }

    public override void Update() {
        base.Update();

        if (IsEquipped()) {
            Select();
        }
    }

    public bool IsEquipped() {
        return transform.parent != null && transform.parent.name != "InventoryInven";
    }
	
	public override Pickup Clone () {
		Debug.Log("Weapon cloning");
		
		Weapon weapon = Instantiate<GameObject>(this.gameObject).GetComponent<Weapon>();
/* 		weapon.stackable = this.stackable;
		weapon.quantity = this.quantity;
		weapon.value = this.value;
		weapon.isConsumeNow = this.isConsumeNow;
		weapon.isPurchasable = this.isPurchasable;
		weapon.cost = this.cost;
		weapon.itemSprite = this.itemSprite;
		weapon.gameManager = this.gameManager;
		weapon.character = this.character;
		weapon.damage = this.damage;
		weapon.accuracy = this.accuracy;
		weapon.isRanged = this.isRanged;
		weapon.range = this.range;
 */		
		return weapon;
	}
	
	public override string GetStats () {
		Debug.Log("Weapon stats");
		string message = "Cost: " + this.cost + "  \tLevel Required: " + this.GetBaseLevel() + "\nDamage: " + this.damage + "\nRange: " + this.range;
		return message;
	}
}