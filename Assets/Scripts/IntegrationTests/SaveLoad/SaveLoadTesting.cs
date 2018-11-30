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
        Transform inv = GameObject.Find("InventoryInven").transform;
        int num_children = inv.childCount;
        int i;
        for (i = 0; i < num_children; i++) {
            if (inv.GetChild(i).name == "invenDummy") {
                DestroyImmediate(inv.GetChild(i).gameObject);
                pickup.transform.SetSiblingIndex(i);
                break;
            }
        }
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
            pickup.transform.SetParent(null);
            Destroy(pickup.gameObject);
        }
        int i;
        for (i = 0; i < 16; i++) {
            GameObject go;
            go = new GameObject();
            go.name = "invenDummy";
            go.AddComponent<Pickup>();
            go.AddComponent<SpriteRenderer>();
            go.transform.parent = GameObject.Find("InventoryInven").transform;
            go.transform.SetSiblingIndex(i);
        }

        player.SetMeleeWeapon(null);
        player.SetRangedWeapon(null);
        foreach (Pickup pickup in player.equipped) {
            pickup.transform.SetParent(null);
            Destroy(pickup.gameObject);
        }
        player.RefreshInventory();
    }
     
    public GameObject InstantiateFrom(GameObject prefab) {
        GameObject go = Instantiate<GameObject>(prefab);
        return PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
    }

    public void PrintInventory() {
        Debug.Log("inventory size: " + player.inventory.Length.ToString());
        foreach (Pickup pickup in player.inventory) {
            Debug.Log(pickup);
        }
    }
}