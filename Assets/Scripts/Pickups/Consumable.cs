using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Pickup {

    public bool IsEquipped() {
        return transform.parent != null;
    }

}
