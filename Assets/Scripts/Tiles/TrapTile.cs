using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {

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

    override protected void Awake() {
        base.Awake();
        this.sprung = false;
        curGm = FindObjectOfType<GameManager>();
    }

    public override void SetCharacter(Character character) {
        Character oldCharacter = this.character;
        base.SetCharacter(character);
        if (!this.sprung && oldCharacter == null) {
            Dialog dialog = this.GetComponent<Dialog>();
            if (dialog != null) {
                dialog.DisplayDialogMessage();
            }

            if (damage > 0) {
                character.ReceiveDamage(damage);
            }

            if (!resetting) {
                this.sprung = true;
            }

            if (newLevel != ""){
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