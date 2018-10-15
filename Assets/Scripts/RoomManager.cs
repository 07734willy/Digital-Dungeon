using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public Pickup[] requiredPickups;
    public Pickup[] optionalPickups;
    public Character[] requiredCharacters;
    public Character[] optionalCharacters;
    private List<GameTile> RoomTiles;

	void Awake () {
        RoomTiles = new List<GameTile>();
	}

    private void Start() {
        foreach (Transform child in this.transform) {
            GameTile tile = child.GetComponent<GameTile>();
            if (tile != null) {
                RoomTiles.Add(tile);
            }
        }
        SpawnPickups();
    }

    void SpawnCharacters() {

    }

    void SpawnPickups() {
        List<GameTile> valid_tiles = new List<GameTile>(RoomTiles);
        while (valid_tiles.Count > 0) {
            int idx = Random.Range(0, RoomTiles.Count);
            GameTile tile = RoomTiles[idx];
            if (tile.IsWalkable()) {
                Pickup pickup = Instantiate<Pickup>(requiredPickups[0]);
                pickup.transform.position = tile.transform.position;
                tile.AddPickup(pickup);
                break;
            } else {
                valid_tiles.RemoveAt(idx);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
