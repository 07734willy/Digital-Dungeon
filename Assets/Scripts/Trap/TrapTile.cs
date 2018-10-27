using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {

    public int damage = 0;
    public bool resetting = false;
	private bool sprung;
    public string newLevel = null;
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

        if (!this.sprung && this.character == null) {
            base.SetCharacter(character);

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

            if (newLevel != null){
            	curGm.LoadNewLevel(newLevel);
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