using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Coodinates {
    private int x1, x2;
    public Coodinates(int x1, int x2) {
        this.x1 = x1;
        this.x2 = x2;
    }
}*/

public class GameManager : MonoBehaviour {

    private Dictionary<Vector2, GameTile> map;

    private void Awake() {
        this.map = new Dictionary<Vector2, GameTile>();
    }

    public void AddTile (GameTile gameTile) {
        Debug.Log("Hello");
        map.Add(gameTile.GetCoodinates(), gameTile);
    }

    public bool requestMovement(Character character, Vector2 movement) {
        if (isTileWalkable(character.GetCoodinates() + movement)) {
            character.transform.Translate(movement);
            return true;
        }
        return false;
    }

    public bool isTileWalkable(Vector2 coordinate) {
        if (!map.ContainsKey(coordinate)) {
            return false;
        }
        return map[coordinate].isWalkable;
    }
}
