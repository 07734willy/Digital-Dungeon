using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class SaveLoadTesting : IntegrationTest {

    public void MovePlayer(Vector2 destination) {
        TurnAction action = new MovementAction(player, destination, 3f, true);
        action.consumeTurn = false;
        action.Execute();
        action.Animate();
    }

    public void InsertInventoryPlayer(Pickup pickup) {
        pickup.transform.parent = GameObject.Find("InventoryInven").transform;
        pickup.GetComponent<SpriteRenderer>().enabled = false;
        pickup.SetCharacter(player);
        player.RefreshInventory();
    }

    public void InsertEquippedPlayer(Pickup pickup) {
        pickup.transform.parent = GameObject.Find("EquippedInven").transform;
        pickup.GetComponent<SpriteRenderer>().enabled = false;
        pickup.SetCharacter(player);
        Weapon weapon = pickup.gameObject.GetComponent<Weapon>();
        if (weapon.isRanged) {
            player.SetRangedWeapon(weapon);
        } else {
            player.SetMeleeWeapon(weapon);
        }
        player.RefreshInventory();
    }

    public void ResetPlayer() {
        PlayerPrefs.DeleteAll();
        foreach (Pickup pickup in player.inventory) {
            pickup.transform.parent = null;
            Destroy(pickup.gameObject);
        }
        player.SetMeleeWeapon(null);
        player.SetRangedWeapon(null);
        foreach (Pickup pickup in player.equipped) {
            pickup.transform.parent = null;
            Destroy(pickup.gameObject);
        }
        player.RefreshInventory();
    }
     
    public GameObject InstantiateFrom(GameObject prefab) {
        GameObject go = Instantiate<GameObject>(prefab);
        return PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
    }
}