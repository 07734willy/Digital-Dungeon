using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Character : MonoBehaviour {

    public float movementSpeed = 3f;
    protected bool isPlayer;
    protected TurnAction currentAction;
    protected TurnAction pendingAction;
    protected GameManager gameManager;

    virtual protected void Awake() {
        this.currentAction = new NullAction(this);
        this.pendingAction = new NullAction(this);
    }
    
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddCharacter(this);
    }

    virtual protected void Update() {
        GameTile tile = gameManager.GetTile(this.GetCoodinates());
        if (tile != null && tile.GetCharacter() == null) {
            tile.SetCharacter(this);
        }
    }

    public Vector2 GetCoodinates() {
        return transform.position;
        // May need to round off the coordinates to prevent floating-point related errors later. Just in case:
        //return new Vector2(Mathf.round(transform.position.x), Mathf.round(transform.position.y));
    }

    abstract public TurnAction RequestAction();

    public bool IsPlayer() {
        return this.isPlayer;
    }

    public GameManager GetGameManager() {
        return gameManager;
    }
}
