using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Pickup {

    public void Awake() {
        this.isConsumable = true;
    }

    public override void Update() {
        base.Update();

        if (IsEquipped()) {
            Select();
        }
    }

    public bool IsEquipped() {
        return transform.parent != null;
    }

}
