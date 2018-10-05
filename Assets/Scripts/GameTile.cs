using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : Physical {

    //public GameManager gameManager;
    public bool isWalkable;
    private Character character;
    private GameManager gameManager;
    private HashSet<Pickup> pickups;

    private void Awake() {
        this.pickups = new HashSet<Pickup>();
    }

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddTile(this);
	}


    public void RefreshContents() {
        if (character != null && character.GetCoordinates() != this.GetCoordinates()) {
            this.character = null;
        }
        foreach (Pickup pickup in pickups) {
            if (pickup == null || pickup.GetCoordinates() != this.GetCoordinates() || pickup.transform.parent != null) {
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

    private void OnMouseDown() {
        Player player = gameManager.GetPlayer();
        if (this.character != null && character != player) {
            //attack
            player.SetPendingAction(new MeleeAttackAction(player, character, 3, true));
        } else if (player.GetCoordinates() != this.GetCoordinates()) {
            // pathfind, and MoveAction() towards it.
        } else if (this.pickups.Count > 0) {
            foreach (Pickup pickup in pickups) {
                // Don't waste a turn trying to pickup if you can't
                if (player.SpareInventoryCapacity() > 0) {
                    player.SetPendingAction(new PickupAction(player, pickup));
                }
                // this is a nasty hack, but we can only (and want to) pick up one item per turn, so to retrieve one item from a hashset...we do this
                break;
            }
        }
    }

    public Character GetCharacter() {
        RefreshContents();
        return character;
    }

    public void SetCharacter(Character character) {
        this.character = character;
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
}
