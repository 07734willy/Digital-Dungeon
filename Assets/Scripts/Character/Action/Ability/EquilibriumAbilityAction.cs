using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquilibriumAbilityAction : AbilityAction {

    private readonly Vector2[] attackShape = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.up + Vector2.right, Vector2.up + Vector2.left, Vector2.down + Vector2.right, Vector2.down + Vector2.left };

    private Character target;

    public EquilibriumAbilityAction(Character character) {
        this.gameManager = character.GetGameManager();
        this.character = character;
        this.target = GetTarget();
        this.abilityClass = Character.AbilityClass.Equilibrium;
        /*this.duration = instant ? 0f : 1f / attackSpeed;*/
    }

    public Character GetTarget() {
        List<Character> targets = new List<Character>();
        foreach (Vector2 rangeCoords in attackShape) {
            GameTile tile = gameManager.GetTile(character.GetCoordinates() + rangeCoords);
            if (tile == null) { continue; }
            Character target = tile.GetCharacter();
            if (target != null) {
                targets.Add(target);
            }
        }
        if (targets.Count < 1) {
            return null;
        }
        int index = Random.Range(0, targets.Count);
        return targets[index];
    }

    public override bool Check() {
        Debug.Assert(GetAbilityLevel() >= 0);
        return GetTarget() != null && GetAbilityLevel() > 0 && !character.IsOnCooldown(abilityClass);
    }

    public override void Animate() {
        isComplete = true;
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        int level = GetAbilityLevel();

        Character characterA = target.health >= character.health ? target : character;
        Character characterB = target.health >= character.health ? character : target;

        int healthVal = (int)((characterA.health - characterB.health) * level / 4);
        characterA.ReceiveDamage(healthVal);
        characterB.Heal(healthVal);

        character.AddActionFinisher(new AbilityFinisher(character, this.abilityClass, 2));
        character.SetOnCooldown(this.abilityClass, true);

        this.startTime = Time.time;
        return true;
    }
}
