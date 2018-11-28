using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public Pickup[] requiredPickups;
    public Pickup[] optionalPickups;
    public int minOptionalPickups;
    public int maxOptionalPickups;
    public Character[] requiredCharacters;
    public Character[] optionalCharacters;
    public int minOptionalCharacters;
    public int maxOptionalCharacters;
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
        spawnElements();
        Debug.Assert(minOptionalPickups <= maxOptionalPickups && maxOptionalPickups <= optionalPickups.Length);
        Debug.Assert(minOptionalCharacters <= maxOptionalCharacters && maxOptionalCharacters <= optionalCharacters.Length);
    }


    /* This spawns all required Pickups / Characters, and somewhere between the min and max amount of the optional ones
     * - this will not spawn multiple instances of the same pickup / character. To have multiple- add them into the
     * initial array multiple times. Also, the  min <= max <= length_of_array, otherwise an assertion is failed */
    void spawnElements() {
        Debug.Assert(minOptionalPickups <= maxOptionalPickups && maxOptionalPickups <= optionalPickups.Length);
        Debug.Assert(minOptionalCharacters <= maxOptionalCharacters && maxOptionalCharacters <= optionalCharacters.Length);
        foreach (Pickup pickup in requiredPickups) {
            SpawnPickup(pickup);
        }
        List<Pickup> valid_pickups = new List<Pickup>(optionalPickups);
        int i;
        for (i = Random.Range(minOptionalPickups, maxOptionalPickups + 1); i > 0; i--) {
            int idx = Random.Range(0, valid_pickups.Count);
            SpawnPickup(valid_pickups[idx]);
            valid_pickups.RemoveAt(idx);
        }

        foreach (Character character in requiredCharacters) {
            SpawnCharacter(character);
        }
        List<Character> valid_characters = new List<Character>(optionalCharacters);
        for (i = Random.Range(minOptionalCharacters, maxOptionalCharacters + 1); i > 0; i--) {
            int idx = Random.Range(0, valid_characters.Count);
            SpawnCharacter(valid_characters[idx]);
            valid_characters.RemoveAt(idx);
        }
    }

    void SpawnCharacter(Character character) {
        List<GameTile> valid_tiles = new List<GameTile>(RoomTiles);
        while (valid_tiles.Count > 0) {
            int idx = Random.Range(0, valid_tiles.Count);
            GameTile tile = RoomTiles[idx];
            if (tile.IsWalkable()) {
                Character instance = Instantiate<Character>(character);
                instance.transform.position = tile.transform.position;
				Debug.Log("RoomManagerSetCharacter");
                tile.SetCharacter(instance);
                break;
            } else {
                valid_tiles.RemoveAt(idx);
            }
        }
        Debug.Assert(valid_tiles.Count > 0);
    }

    void SpawnPickup(Pickup pickup) {
        List<GameTile> valid_tiles = new List<GameTile>(RoomTiles);
        while (valid_tiles.Count > 0) {
            int idx = Random.Range(0, valid_tiles.Count);
            GameTile tile = RoomTiles[idx];
            if (tile.IsWalkable() || tile.GetCharacter() != null) {
                Pickup instance = Instantiate<Pickup>(pickup);
                instance.transform.position = tile.transform.position;
                tile.AddPickup(instance);
                break;
            } else {
                valid_tiles.RemoveAt(idx);
            }
        }
        Debug.Assert(valid_tiles.Count > 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
