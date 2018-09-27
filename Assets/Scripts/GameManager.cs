using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Dictionary<Vector2, GameTile> map;
    private Queue<Character> characterQueue;
    private TurnAction currentAction;
    private Player player;

    private void Awake() {
        this.map = new Dictionary<Vector2, GameTile>();
        this.characterQueue = new Queue<Character>();
    }

    private void Start() {
        // it might be possible to update this in Update() with a `.isPlayer` check, but that's only necessary if we do multiplayer
        this.player = GameObject.Find("Player").GetComponent<Player>();
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

    public GameTile GetTile(Vector2 coordinates) {
        if (map.ContainsKey(coordinates)) {
            return map[coordinates];
        }
        return null;
    }

    public Player GetPlayer() {
        return this.player;
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
