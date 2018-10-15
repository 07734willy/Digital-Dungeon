using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public static float EASY = 0.75f;
    public static float NORMAL = 1.0f;
    public static float HARD = 1.25f;
    public static float EXTREME = 1.75f;
    public float difficulty = NORMAL;
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
    override protected void Awake() {
        base.Awake();
        this.isPlayer = false;
        this.maxHealth = (int)(this.maxHealth * difficulty);
        this.health = this.maxHealth;
        this.evasion = this.evasion / difficulty;
        this.armor = (int)(this.armor * difficulty);
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
	public bool searchList(List<visitedNode> list, Vector2 x){
		for(int i = 0; i < list.Count; i++){
			if(list[i].visit == x){
				return true;
			}
		}
		return false;
	}
	public visitedNode searchListForNode(List<visitedNode> list, Vector2 x){
		visitedNode z = new visitedNode(GetCoordinates(), GetCoordinates());
		for(int i = 0; i < list.Count; i++){
				if(list[i].visit == x){
					return list[i];
				}
		}
		return z;
	}
	
	public Vector2 backTrack(List<visitedNode> list){
		//The player coordinates are the end of the path we want, so find them in the list
		visitedNode x = searchListForNode(list, this.gameManager.GetPlayer().GetCoordinates());
		
		//While the current visitedNode isn't one move away from our starting point, backtrack 
		while(x.visitedBy != GetCoordinates()){
			x = searchListForNode(list, x.visitedBy);
		}
		
		//we are now on the tile that is one move away from the enemy on the shortest path to the player, return it
		return x.visit;
	}
	
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
    public override TurnAction RequestAction() {
        Debug.Assert(currentAction.isComplete);
        if (getDistance() <= 4) {
			setAlertLevel(1);
			if(getDistance() <= 2.0001){
				setAlertLevel(2);
			}
			return new MovementAction(this, shortestPath(), this.movementSpeed, this.instantTurn);
		}
		setAlertLevel(0);
		return getRandomMovement();
    }
	
	public int getDistance(){
		Vector2 playerCoords = this.gameManager.GetPlayer().GetCoordinates();
		Vector2 enemyCoords = GetCoordinates();
        return (int)Mathf.Abs((playerCoords - enemyCoords).magnitude);
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
		return new MovementAction(this, GetCoordinates() + movement, this.movementSpeed, this.instantTurn);
	}
}