using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTile : Physical, IPointerClickHandler {

    //public GameManager gameManager;
    public bool isWalkable;
    public GameObject fogPrefab;
	protected bool fogActivated = true;
    protected GameObject fog;
    protected Character character;
    private GameManager gameManager;
    private HashSet<Pickup> pickups;

    protected virtual void Awake() {
        this.pickups = new HashSet<Pickup>();
        this.fog = Instantiate<GameObject>(fogPrefab);
        this.fog.transform.parent = this.transform;
        this.fog.transform.position = this.transform.position;
    }

    // Use this for initialization
    protected virtual void Start () {
		//this.fog.SetActive(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddTile(this);
	}
	public bool getFogActivated(){
		return this.fogActivated;
	}
	public void setFogActivated(bool x){
		this.fogActivated= x;
	}
    public void RefreshContents() {
        if (character != null && character.GetCoordinates() != this.GetCoordinates()) {
            Debug.LogError("Character was moved from tile without updating tile");
            this.character = null;
        }
        foreach (Pickup pickup in pickups) {
            if (pickup == null || pickup.GetCoordinates() != this.GetCoordinates() || pickup.transform.parent != null) {
                Debug.Log("Pickup invalid / not in gametile, yet listed as such in the GameTile.pickups");
                RemovePickup(pickup);
                // This is because we can't modify the contents of a HashSet while iterating over it
                RefreshContents();
                break;
            }
        }
    }

    private void Update() {
        RefreshContents();
    }

    public void OnPointerClick(PointerEventData eventData) {
        Player player = gameManager.GetPlayer();
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (this.character != null && character != player && (player.GetCoordinates() - character.GetCoordinates()).magnitude > 1) {
                if (this.gameManager.GetPlayer().GetRangedWeapon() != null) {
                    Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
                    Vector2 enemyCoords = this.character.GetCoordinates();
                    int difference = (int)Mathf.Abs((playerCoords - enemyCoords).magnitude);
                    if (difference <= this.gameManager.GetPlayer().GetRangedWeapon().range) {
                        if (this.gameManager.GetPlayer().checkPlayerRangedAttack(this.character)) {
                            if (this.gameManager.GetPlayer().arrows > 0) {
                                player.SetPendingAction(new RangedAttackAction(player, character, player.movementSpeed, player.instantTurn));
                                this.gameManager.GetPlayer().arrows--;
                            } else {
                                Debug.Log("No ammunition!");
                            }
                        }
                    } else {
                        Debug.Log("Out of range!");
                    }
                }
                //player.SetPendingAction(new RangedAttackAction(player, character, player.movementSpeed, player.instantTurn));
            } else if (this.character != null && character != player) {
                //attack
                player.SetPendingAction(new MeleeAttackAction(player, character, player.movementSpeed, player.instantTurn));
				this.gameManager.GetPlayer().shiftLogBox();
				this.gameManager.GetPlayer().logs[0]="Enemy has " + character.health + " health";
            } else if (player.GetCoordinates() != this.GetCoordinates()) {
                // pathfind, and MoveAction() towards it.
            } else if (this.pickups.Count > 0) {
                foreach (Pickup pickup in pickups) {
                    // Don't waste a turn trying to pickup if you can't
                    if (player.SpareInventoryCapacity() > 0 || pickup is ConsumeNow) {
                        player.SetPendingAction(new PickupAction(player, pickup));
                    }
                    // this is a nasty hack, but we can only (and want to) pick up one item per turn, so to retrieve one item from a hashset...we do this
                    break;
                }
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            if (this.fog.activeSelf) {
                return;
            }
            Dialog dialog;
            if (this.character != null && (dialog = this.character.GetComponent<Dialog>()) != null && dialog.CanInspect()) {
                dialog.DisplayInspection();
            } else {
                foreach (Pickup pickup in this.pickups) {
                    if ((dialog = pickup.GetComponent<Dialog>()) != null && dialog.CanInspect()) {
                        dialog.DisplayInspection();
                        return;
                    }
                }
                if ((dialog = this.gameObject.GetComponent<Dialog>()) != null && dialog.CanInspect()) {
                    dialog.DisplayInspection();
                }
            }
        }
    }

    public Character GetCharacter() {
        RefreshContents();
        return character;
    }

    public virtual void SetCharacter(Character character) {
        Character oldCharacter = this.character;
		this.character = character;
		//Debug.Log("SetCharacter");
		Dialog dialog = this.GetComponent<Dialog>();
		string message = "";
		foreach (Pickup pickup in pickups) {
            if(pickup.IsPurchasable()){
				message += pickup.GetStats();
			}
		}
		
		if (dialog != null && message != "" && oldCharacter == null) {
			dialog.message = message;
            dialog.DisplayDialogMessage();
        }
    }

    public void AddPickup(Pickup pickup) {
        // Note- we rely on HashSet's unique poperty to ensure duplicates don't get inserted
        this.pickups.Add(pickup);
    }

    public bool RemovePickup(Pickup pickup) {
        return this.pickups.Remove(pickup);
    }

    public bool IsWalkable() {
        return this.isWalkable && (character == null);
    }

    public void HideFog() {
        this.fog.SetActive(false);
		this.setFogActivated(false);
    }
	
	public HashSet<Pickup> GetPickups(){
		return pickups;
	}
}
