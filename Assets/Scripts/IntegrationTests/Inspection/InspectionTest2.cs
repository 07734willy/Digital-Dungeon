using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionTest2 : InspectionTesting {

    public GameTile testTile;

    public override string description {
        get {
            return "Tests that right clicking on an object updates the dialog with repeated clicks";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        string[] oldLog = GetDialogText().Clone() as string[];
        bool passed = true;
        int i;
        for (i = 0; i < 4; i++) {
            RightClickTile(testTile);
            if (CompareText(oldLog, GetDialogText())) {
                passed = false;
                break;
            }
            oldLog = GetDialogText().Clone() as string[];
        }
        status = passed ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}