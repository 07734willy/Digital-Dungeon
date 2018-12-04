using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveLoadTest1a : SaveLoadTesting {
    public GameObject swordPrefab;

    public override string description {
        get {
            return "Tests that a single item in the inventory is saved acrossed levels - pt1 (should never return)";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        ResetPlayer();
        GameObject go = InstantiateFrom(swordPrefab);
        InsertInventoryPlayer(go.GetComponent<Pickup>());
        gameManager.instantLoad("SaveLoad1bTesting");
        // this shouldn't execute
        status = TestManager.Status.failed;
        yield return null;
    }
}
