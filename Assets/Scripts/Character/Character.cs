using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

abstract public class Character : Physical {
    public enum AbilityClass {
        None,
        Heal,
        Spin,
        Teleport,
        Fury,
        Equilibrium,
        Push
    }

    public float movementSpeed = 3f;
    public bool instantTurn = false;
    public int inventoryCapacity;
	public int itemAmount = 0;
    public int health;
    public int maxHealth = 100;
    public float evasion = 0.1f;
    public int armor = 0;
	public int level = 1;
	public int gold = 0;
	public int keys = 0;
	public int arrows = 0;
	public int totalExperience = 0;
	public double expMilestone = 100;

    protected int turnNumber = 0;

    protected Dictionary<int, List<ActionFinisher>> actionFinishers;
    protected TurnAction currentAction;
    protected TurnAction pendingAction;

    protected Dictionary<AbilityClass, int> abilityLevel;
    protected Dictionary<AbilityClass, bool> onCooldown;

    protected Weapon meleeWeapon;
	[SerializeField]
    protected Weapon rangedWeapon;

	public Pickup[] inventory = new Pickup[16];
	public Pickup[] equipped = new Pickup[4];
	public int itemsEquipped = 0;
	public int items = 0;
    protected GameManager gameManager;

    private bool initialized = false;

