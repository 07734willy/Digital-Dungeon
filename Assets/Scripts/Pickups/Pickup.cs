using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Physical {

    public bool stackable = false;
	public bool isArmor = false;
    public int quantity = 1;
    public int value = 0;
    public bool isConsumeNow;
	public bool isPurchasable = false;
	public int cost = -1;
	public int baseLevel = 0;
    public Sprite itemSprite;
	public string tag_name = "Pickup";
    protected GameManager gameManager;
    protected Character character;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		itemSprite = this.GetComponent<SpriteRenderer>().sprite;
        //gameManager.AddPickup(this);
    }

    public void RefreshStatus() {
        // If it is not a child of another gameobject <- that is, not inside a character inventory

        if (transform.parent == null) {
            GameTile tile = gameManager.GetTile(this.GetCoordinates());
            if (tile != null) {
                tile.AddPickup(this);
            }
            //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            //gameObject.GetComponent<SpriteRenderer>().enabled = false;
            /*if (tile != null && tile.GetCharacter() != null) { 
                this.character = tile.GetCharacter().GetComponent<Character>();
            }*/
        }
    }
	public void SetCharacter (Character character){
		this.character = character;
	}

    public virtual void Select() {
        return;
    }

	// Update is called once per frame
	public virtual void Update () {
        RefreshStatus();
    }
	
	public virtual Pickup Clone () {
		Debug.Log("Wrong cloning");
		return this;
	}
	
	public virtual string GetStats () {
		Debug.Log("Wrong stats");
		return "";
	}
	
	public bool IsPurchasable () {
		return isPurchasable;
	}
	
	public int GetCost () {
		return cost;
	}
	
	public int GetBaseLevel () {
		return baseLevel;
	}
}