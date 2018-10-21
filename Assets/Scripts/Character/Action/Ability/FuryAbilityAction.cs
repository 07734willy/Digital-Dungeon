using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryAbilityAction : AbilityAction {

    private readonly Vector2[] attackShape = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.up + Vector2.right, Vector2.up + Vector2.left, Vector2.down + Vector2.right, Vector2.down + Vector2.left };

    private List<Character> targets;

    public FuryAbilityAction(Character character) {
        this.gameManager = character.GetGameManager();
        this.character = character;
        this.targets = GetTargets();
        this.abilityClass = Character.AbilityClass.Fury;
        /*this.duration = instant ? 0f : 1f / attackSpeed;*/
    }

    public List<Character> GetTargets() {
        List<Character> targets = new List<Character>();
        foreach (Vector2 rangeCoords in attackShape) {
            GameTile tile = gameManager.GetTile(character.GetCoordinates() + rangeCoords);
            if (tile == null) { continue; }
            Character target = tile.GetCharacter();
            if (target != null) {
                targets.Add(target);
            }
        }
        return targets;
    }

    public override bool Check() {
        Debug.Assert(GetAbilityLevel() >= 0);
        return GetAbilityLevel() > 0;
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

        int randVal = Random.Range(0, 8);

        if (randVal < targets.Count) {
            targets[randVal].ReceiveDamage((int)(145 * Mathf.Pow(1.5f, level - 1)));
        }

        this.startTime = Time.time;
        return true;
    }
}
