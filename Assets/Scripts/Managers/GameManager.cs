using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public enum Difficulty{
		Easy, 
		Normal, 
		Hard, 
		Extreme
	};
	
	public Difficulty difficulty;
    private Dictionary<Vector2, GameTile> map;
    private Queue<Character> characterQueue;
    private TurnAction currentAction;
    private Player player;
    private SaveManager saveManager;

    private void Awake() {
        this.map = new Dictionary<Vector2, GameTile>();
        this.characterQueue = new Queue<Character>();
    }

    private void Start() {
        // it might be possible to update this in Update() with a `.isPlayer` check, but that's only necessary if we do multiplayer
        this.player = GameObject.Find("Player").GetComponent<Player>();
        this.saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }


    private void Update() {

        if (currentAction != null && !currentAction.isComplete) {
            currentAction.Animate();
            return;
        }

        UpdateFog(player.fogDistance);

        Character character = characterQueue.Peek();
        if (character == null) {
            characterQueue.Dequeue();
            Update();
            return;
        }
        character.FinishActions();
        TurnAction action = character.RequestAction();
        if (action.Execute()) {
            if (action.consumeTurn) { 
                characterQueue.Enqueue(characterQueue.Dequeue());
            }
            character.IncrementTurnNumber();
            currentAction = action;
            currentAction.isComplete = false;
            currentAction.Animate();
        } else {
            action.isComplete = true;
            if (!(character is Player)) {
                if (action.consumeTurn) {
                    characterQueue.Enqueue(characterQueue.Dequeue());
                }
                character.IncrementTurnNumber();
            }
        }
    }

    public void AddTile (GameTile gameTile) {
        if (!map.ContainsKey(gameTile.GetCoordinates())) { 
            map.Add(gameTile.GetCoordinates(), gameTile);
        } else {
            Debug.LogError("Error: Duplicate Tile Placement at: " + gameTile.GetCoordinates());
        }
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

    public void UpdateFog(int size) {
        Vector2 topLeft = player.GetCoordinates() + (Vector2.up + Vector2.left) * size;
        int i, j;
        for (i = 0; i < size * 2 + 1; i++) {
            for (j = 0; j < size * 2 + 1; j++) {
                GameTile tile = GetTile(topLeft + Vector2.down * i + Vector2.right * j);
                if (tile != null) {
                    tile.HideFog();
                }
            }
        }
    }
    

    public void setDifficulty(String diffic)
    {
        switch(diffic.ToLower())
        {
            case "easy":
                difficulty = Difficulty.Easy;
                break;
            case "normal":
                difficulty = Difficulty.Normal;
                break;
            case "hard":
                difficulty = Difficulty.Hard;
                break;
            case "extreme":
                difficulty = Difficulty.Extreme;
                break;
            default:
                difficulty = Difficulty.Normal;
                break;
        }
    }

    public Player GetPlayer() {
        return this.player;
    }

    public SaveManager GetSaveManager() {
        return this.saveManager;
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
        Debug.Log("GameManagerSetCharacter");
		return true;
    }
    
    public void loadNewLevel(string levelName){
        this.saveManager.SaveData();
        SceneManager.LoadScene(levelName);
    }
}
