using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        RightClickTile(testTile);
        status = GetDialogText() != startText ? TestManager.Status.passed : TestManager.Status.failed;
        yield return null;
    }
}
