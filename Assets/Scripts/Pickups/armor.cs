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
	
	public override Pickup Clone () {
		armor armorClone = Instantiate<GameObject>(this.gameObject).GetComponent<armor>();

		return armorClone;
	}
	
	public override string GetStats () {
		string message = "Cost: " + this.cost + "  \tLevel Required: " + this.GetBaseLevel() +  "\nSell: " + (int)(this.cost*0.8) + "  \tArmor: " + this.armorValue;
		return message;
	}
}