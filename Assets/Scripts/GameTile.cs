﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {

    //public GameManager gameManager;
    public bool isWalkable;
    private Character character;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddTile(this);
	}

    private void Update() {
        if (character != null && character.GetCoodinates() != this.GetCoodinates()) {
            this.character = null;
        }
    }

    public Character GetCharacter() {
        this.Update();
        return character;
    }

    public void SetCharacter(Character character) {
        this.character = character;
    }

    public bool IsWalkable() {
        return this.isWalkable && (character == null);
    }

    public Vector2 GetCoodinates() {
        return (Vector2)transform.position;
        // May need to round off the coordinates to prevent floating-point related errors later. Just in case:
        //return new Vector2(Mathf.round(transform.position.x), Mathf.round(transform.position.y));
    }
}
