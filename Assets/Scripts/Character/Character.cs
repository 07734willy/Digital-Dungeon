using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Character : Physical {

    public float movementSpeed = 3f;
    public bool instantTurn = false;
    public int inventoryCapacity = 8;
    public int health;
    public int maxHealth = 100;
    public float evasion = 0.1f;
    public int armor = 0;
	public int level = 1;
	public int gold = 0;
	public int totalExperience = 0;
	public double expMilestone = 100;
    protected bool isPlayer;
    protected TurnAction currentAction;
    protected TurnAction pendingAction;
    //protected List<Item> inventory;
    protected Weapon equippedWeapon;
    protected Pickup[] inventory;
    protected GameManager gameManager;

    virtual protected void Awake() {
        this.currentAction = new NullAction(this);
        this.pendingAction = new NullAction(this);
        //this.inventory = new List<Item>();
        this.inventory = this.gameObject.GetComponentsInChildren<Pickup>();
        this.health = maxHealth;
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
/*foreach (Pickup pickup in inventory) {
            if (pickup.isWeapon) {
                this.equippedWeapon = (Weapon)pickup;
                Debug.Log("weap equipped");
                break;
            }
        }*/
    }

    public void RefreshPosition() {
        GameTile tile = gameManager.GetTile(this.GetCoordinates());
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

    public void ReceiveDamage(int damage) {
        if (Random.Range(0, 1000) < 1000 * evasion) {
            return;
        }
        int baseArmor = 20;
        Debug.Assert(armor + baseArmor > 0);
        Debug.Log("Damage taken: " + damage / Mathf.Sqrt(armor + baseArmor));
        health -= (int)(damage / Mathf.Sqrt(armor + baseArmor));

        if (health <= 0) {
            this.Kill();
        }
    }

    public void SetWeapon(Weapon weapon) {
        this.equippedWeapon = weapon;
    }

    public Weapon GetWeapon() {
        return this.equippedWeapon;
    }

    public void Kill() {
        if (this.isPlayer) {
            Debug.LogError("Not yet implemented: player death");
        } else {
			//Add experience to the player
			this.gameManager.GetPlayer().totalExperience += 50;
			
			//Increase player's gold
				this.gameManager.GetPlayer().gold += 50;
			
			//Level the player up if the milestone is reached, and adjust the milestone
			if(this.gameManager.GetPlayer().totalExperience >= expMilestone){
				this.gameManager.GetPlayer().level++;
				this.gameManager.GetPlayer().expMilestone *= 1.5;
				
				//Increase base stats upon level up
				this.gameManager.GetPlayer().maxHealth += 10;
				this.gameManager.GetPlayer().armor += 1;
				this.gameManager.GetPlayer().evasion += .1f;
			}
			Debug.LogError(this.gameManager.GetPlayer().gold);
            foreach (Transform child in this.transform) {
                if (child.GetComponent<Pickup>() != null) {
                    child.transform.position = this.transform.position;
                    child.SetParent(null);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
