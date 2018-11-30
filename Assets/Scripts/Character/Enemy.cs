using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public Sprite alertImage;
	public Sprite engageImage;
	public struct visitedNode {
			public Vector2 visit;
			public Vector2 visitedBy;
			public visitedNode (Vector2 x, Vector2 y){
				visit = x;
				visitedBy = y;
			}
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
                Debug.LogError("Invalid alert level to setAlertLevel()");
			break;
		}
	}
	public bool searchList(List<visitedNode> list, Vector2 x){
		for(int i = 0; i < list.Count; i++){
			if(list[i].visit == x){
				return true;
			}
		}
		return false;
	}
	
	//Finds a specific node in the given list
	public visitedNode searchListForNode(List<visitedNode> list, Vector2 x){
		visitedNode z = new visitedNode(GetCoordinates(), GetCoordinates());
		for(int i = 0; i < list.Count; i++){
				if(list[i].visit == x){
					return list[i];
				}
		}
		return z;
	}
	//Goes back through the list of nodes starting from the player's tile to get the first tile from the enemy in the shortest path to the player
	
	public Vector2 easyPath(){
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
		int playerX, playerY, enemyX, enemyY;
		playerX = (int)playerCoords.x;
		playerY = (int)playerCoords.y;
		enemyX = (int)enemyCoords.x;
		enemyY = (int)enemyCoords.y;
        Debug.Assert(currentAction.isComplete);
        if (Mathf.Abs(enemyX-playerX) < Mathf.Abs(enemyY-playerY) || (Mathf.Abs(enemyX-playerX) == Mathf.Abs(enemyY-playerY) && Random.Range(0,2) == 0)){
			if(enemyY > playerY){
				return GetCoordinates() + Vector2.down;
			}
			else {
				return GetCoordinates() + Vector2.up;
			}
		}
		else {
			if(enemyX > playerX){
				return GetCoordinates() + Vector2.left;
			}	
			else {
				return GetCoordinates() + Vector2.right;
			}
		}
	}

	public Vector2 backTrack(List<visitedNode> list){
		//The player coordinates are the end of the path we want, so find them in the list
		visitedNode x = searchListForNode(list, this.gameManager.GetPlayer().GetCoordinates());
		
		//While the current visitedNode isn't one move away from our starting point, backtrack 
		while(x.visitedBy != GetCoordinates()){
			x = searchListForNode(list, x.visitedBy);
		}
		
		//we are now on the tile that is one move away from the enemy on the shortest path to the player, return it if it is not the players coords
		if(x.visit.Equals(this.gameManager.GetPlayer().GetCoordinates())){
			return Vector2.zero;
		}
		return x.visit;
	}
	
	//Locates the shortest path between the player and the enemy
	public Vector2 shortestPath(){
		bool pathFound = false;
		//list that holds all the visitedNode structs (tile currently being visited, which tile visited it)
		List<visitedNode> visited = new List<visitedNode>();
		//Queue to hold tiles that need to be searched
		Queue<Vector2> searchingQueue = new Queue<Vector2>();
		
		//Add enemy tile to the queue to start the process
		searchingQueue.Enqueue(GetCoordinates());
		
		//Add current game tile to the list of visited nodes to make sure the enemy's tile isn't processed again
		visitedNode starter = new visitedNode(GetCoordinates(), GetCoordinates());
		visited.Add(starter);
		
		while(pathFound == false){
			//Grab next game tile to be processed
			if(searchingQueue.Count == 0){
				return Vector2.zero;
			}
			Vector2 current = searchingQueue.Dequeue();
			
			//Check to see if the current vector is the player's tile
			if(current.Equals(this.gameManager.GetPlayer().GetCoordinates())){
				pathFound = true;
			}
			
			//if not, Grab all neighboring tiles
			GameTile up =    gameManager.GetTile(current + Vector2.up);
			GameTile down =  gameManager.GetTile(current + Vector2.down);
			GameTile right = gameManager.GetTile(current + Vector2.right);
			GameTile left =  gameManager.GetTile(current + Vector2.left);
			
			//For each neighbor, check to see if it can be walked on, and make sure it hasn't already been visited
			//via the searchList function
			if(up != null && up.isWalkable == true){
				if(searchList(visited, current + Vector2.up) == false){
					visitedNode z = new visitedNode(current + Vector2.up, current);
					visited.Add(z);
					searchingQueue.Enqueue(current+Vector2.up);
				}
			}
			if(down != null && down.isWalkable == true){
				if(searchList(visited, current + Vector2.down) == false){
					visitedNode z = new visitedNode(current + Vector2.down, current);
					visited.Add(z);
					searchingQueue.Enqueue(current+Vector2.down);
				}
			}
			if(right != null && right.isWalkable == true){
				if(searchList(visited, current + Vector2.right) == false){
					visitedNode z = new visitedNode(current + Vector2.right, current);
					visited.Add(z);
					searchingQueue.Enqueue(current+Vector2.right);
				}
			}
			if(left != null && left.isWalkable == true){
				if(searchList(visited, current + Vector2.left) == false){
					visitedNode z = new visitedNode(current + Vector2.left, current);
					visited.Add(z);
					searchingQueue.Enqueue(current + Vector2.left);
				}
			}
		}
		
		//The path has been found, now backtrack through the list to find what the next move is
		return backTrack(visited);
	}
	public void decideInstant(){
		if(gameManager.GetTile(GetCoordinates()).getFogActivated()){
					this.instantTurn = true;
				}
				else {
					this.instantTurn = false;
				}
	}
    public override TurnAction RequestAction() {
		this.decideInstant();
        Debug.Assert(currentAction.isComplete);
        //Get the overall distance to determine which pathfinding to use
		if (getDistance() <= 4) {
			//Set the alert level to "alert"
			setAlertLevel(1);
			if(getDistance() <= 2.0001){
				//Set the alert level to "engaged"
				setAlertLevel(2);
			}
			/* Implement condition for ranged attack here */
			if(this.rangedWeapon != null){
				if(this.checkRangedAttack()){
					if(getDistance() <= this.rangedWeapon.range){
						Debug.Log("Ranged attack launched!");
						return new RangedAttackAction(this, this.gameManager.GetPlayer(), this.movementSpeed, this.instantTurn);
					}
				}
			}
			/* If not within range for ranged attack, get path movement */
			if(shortestPath().Equals(Vector2.zero)==false){
				switch (gameManager.difficulty)
				{
					case GameManager.Difficulty.Easy:
						return new MovementAction(this, easyPath(), this.movementSpeed, this.instantTurn);	
					default:
						return new MovementAction(this, shortestPath(), this.movementSpeed, this.instantTurn);
				}
			}
			
			/* If too close for path movement, enemy is within distance of a melee attack */
			if(getDistance() <= 1.0001){
				return new MeleeAttackAction(this, this.gameManager.GetPlayer(), this.movementSpeed, this.instantTurn);
			}
			
			/* Just wait */
			return new WaitAction(this);
		}
		//Otherwise, set alert level to "unalert" and move randomly
		setAlertLevel(0);
		return getRandomMovement();
    }
	
	//Function to determine overall distance between this character and the player
	public int getDistance(){
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
        return (int)Mathf.Abs((playerCoords - enemyCoords).magnitude);
	}
	
	//Returns a random movement for the enemy to move in
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
		return new MovementAction(this, GetCoordinates() + movement, this.movementSpeed, this.instantTurn);
	}
	
	public override void ReceiveDamage (int damage){
		float multiplier = 0f;
		switch (gameManager.difficulty)
		{
			case GameManager.Difficulty.Easy: multiplier = 0.75f;
				break;
			case GameManager.Difficulty.Hard: multiplier = 1.25f;
				break;
			case GameManager.Difficulty.Extreme: multiplier = 1.75f;
				break;
			default: multiplier = 1f;
				break;
		}
		damage = (int)(damage * (1 / multiplier));
		base.ReceiveDamage(damage);
	}
}