using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile {
    public static float EASY = 0.75f;
    public static float NORMAL = 1.0f;
    public static float HARD = 1.25f;
    public static float EXTREME = 1.75f;
    public float difficulty = NORMAL;

    public int damage = 0;
    public bool resetting = true;
	private bool sprung;
    public string trapType;

    override protected void Awake() {
        base.Awake();
        this.sprung = false;
        this.damage = (int)(30 * difficulty);
    }

    public override void SetCharacter(Character character) {
        base.SetCharacter(character);

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
        }
    }
}