using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Physical {

    protected GameManager gameManager;
	protected Item item;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gameManager.AddPickup(this);
    }

    private void RefreshStatus() {
        // If it is not a child of another gameobject <- that is, not inside a character inventory
        if (transform.parent == null) {
            GameTile tile = gameManager.GetTile(this.GetCoodinates());
            if (tile != null) {
                tile.AddPickup(this);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        RefreshStatus();
    }
	
	public Item GetItem() {
		return this.item;
	}
}
