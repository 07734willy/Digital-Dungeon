using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionTest3 : InspectionTesting {

    public GameTile testTile;

    public override string description {
        get {
            return "Tests consistency of right clicking an object excessively";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        int i;
        for (i = 0; i < 5; i++) {
            RightClickTile(testTile);
        }
        string[] startLog = GetDialogText();
        for (i = 0; i < 2; i++) {
            RightClickTile(testTile);
        }
        status = CompareText(GetDialogText(), startLog) ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}