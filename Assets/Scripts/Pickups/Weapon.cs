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
}