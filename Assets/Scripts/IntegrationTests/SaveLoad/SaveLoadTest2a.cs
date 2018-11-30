using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveLoadTest2a : SaveLoadTesting {
    public GameObject swordPrefab;

    public override string description {
        get {
            return "Tests that a single item in the equippeditems is saved acrossed levels - pt1 (should never return)";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        ResetPlayer();
        GameObject go = InstantiateFrom(swordPrefab);
        InsertEquippedPlayer(go.GetComponent<Pickup>());
        gameManager.instantLoad("SaveLoad2bTesting");
        // this shouldn't execute
        status = TestManager.Status.failed;
        yield return null;
    }
}
