using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Dictionary<Vector2, GameTile> map;
    private Queue<Character> characterQueue;
    private TurnAction currentAction;

    private void Awake() {
        this.map = new Dictionary<Vector2, GameTile>();
        this.characterQueue = new Queue<Character>();
    }


    private void Update() {

        if (currentAction != null && !currentAction.isComplete) {
            currentAction.Animate();
            return;
        }

        Character character = characterQueue.Peek();
        TurnAction action = character.RequestAction();
        if (action.Execute()) {
            characterQueue.Enqueue(characterQueue.Dequeue());
            currentAction = action;
            currentAction.isComplete = false;
            currentAction.Animate();
        } else {
            action.isComplete = true;
            if (!(character.IsPlayer())) {
                characterQueue.Enqueue(characterQueue.Dequeue());
            }
        }
    }

    public void AddTile (GameTile gameTile) {
        map.Add(gameTile.GetCoodinates(), gameTile);
    }

    public void AddCharacter (Character character) {
        characterQueue.Enqueue(character);
    }

    /*public bool IsTileWalkable(Vector2 coordinate) {
        if (!map.ContainsKey(coordinate)) {
            return false;
        }
        return map[coordinate].isWalkable && (!map[coordinate].GetCharacter());
    }*/

    public GameTile GetTile(Vector2 coordinates) {
        if (map.ContainsKey(coordinates)) {
            return map[coordinates];
        }
        return null;
    }

    public bool TileUnsetCharacter(Vector2 coordinates) {
        return TileSetCharacter(coordinates, null);
    }

    public bool TileSetCharacter(Vector2 coordinates, Character character) {
        if (!map.ContainsKey(coordinates)) {
            return false;
        }
        if (map[coordinates].GetCharacter() && character) {
            return false;
        }
        map[coordinates].SetCharacter(character);
        return true;
    }
}
