using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    override protected void Awake() {
        base.Awake();
        this.isPlayer = false;
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
		if(getDistance() <= 2){
			if(Mathf.Abs(enemyX-playerX) < Mathf.Abs(enemyY-playerY) || (Mathf.Abs(enemyX-playerX) == Mathf.Abs(enemyY-playerY) && Random.Range(0,2) == 0)){
				if(enemyY > playerY){
					return new MovementAction(this, GetCoordinates() + Vector2.down, movementSpeed, false);
				}
				else {
					return new MovementAction(this, GetCoordinates() + Vector2.up, movementSpeed, false);
				}
			}
			else {
				if(enemyX > playerX){
					return new MovementAction(this, GetCoordinates() + Vector2.left, movementSpeed, false);
				}
				else {
					return new MovementAction(this, GetCoordinates() + Vector2.right, movementSpeed, false);
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
		return new MovementAction(this, GetCoordinates() + movement, movementSpeed, false);
	}
}
