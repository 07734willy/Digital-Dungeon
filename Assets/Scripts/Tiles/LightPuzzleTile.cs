using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzleTile : GameTile {

    public DoorTile door;
    public Sprite litSprite;
    private Sprite unlitSprite;
    public List<LightPuzzleTile> toggleLights;
    public bool lit = false;

    protected override void Awake() {
        
        this.unlitSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        if (this.lit) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = litSprite;
        }
        base.Awake();
    }

    protected override void Start() {
        if (!this.lit) {
            door.lightLocks++;
        }
        base.Start();
    }

    public override void SetCharacter(Character character) {
        if (character != null) {
            foreach (LightPuzzleTile tile in toggleLights) {
                tile.ToggleLit();
            }
            this.ToggleLit();
        }
        if (this.door.lightLocks == 0) {
            this.door.OpenDoor();
        }
        base.SetCharacter(character);
    }

    public void ToggleLit() {
        this.lit = !this.lit;
        if (this.lit) {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = this.litSprite;
            this.door.lightLocks--;
        } else {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = this.unlitSprite;
            this.door.lightLocks++;
        }
    }
}
