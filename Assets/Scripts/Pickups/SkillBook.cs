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
	
	public override Pickup Clone () {
		//Debug.Log("SkillBook cloning");
		
		SkillBook skill = Instantiate<GameObject>(this.gameObject).GetComponent<SkillBook>();

		return skill;
	}
	
	public override string GetStats () {
		//Debug.Log("SkillBook stats");
		string message = "Cost: " + this.cost + "  \tLevel Required: " + this.GetBaseLevel() +  "\nSell: " + (int)(this.cost*0.8) + "  \tAbility: " + this.abilityClass;
		return message;
	}
}