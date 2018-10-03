using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    override protected void Awake() {
        base.Awake();
        this.isPlayer = false;
    }

    public override TurnAction RequestAction() {
        Debug.Assert(currentAction.isComplete);
		if(getDistance() < 2){
			
		}
		return getRandomMovement();
    }
	
	public int getDistance(){
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
		int playerX, playerY, enemyX, enemyY;
		playerX = (int)playerCoords.x;
		playerY = (int)playerCoords.y;
		enemyX = (int)enemyCoords.x;
		enemyY = (int)enemyCoords.y;
		return (int)Mathf.Abs(Mathf.Sqrt((enemyX - playerX) + (enemyY - playerY)));
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
