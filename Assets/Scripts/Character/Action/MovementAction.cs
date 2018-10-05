using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : TurnAction {

    public Vector2 destination;
    public Vector2 coordinates;

    public MovementAction(Character character, Vector2 destination, float movementSpeed, bool instant) {
        this.character = character;
        this.destination = destination;
        this.coordinates = character.GetCoordinates();
        this.duration = instant ? 0f : 1f / movementSpeed;
        this.gameManager = character.GetGameManager();
    }

    public override bool Check() {
        if ((destination - this.coordinates).sqrMagnitude != 1) {
            return false;
        }
        GameTile tile = gameManager.GetTile(destination);
        if (tile == null || !tile.IsWalkable()) {
            return false;
        }
        return true;
    }

    public override void Animate() {
        if (this.duration > 0) {
            character.transform.position = Vector2.Lerp(coordinates, destination, (Time.time - startTime) / duration);
            if (startTime + duration < Time.time) {
                isComplete = true;
                character.SnapToGrid();
            }
        } else {
            character.transform.position = destination;
            isComplete = true;
            character.SnapToGrid();
        }
    }

    public override bool Execute() {
        if (!Check()) {
            return false;
        }
        this.startTime = Time.time;
        return true;
    }
}
