using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Character : Physical {

    public float movementSpeed = 3f;
    public int inventoryCapacity = 8;
    protected bool isPlayer;
    protected TurnAction currentAction;
    protected TurnAction pendingAction;
    //protected List<Item> inventory;
    protected Pickup[] inventory;
    protected GameManager gameManager;

    virtual protected void Awake() {
        this.currentAction = new NullAction(this);
        this.pendingAction = new NullAction(this);
        //this.inventory = new List<Item>();
        this.inventory = this.gameObject.GetComponentsInChildren<Pickup>();
    }
    
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddCharacter(this);
    }

    public void RefreshInventory() {
        inventory = gameObject.GetComponentsInChildren<Pickup>();
        Debug.Assert(inventoryCapacity >= 0);
        while (inventory.Length > inventoryCapacity) {
            inventory[inventory.Length - 1].transform.parent = null;
            inventory = gameObject.GetComponentsInChildren<Pickup>();
        }
    }

    public void RefreshPosition() {
        GameTile tile = gameManager.GetTile(this.GetCoodinates());
        if (tile != null) {
            Debug.Assert(tile.GetCharacter() == null || tile.GetCharacter() == this);
            tile.SetCharacter(this);
        }
    }

    virtual protected void Update() {
        RefreshPosition();
        RefreshInventory();
    }

    abstract public TurnAction RequestAction();

    public bool IsPlayer() {
        return this.isPlayer;
    }

    public GameManager GetGameManager() {
        return gameManager;
    }

    public int SpareInventoryCapacity() {
        return inventoryCapacity - inventory.Length;
    }

    public void SetPendingAction(TurnAction action) {
        this.pendingAction = action;
    }
}
