using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : Consumable {

    public Character.AbilityClass abilityClass = Character.AbilityClass.Spin;

    public override void Select() {
        RefreshStatus();
        if (IsEquipped()) {
            character.SetAbilityLevel(abilityClass, character.getAbilityLevel(abilityClass) + 1);
            Destroy(this.gameObject);
        }
        character.RefreshInventory();
    }
}