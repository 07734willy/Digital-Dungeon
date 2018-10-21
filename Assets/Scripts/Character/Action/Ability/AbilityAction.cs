using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityAction : TurnAction {

    protected Character.AbilityClass abilityClass = Character.AbilityClass.None;


    protected int GetAbilityLevel() {
        Debug.Assert(abilityClass != Character.AbilityClass.None);
        return this.character.getAbilityLevel(abilityClass);
    }
}
