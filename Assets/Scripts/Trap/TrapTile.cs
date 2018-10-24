using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {

    public int damage = 0;
    public bool resetting = false;
	private bool sprung;
    public string trapType;
    public string newLevel;
    private GameManager curGm;
    public GameTile spawnLocation;
    public GameObject whatToSpawn;

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
            		Instantiate(whatToSpawn, spawnLocation.GetCoordinates(), Quaternion.identity);
            	}
            }
        }
    }
}