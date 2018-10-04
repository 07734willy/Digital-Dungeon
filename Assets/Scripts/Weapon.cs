using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Pickup {
    public int damage = 100;
    public float accuracy = 1f;

    public void Awake() {
        this.isWeapon = true;
    }

    public int GetDamageDealt() {
        return Random.Range(0, 1000) < 1000 * accuracy ? this.damage : 0;
    }
}