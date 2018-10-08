using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
	public Sprite alertImage;
	public Sprite engageImage;
    override protected void Awake() {
        base.Awake();
        this.isPlayer = false;
    }
	
	public void setAlertLevel(int x){
		SpriteRenderer renderer = null;
		foreach(Transform child in this.transform){
			if(child.name == "alertIcon"){
				renderer = child.GetComponent<SpriteRenderer>();
			}
		}
        if (renderer == null) {
            Debug.LogError("couldn't get alertIcon");
            return;
        }
		switch (x) {
			case 0:
			renderer.sprite = null;
				break;
			case 1:
			renderer.sprite = alertImage;
				break;
			case 2:
			renderer.sprite = engageImage;
				break;
			default:
			break;
		}
	}
	
    public override TurnAction RequestAction() {
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
		int playerX, playerY, enemyX, enemyY;
		playerX = (int)playerCoords.x;
		playerY = (int)playerCoords.y;
		enemyX = (int)enemyCoords.x;
		enemyY = (int)enemyCoords.y;
        Debug.Assert(currentAction.isComplete);

        if (getDistance() <= 4) {
            setAlertLevel(1);
            if (getDistance() <= 2.0001) {
                setAlertLevel(2);
                // Future combat stuff will go here
            }
            if (Mathf.Abs(enemyX-playerX) < Mathf.Abs(enemyY-playerY) || (Mathf.Abs(enemyX-playerX) == Mathf.Abs(enemyY-playerY) && Random.Range(0,2) == 0)){
				if(enemyY > playerY){
					return new MovementAction(this, GetCoordinates() + Vector2.down, this.movementSpeed, this.instantTurn);
				}
				else {
					return new MovementAction(this, GetCoordinates() + Vector2.up, this.movementSpeed, this.instantTurn);
				}
			}
			else {
				if(enemyX > playerX){
					return new MovementAction(this, GetCoordinates() + Vector2.left, this.movementSpeed, this.instantTurn);
				}
				else {
					return new MovementAction(this, GetCoordinates() + Vector2.right, this.movementSpeed, this.instantTurn);
				}
			}
		}
		return getRandomMovement();
    }
	
	public int getDistance(){
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
        return (int)Mathf.Abs((playerCoords - enemyCoords).magnitude);
		/*int playerX, playerY, enemyX, enemyY;
		playerX = (int)playerCoords.x;
		playerY = (int)playerCoords.y;
		enemyX = (int)enemyCoords.x;
		enemyY = (int)enemyCoords.y;
		return (int)Mathf.Abs(Mathf.Sqrt((enemyX - playerX) + (enemyY - playerY)));*/
	}
	
	public TurnAction getRandomMovement(){
		setAlertLevel(0);
		int x = Random.Range(0,4);
		Vector2 movement;
		switch(x){
			case 0:
				movement = Vector2.up;
				break;
			case 1:
				movement = Vector2.right;
				break;
			case 2:
				movement = Vector2.down;
				break;
			default:
				movement = Vector2.left;
				break;
			
		}
		return new MovementAction(this, GetCoordinates() + movement, this.movementSpeed, this.instantTurn);
	}
}