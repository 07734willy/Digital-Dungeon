using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionTest4 : InspectionTesting {

    public GameTile testTile1;
    public GameTile testTile2;

    public override string description {
        get {
            return "Tests that right clicking on an different objects produces distinct dialog messages";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        int i;
        for (i = 0; i < 5; i++) {
            RightClickTile(testTile1);
        }
        string[] startLog = GetDialogText().Clone() as string[];
        //PrintText(GetDialogText());
        //PrintText(startLog);
        RightClickTile(testTile2);
        status = !CompareText(GetDialogText(), startLog) ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}