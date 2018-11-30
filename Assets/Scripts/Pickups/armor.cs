using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class armor : Pickup {
    public int armorValue = 10;
    public override void Select() {
        RefreshStatus();
    }

    public override void Update() {
        base.Update();
    }
}