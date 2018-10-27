using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

abstract public class Character : Physical {

    public float movementSpeed = 3f;
    public bool instantTurn = false;
    public int inventoryCapacity = 16;
	public int itemAmount = 0;
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
	public Weapon rangedWeapon;
    public Pickup[] inventory = new Pickup[16];
	public Pickup[] equipped = new Pickup[4];
	public int itemsEquipped = 0;
	public int items = 0;
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
	public void setInventory(int index, Pickup b){
		this.inventory[index] = b;
	}
    public void RefreshInventory() {
		inventory = GameObject.Find("InventoryInven").GetComponentsInChildren<Pickup>();
		equipped = GameObject.Find("EquippedInven").GetComponentsInChildren<Pickup>();
		for(int j = 0; j < 16; j++){
			GameObject gamex = GameObject.Find("equippedImage ("+j.ToString()+")");
			if(gamex!=null){
				gamex.GetComponent<Image>().sprite = null;
				Image image = gamex.GetComponent<Image>();
				var tempColor = image.color;
				tempColor.a = 0f;
				image.color = tempColor;
			}
		}
		for(int j = 0; j < 16; j++){
			GameObject gamex = GameObject.Find("Image ("+j.ToString()+")");
			if(gamex!=null){
				gamex.GetComponent<Image>().sprite = null;
				Image image = gamex.GetComponent<Image>();
				var tempColor = image.color;
				tempColor.a = 0f;
				image.color = tempColor;
			}
		}
		int index = 0;
		int eqIndex = 0;
		foreach(Pickup item in this.equipped){
			GameObject gamex = GameObject.Find("equippedImage ("+eqIndex.ToString()+")");
			if(gamex!=null){
				gamex.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
				Image image = gamex.GetComponent<Image>();
				var tempColor = image.color;
				tempColor.a = 1f;
				image.color = tempColor;
			}
			eqIndex++;
		}
		foreach(Pickup item in this.inventory){
			GameObject gamex = GameObject.Find("Image ("+index.ToString()+")");
			if(gamex!=null){
				//Debug.Log(this.inventory[0].itemSprite.name);
				gamex.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
				Image image = gamex.GetComponent<Image>();
				var tempColor = image.color;
				tempColor.a = 1f;
				image.color = tempColor;
			}
			index++;
		}
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
	public bool checkPlayerRangedAttack(Character enemy){
			Vector2 difference = this.gameManager.GetPlayer().GetCoordinates()-enemy.GetCoordinates();
			if(difference.x != 0 && difference.y != 0){
				Debug.Log("Not a straight shot!");
				return false;
			}
			else {
				Vector2 coords = this.GetCoordinates();
				Debug.Log("Initial check passed");
				if(difference.x < 0){
					Debug.Log("Character to the right");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.right;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(enemy.GetCoordinates())){
							break;
						}
						if(!gameManager.GetTile(coords).isWalkable){
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.x > 0){
					Debug.Log("Character to the left");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.left;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(enemy.GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.y < 0){
					Debug.Log("Character to the top");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.up;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(enemy.GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.y > 0){
					Debug.Log("Character to downwards");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.down;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(enemy.GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				return true;
			}
	}
	public bool checkRangedAttack(){
		
		if(this != this.gameManager.GetPlayer()){
			Vector2 difference = this.gameManager.GetPlayer().GetCoordinates()-this.GetCoordinates();
			if(difference.x != 0 && difference.y != 0){
				Debug.Log("Not a straight shot!");
				return false;
			}
			else {
				Vector2 coords = this.GetCoordinates();
				Debug.Log("Initial check passed");
				if(difference.x > 0){
					Debug.Log("Character to the right");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.right;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(this.gameManager.GetPlayer().GetCoordinates())){
							break;
						}
						if(!gameManager.GetTile(coords).isWalkable){
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.x < 0){
					Debug.Log("Character to the left");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.left;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(this.gameManager.GetPlayer().GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.y > 0){
					Debug.Log("Character to the top");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.up;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(this.gameManager.GetPlayer().GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				else if(difference.y < 0){
					Debug.Log("Character to downwards");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.down;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(this.gameManager.GetPlayer().GetCoordinates())){
							break;
						}
						if(tileBeingChecked.isWalkable == false){
							Debug.Log(coords);
							Debug.Log("Wall in the way!");
							return false;
						}
					}
					Debug.Log("Can shoot!");
				}
				return true;
			}
		}
		
return true;
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
	//Debug.Log("Weapon set");
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
				this.gameManager.GetPlayer().health += 10;
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
