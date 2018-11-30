using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectionTest1 : InspectionTesting {

    public GameTile testTile;

    public override string description {
        get {
            return "Tests that right clicking on an object changes the dialog";
        }
    }

    public override IEnumerator Run() {
        status = TestManager.Status.running;
        string startText = "sample text";
        player.SetDialogMessage(startText);
        string[] startLogs = GetDialogText().Clone() as string[];
        RightClickTile(testTile);
        status = !CompareText(GetDialogText(), startLogs) ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}
