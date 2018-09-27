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


    private void RefreshContents() {
        if (character != null && character.GetCoodinates() != this.GetCoodinates()) {
            this.character = null;
        }
        foreach (Pickup pickup in pickups) {
            if (pickup == null || pickup.GetCoodinates() != this.GetCoodinates() || pickup.transform.parent != null) {
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
