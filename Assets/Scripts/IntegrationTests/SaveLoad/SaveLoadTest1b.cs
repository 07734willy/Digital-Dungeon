using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveLoadTest1b : SaveLoadTesting {
    public GameObject swordPrefab;

    public override string description {
        get {
            return "Tests that a single item in the inventory is saved acrossed levels";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        bool passed = (player.inventory.Length == 1) && (((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(player.inventory[0].gameObject)) == swordPrefab);
        status = passed ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}
