using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {

    public int damage = 0;
    public bool resetting = false;
	protected bool sprung;
    public string achievement = "";
    public string newLevel = "";
    private GameManager curGm;
    public GameTile spawnLocation;
    public GameObject whatToSpawnEasy;
	public GameObject whatToSpawnNormal;
	public GameObject whatToSpawnHard;
	public GameObject whatToSpawnExtreme;
    public string achievementShowcase = "";
    public bool test = false;

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

            if (achievement != "" && this.character is Player){
                if(achievement == "Game Complete"){
                    switch(curGm.difficulty){
                        case GameManager.Difficulty.Extreme:
                        ((Player)character).completeAchievement("Game Complete Extreme");
                        goto case GameManager.Difficulty.Hard;
                        case GameManager.Difficulty.Hard:
                        ((Player)character).completeAchievement("Game Complete Hard");
                        goto case GameManager.Difficulty.Normal;
                        case GameManager.Difficulty.Normal:
                        ((Player)character).completeAchievement("Game Complete Normal");
                        goto case GameManager.Difficulty.Easy;                       
                        case GameManager.Difficulty.Easy:
                        ((Player)character).completeAchievement("Game Complete Easy");
                        break;    
                        default:
                        break;
                    }
                }else{
                    ((Player)character).completeAchievement(achievement);
            }
            }

            if (damage > 0) {
                character.ReceiveDamage(damage);
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

            if(achievementShowcase != "" && this.character is Player){
                ((Player)character).displayAchievement(achievementShowcase);
            }

            if(test){
                //These are test functions to run them easily somewhere, just
                //make sure test is false for real game tiles
                //also try to remember to comment out tests as a double safety
                //((Player)character).wipeAchievements();
            }
        }
        base.SetCharacter(character);
    }
}