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
}
