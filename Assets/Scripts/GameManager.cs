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
    public GameObject pauseMenu;
    public GameObject deathMenu;
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
        if (character == null) {
            characterQueue.Dequeue();
            Update();
            return;
        }
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

    public void setDifficulty(String diffic)
    {
        switch(diffic)
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

        Debug.Log(difficulty);
        print(difficulty);
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

    public void loadNewLevel(string levelName){
        SceneManager.LoadScene(levelName);
    }

    public void setPause(bool shown)
    {
        pauseMenu.SetActive(shown);
        Debug.Log(shown);
    }

   
}