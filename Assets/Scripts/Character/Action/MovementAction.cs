using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : TurnAction {

    public Vector2 movement;
    public Vector2 coordinates;

    public MovementAction(Character character, Vector2 movement, float movementSpeed, bool instant) {
        this.character = character;
        this.movement = movement;
        this.coordinates = character.GetCoodinates();
        this.duration = instant ? 0f : 1f / movementSpeed;
        this.gameManager = character.GetGameManager();
    }

    public override void Animate() {
        if (this.duration > 0) {
            character.transform.position = Vector2.Lerp(coordinates, coordinates + movement, (Time.time - startTime) / duration);
            if (startTime + duration < Time.time) {
                isComplete = true;
                character.SnapToGrid();
            }
        } else {
            character.transform.position = coordinates + movement;
            isComplete = true;
            character.SnapToGrid();
        }
    }

    public override bool Execute() {
        GameTile tile = gameManager.GetTile(character.GetCoodinates() + movement);
        if (tile == null || !tile.IsWalkable()) {
            return false;
        }
        this.startTime = Time.time;
        return true;
    }
}
