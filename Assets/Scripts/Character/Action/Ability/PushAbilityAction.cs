using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAbilityAction : AbilityAction {

    private readonly Vector2[] attackShape = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Character target;
    private MovementAction action;

    public PushAbilityAction(Character character) {
        this.gameManager = character.GetGameManager();
        this.character = character;
        this.target = GetTarget();
        this.abilityClass = Character.AbilityClass.Push;
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
        this.isComplete = true;
        if (this.action != null) {
            this.action.Animate();
            this.isComplete = this.action.isComplete;
        }

    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }

        int level = GetAbilityLevel();

        Vector2 direction = target.GetCoordinates() - character.GetCoordinates();
        Vector2 destination = target.GetCoordinates();
        int i;
        for (i = 0; i < level + 1 && this.gameManager.GetTile(destination + direction).IsWalkable(); i++) {
            destination += direction;
        }

        if (destination != target.GetCoordinates()) {
            this.action = new MovementAction(target, destination, character.movementSpeed, false);
            this.action.Execute();
        }

        character.AddActionFinisher(new AbilityFinisher(character, this.abilityClass, 2));
        character.SetOnCooldown(this.abilityClass, true);

        this.startTime = Time.time;
        return true;
    }
}
