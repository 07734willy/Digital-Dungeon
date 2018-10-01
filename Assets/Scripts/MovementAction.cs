using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : TurnAction {

    public Vector2 movement;
    public Vector2 coordinates;

    public MovementAction(Character character, Vector2 movement) {
        this.character = character;
        this.movement = movement;
        this.coordinates = character.GetCoodinates();
        this.duration = 0.3f;
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public override void Animate() {
        character.transform.position = Vector2.Lerp(coordinates, coordinates + movement, (Time.time - startTime) / duration);
        if (startTime + duration < Time.time) {
            isComplete = true;
        }
    }

    public override bool Execute() {
        if (!gameManager.IsTileWalkable(character.GetCoodinates() + movement)) {
            return false;
        }
        if (!gameManager.TileSetCharacter(coordinates + movement, character)) {
            return false;
        }
        gameManager.TileUnsetCharacter(coordinates);
        this.startTime = Time.time;
        return true;
    }
}
