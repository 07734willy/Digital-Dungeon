<<<<<<< HEAD:Assets/Scripts/Pickups/Weapon.cs
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

    public override void Select() {
        RefreshStatus();
        if (IsEquipped() && this.character != null) {
            character.SetWeapon(this);
        }
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
=======
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

    public override void Use() {
        if (IsEquipped()) {
            transform.parent.GetComponent<Character>().SetWeapon(this);
        }
    }

    public override void Update() {
        base.Update();

        if (IsEquipped()) {
            Use();
        }
    }

    public bool IsEquipped() {
        return transform.parent != null;
    }
>>>>>>> dialogue:Assets/Scripts/Weapon.cs
}