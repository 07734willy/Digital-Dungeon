using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character {

    public Text dialogText;
    public GameObject dialogBox;

    override protected void Awake() {
        base.Awake();
        this.isPlayer = true;
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();

        if (currentAction.isComplete) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                UseAbility1();
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                UseAbility2();
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                UseAbility3();
            } else {
                Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                // If both controls are held- they "cancel" just like up+down or left+right would.
                if (movement.x != 0 && movement.y != 0) {
                    movement = Vector2.zero;
                }
                if (movement != Vector2.zero) {
                    movement.Normalize();
                    pendingAction = new MovementAction(this, GetCoordinates() + movement, movementSpeed, false);
                }
            }
        }
    }

    public override TurnAction RequestAction() {
        SnapToGrid();
        Debug.Assert(currentAction.isComplete);
        currentAction = pendingAction;
        pendingAction = new NullAction(this);
        return currentAction;
    }


    // Good
    public void UseAbility1() {
        Debug.Log("healing ablity used");
        //SetDialogMessage("dialog text updated");
        this.pendingAction = new HealAbilityAction(this);
    }

    public void UseAbility2() {
        Debug.Log("spin ability used");
        this.pendingAction = new SpinAbilityAction(this);
    }

    public void UseAbility3() {

    }

    // Good
    public void SetDialogMessage(string message) {
        this.dialogText.text = message;
        ToggleDialogBox();
    }

    // Bad (show hide isntead)
    public void ToggleDialogBox() {
        dialogBox.SetActive(!dialogBox.activeSelf);
    }
}
