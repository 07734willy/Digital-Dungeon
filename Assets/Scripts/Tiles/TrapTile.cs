using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {
	public enum TrapDependency{
		None, 
		Health, 
		Level, 
		Gold
	};
	public TrapDependency trapDependency;

    public int damage = 0;
    public bool resetting = false;
	private bool sprung;
    public string newLevel = "";
    private GameManager curGm;
    public GameTile spawnLocation;
    public GameObject whatToSpawnEasy;
	public GameObject whatToSpawnNormal;
	public GameObject whatToSpawnHard;
	public GameObject whatToSpawnExtreme;
	public int minDependency;
	public int maxDependency;
	

    override protected void Awake() {
        base.Awake();
        this.sprung = false;
        curGm = FindObjectOfType<GameManager>();
    }

    public override void SetCharacter(Character character) {
        Character oldCharacter = this.character;
        this.character = character;
        if (!this.sprung && oldCharacter == null) {
            Dialog dialog = this.GetComponent<Dialog>();
            if (dialog != null && this.character is Player) {
                dialog.DisplayDialogMessage();
            }

            switch(trapDependency)
			{
				case TrapDependency.Health:
					if(this.character.GetHealth() < minDependency || this.character.GetHealth() > maxDependency){
						if (damage > 0) {
							character.ReceiveDamage(damage);
						}
					}
					break;
				case TrapDependency.Level:
					if(this.character.GetLevel() < minDependency || this.character.GetLevel() > maxDependency){
						if (damage > 0) {
							character.ReceiveDamage(damage);
						}
					}
					break;
				case TrapDependency.Gold:
					if(this.character.GetGold() < minDependency || this.character.GetGold() > maxDependency){
						if (damage > 0) {
							character.ReceiveDamage(damage);
						}
					}
					break;
				default:
					if (damage > 0) {
						character.ReceiveDamage(damage);
					}
					break;
			}
			
			if (!resetting) {
                this.sprung = true;
            }

            if (newLevel != "" && this.character is Player){
            	curGm.loadNewLevel(newLevel);
            }

            if(spawnLocation != null){
            	if(spawnLocation.GetCharacter() == null && spawnLocation.IsWalkable()){
            		switch (curGm.difficulty)
					{
						case GameManager.Difficulty.Easy:
							Instantiate(whatToSpawnEasy, spawnLocation.GetCoordinates(), Quaternion.identity);
							break;
						case GameManager.Difficulty.Hard: 
							Instantiate(whatToSpawnHard, spawnLocation.GetCoordinates(), Quaternion.identity);
							break;
						case GameManager.Difficulty.Extreme: 
							Instantiate(whatToSpawnExtreme, spawnLocation.GetCoordinates(), Quaternion.identity);
							break;
						default: 
							Instantiate(whatToSpawnNormal, spawnLocation.GetCoordinates(), Quaternion.identity);
							break;
					}
            	}
            }
        }
    }
}