using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityAction : AbilityAction {

    public HealAbilityAction(Character character) {
        this.character = character;
        this.abilityClass = Character.AbilityClass.Heal;
    }

    public override bool Check() {
        Debug.Assert(GetAbilityLevel() >= 0);
        return GetAbilityLevel() > 0 && !character.IsOnCooldown(abilityClass);
    }

    public override void Animate() {
        isComplete = true;
        character.SnapToGrid();
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        int level = GetAbilityLevel();

        character.Heal((int)(10 * Mathf.Pow(1.5f, level - 1)));

        character.AddActionFinisher(new AbilityFinisher(character, this.abilityClass, 2));
        character.SetOnCooldown(this.abilityClass, true);

        this.startTime = Time.time;
        return true;
    }
}
