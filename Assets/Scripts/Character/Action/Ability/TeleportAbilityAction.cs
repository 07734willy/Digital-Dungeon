using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAbilityAction : AbilityAction {

    private readonly Vector2[] attackShape = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right, 2 * Vector2.up, 2 * Vector2.down, 2 * Vector2.left, 2 * Vector2.right, 3 * Vector2.up, 3 * Vector2.down, 3 * Vector2.left, 3 * Vector2.right };

    private Character target;

    public TeleportAbilityAction(Character character) {
        this.gameManager = character.GetGameManager();
        this.character = character;
        this.target = GetTarget();
        this.abilityClass = Character.AbilityClass.Teleport;
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
        character.SnapToGrid();
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        Vector2 temp = character.transform.position;
        character.transform.position = target.transform.position;
        target.transform.position = temp;

        character.AddActionFinisher(new AbilityFinisher(character, this.abilityClass, 2));
        character.SetOnCooldown(this.abilityClass, true);

        this.startTime = Time.time;
        return true;
    }
}
