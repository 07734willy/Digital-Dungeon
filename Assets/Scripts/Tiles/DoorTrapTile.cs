using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrapTile : TrapTile {

    public bool openDoor;
    public GameObject doorTile;

    public override void SetCharacter(Character character) {
        Debug.Assert(doorTile != null);
        DoorTile door = doorTile.GetComponent<DoorTile>();
        if (!this.sprung) {
            if (openDoor) {
                door.OpenDoor();
            } else {
                door.CloseDoor();
            }
        }
        base.SetCharacter(character);
    }
}