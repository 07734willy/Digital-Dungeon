using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Physical {

    public bool stackable = false;
    public int quantity = 1;
    public int value = 0;
    public bool isWeapon;
    public bool isArmor;
    public bool isConsumable;
    protected GameManager gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gameManager.AddPickup(this);
    }

    public void RefreshStatus() {
        // If it is not a child of another gameobject <- that is, not inside a character inventory
        if (transform.parent == null) {
            GameTile tile = gameManager.GetTile(this.GetCoordinates());
            if (tile != null) {
                tile.AddPickup(this);
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public virtual void Use() {
        return;
    }
	
	// Update is called once per frame
	public virtual void Update () {
        RefreshStatus();
    }
}
