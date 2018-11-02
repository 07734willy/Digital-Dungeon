using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Pickup {

    public bool IsEquipped() {
        return transform.parent != null;
    }

	public override Pickup Clone () {
		Debug.Log("Consumable cloning");
		
		Consumable consume = Instantiate<GameObject>(this.gameObject).GetComponent<Consumable>();

		return consume;
	}
}
