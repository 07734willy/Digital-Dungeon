using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
public class Coodinates {
    private int x1, x2;
    public Coodinates(int x1, int x2) {
        this.x1 = x1;
        this.x2 = x2;
    }
}*/



public class GameManager : MonoBehaviour {

    private Dictionary<Vector2, GameTile> map;
    private Queue<Character> characterQueue;

    private void Awake() {
        this.map = new Dictionary<Vector2, GameTile>();
        this.characterQueue = new Queue<Character>();
    }

    private void Update() {
        Character character = characterQueue.Peek();
        bool successful = false;
        TurnAction turnAction = character.RequestAction();
        switch (turnAction.action) {
            case TurnAction.ActionType.Movement:
                successful = RequestMovement(character, turnAction.movement);
                TileSetCharacter(turnAction.coordinates, null);
                TileSetCharacter(turnAction.coordinates + turnAction.movement, character);
                break;
            case TurnAction.ActionType.None:
                successful = false;
                break;
            case TurnAction.ActionType.Animation:
                // return early, to allow the animation to continue
                return;
            default:
                Debug.Log("Failed TurnAction switch");
                break;
        }
        if (successful || !(character is Player)) {
            character.SetAction(turnAction);
            characterQueue.Enqueue(characterQueue.Dequeue());
        }
    }

    public void AddTile (GameTile gameTile) {
        map.Add(gameTile.GetCoodinates(), gameTile);
    }

    public void AddCharacter (Character character) {
        characterQueue.Enqueue(character);
    }

    public bool RequestMovement(Character character, Vector2 movement) {
        /*// We return true because the move is accepted/acknowledged (yet ignored due to being out of turn)
        if (characterQueue.Peek() != character) {
            return true;
        }*/
        /*// Test to make sure the move is valid
        if (IsTileWalkable(character.GetCoodinates() + movement)) {
            //character.transform.Translate(movement);
            characterQueue.Enqueue(characterQueue.Dequeue());
            return true;
        }*/
        /*// If not the player, invalid moves mean they lose their turn, and a debug message is logged
        if (!(character is Player)) {
            Debug.Log(character);
            Debug.Log(" ^^ Wasted their turn with an invalid move.");
            characterQueue.Enqueue(characterQueue.Dequeue());
            // We return true because the move is accepted/acknowledged (yet ignored due to being invalid)
            return true;
        }*/
        // The only time we return false is when the move is rejected, and we wish to request a new move
        /*return false;*/
        return IsTileWalkable(character.GetCoodinates() + movement);
    }

    public bool IsTileWalkable(Vector2 coordinate) {
        if (!map.ContainsKey(coordinate)) {
            return false;
        }
        return map[coordinate].isWalkable && (!map[coordinate].GetCharacter());
    }

    private bool TileSetCharacter(Vector2 coordinate, Character character) {
        if (!map.ContainsKey(coordinate)) {
            return false;
        }
        map[coordinate].SetCharacter(character);
        return true;
    }
}