    virtual protected void Awake() {
        this.currentAction = new NullAction(this);
        this.pendingAction = new NullAction(this);
        //this.inventory = new List<Item>();
        this.inventory = this.gameObject.GetComponentsInChildren<Pickup>();
        this.health = maxHealth;
        this.actionFinishers = new Dictionary<int, List<ActionFinisher>>();
        this.onCooldown = new Dictionary<AbilityClass, bool>();
    }
    
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddCharacter(this);
    }
	
	public void setInventory(int index, Pickup pickup){
		this.inventory[index] = pickup;
	}

    public void RefreshInventory() {
        Debug.Assert(inventoryCapacity >= 0);
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
				if(item.name == "invenDummy"){
					gamex.GetComponent<Image>().sprite = null;
					Image image = gamex.GetComponent<Image>();
					var tempColor = image.color;
					tempColor.a = 0f;
					image.color = tempColor;
				}
				else {
					gamex.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
					Image image = gamex.GetComponent<Image>();
					var tempColor = image.color;
					tempColor.a = 1f;
					image.color = tempColor;
				}
			}
			index++;
		}
    }
    public void RefreshPosition() {
        GameTile tile = gameManager.GetTile(this.GetCoordinates());
        if (tile != null) {
            Debug.Assert(tile.GetCharacter() == null || tile.GetCharacter() == this);
            if (tile.GetCharacter() != this) {
                Debug.LogError("Player moved to tile, but tile was not updated");
				tile.SetCharacter(this);
			}
        }
    }

    private void Initialize() {
        if (!this.initialized) {
            this.initialized = true;
			//Debug.Log("Initialize");
            if (this.gameObject == null) {
                Debug.Log("Player not on a gametile");
            }
            gameManager.GetTile(this.GetCoordinates()).SetCharacter(this);
        }
    }

    virtual protected void Update() {
        Initialize();
        RefreshPosition();
        RefreshInventory();
    }

    public void AddActionFinisher(ActionFinisher finisher) {
        if (!this.actionFinishers.ContainsKey(finisher.finishTurn)) {
            this.actionFinishers[finisher.finishTurn] = new List<ActionFinisher>();
        }
        this.actionFinishers[finisher.finishTurn].Add(finisher);
    }

    public void FinishActions() {
        if (this.actionFinishers.ContainsKey(this.turnNumber)) {
            foreach (ActionFinisher finisher in this.actionFinishers[this.turnNumber]) {
                finisher.Execute();
            }
        }
    }

    abstract public TurnAction RequestAction();

    public int GetTurnNumber() {
        return turnNumber;
    }

    public void IncrementTurnNumber() {
        this.turnNumber += 1;
    }

    public GameManager GetGameManager() {
        return gameManager;
    }

    public int SpareInventoryCapacity() {
		int count = 0;
		for(int i = 0; i < inventory.Length; i++){
			if(inventory[i].GetComponent<dummyItem>()){
				count++;
			}
		}
        return inventoryCapacity - count;//inventory.Length;
        //return inventoryCapacity - inventory.Length;
    }

    public void SetPendingAction(TurnAction action) {
        this.pendingAction = action;
    }

	public bool checkPlayerRangedAttack(Character enemy){
			Vector2 difference = this.gameManager.GetPlayer().GetCoordinates()-enemy.GetCoordinates();
			if(difference.x != 0 && difference.y != 0){
				Debug.Log("Not a straight shot!");
				this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Not a straight shot!";
				return false;
			}
			else {
				Vector2 coords = this.GetCoordinates();
				Debug.Log("Initial check passed");
				if(difference.x < 0){
					Debug.Log("Character to the right");
					for(int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.right;
						//GameTile tileBeingChecked = gameManager.GetTile(coords);
						if(coords.Equals(enemy.GetCoordinates())){
							break;
						}
						if(!gameManager.GetTile(coords).isWalkable){
							Debug.Log("Wall in the way!");
							this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Wall in the way of shot!";
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
							this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Wall in the way of shot!";
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
							this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Wall in the way of shot!";
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
							this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Wall in the way of shot!";
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
						//GameTile tileBeingChecked = gameManager.GetTile(coords);
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
				else if (difference.y < 0){
					Debug.Log("Character to downwards");
					for (int i = 0; i < this.rangedWeapon.range; i++){
						coords = coords + Vector2.down;
						GameTile tileBeingChecked = gameManager.GetTile(coords);
						if (coords.Equals(this.gameManager.GetPlayer().GetCoordinates())){
							break;
						}
						if (tileBeingChecked.isWalkable == false){
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
	
    public void SetOnCooldown(AbilityClass abilityClass, bool value) {
        this.onCooldown[abilityClass] = value;
    }

    public bool IsOnCooldown(AbilityClass abilityClass) {
        bool value;
        return this.onCooldown.TryGetValue(abilityClass, out value) ? value : false;
    }
    public virtual void ReceiveDamage(int damage) {
        if (Random.Range(0, 1000) < 1000 * evasion) {
			Debug.Log("Evaded damage!");
			if(this == this.gameManager.GetPlayer()){
				this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="You evaded damage!";
			}
			else {
				this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Enemy evaded damage!";
			}
            return;
        }
        int baseArmor = 20;
        Debug.Assert(armor + baseArmor > 0);
        Debug.Log("Damage taken: " + damage / Mathf.Sqrt(armor + baseArmor));
        health -= (int)(damage / Mathf.Sqrt(armor + baseArmor));
		if(this == this.gameManager.GetPlayer()){
			this.gameManager.GetPlayer().shiftLogBox();
			this.gameManager.GetPlayer().logs[0]="You took " + damage / Mathf.Sqrt(armor + baseArmor) + " damage!";
		}
        if (health <= 0) {
            this.Kill();
        }
    }

    public void Heal(int heal) {
        this.health += heal;
        if (this.health > this.maxHealth) {
            this.health = this.maxHealth;
        }
    }

    public void SetMeleeWeapon(Weapon weapon) {
        this.meleeWeapon = weapon;
    }

    public Weapon GetMeleeWeapon() {
        return this.meleeWeapon;
    }

    public void SetRangedWeapon(Weapon weapon) {
        this.rangedWeapon = weapon;
    }

    public Weapon GetRangedWeapon() {
        return this.rangedWeapon;
    }

    public void SetAbilityLevel(AbilityClass abilityClass, int level) {
        this.abilityLevel[abilityClass] = level;
    }

    public int getAbilityLevel(AbilityClass abilityClass) {
        if (!this.abilityLevel.ContainsKey(abilityClass)) {
            return 0;
        }
        return this.abilityLevel[abilityClass];
    }
	public void upgradeStats(int armInc, float evaInc, int maxHealthInc, int healthInc){
				this.maxHealth += maxHealthInc;
				this.health += healthInc;
				this.armor += armInc;
				this.evasion += evaInc;
	}
    public void Kill() {
        if (this is Player) {
            this.health = -1;
            gameManager.instantLoad("DeathScene");
        } else {
        	this.gameManager.GetPlayer().completeAchievement("First Enemy Defeated");
			//Add experience to the player
			this.gameManager.GetPlayer().totalExperience += 50;
			
			//Increase player's gold
				this.gameManager.GetPlayer().gold += 50;
			
			//Level the player up if the milestone is reached, and adjust the milestone
			if(this.gameManager.GetPlayer().totalExperience >= this.gameManager.GetPlayer().expMilestone){
				this.gameManager.GetPlayer().level++;
				this.gameManager.GetPlayer().expMilestone *= 2.5;
				
				//Increase base stats upon level up
				this.gameManager.GetPlayer().upgradeStats(10, .1f, 10, 10);
				this.gameManager.GetPlayer().health = this.gameManager.GetPlayer().maxHealth;
				//Level up all enemies
				var enemies = FindObjectsOfType<Enemy>();
				foreach(Enemy enemy in enemies){
					enemy.upgradeStats(5, .25f, 10, 10);
				}
			}
			//Debug.Log(this.gameManager.GetPlayer().gold);
            foreach (Transform child in this.transform) {
                if (child.GetComponent<Pickup>() != null) {
                	child.GetComponent<SpriteRenderer>().enabled = true;
                    child.transform.position = this.transform.position;
                    child.SetParent(null);
                }
            }
            Destroy(this.gameObject);
        }
    }
	
	public int GetGold () {
		return gold;
	}
	
	public void SetGold (int newGold) {
		gold = newGold;
	}
	
	public int GetLevel () {
		return level;
	}
	
	public int GetHealth () {
		return health;
	}
}
