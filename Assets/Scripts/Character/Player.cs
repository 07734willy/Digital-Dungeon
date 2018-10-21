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
        this.abilityLevel = new Dictionary<AbilityClass, int>() {
            { AbilityClass.Spin, 1 },
            { AbilityClass.Heal, 1 },
            { AbilityClass.Teleport, 1 }
        };
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        DisplayStats();

        if (currentAction.isComplete) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                UseAbility1();
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                UseAbility2();
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                UseAbility3();
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                UseAbility4();
            } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
                UseAbility5();
            } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
                UseAbility6();
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

    public void DisplayStats() {
        GameObject.Find("UIHealthValue").GetComponent<Text>().text = this.health.ToString();
        GameObject.Find("UIEvasionValue").GetComponent<Text>().text = (this.evasion*100).ToString();
        GameObject.Find("UISpeedValue").GetComponent<Text>().text = this.movementSpeed.ToString();
    }

    public override TurnAction RequestAction() {
        SnapToGrid();
        Debug.Assert(currentAction.isComplete);
        currentAction = pendingAction;
        pendingAction = new NullAction(this);
        if (!(currentAction is NullAction)) {
            HideDialogBox();
        }
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
        Debug.Log("teleport ability used");
        this.pendingAction = new TeleportAbilityAction(this);
    }

    public void UseAbility4() {
        Debug.Log("fury ability used");
        this.pendingAction = new FuryAbilityAction(this);
    }

    public void UseAbility5() {
        Debug.Log("equilibrium ability used");
        this.pendingAction = new EquilibriumAbilityAction(this);
    }

    public void UseAbility6() {
        Debug.Log("push ability used");
        this.pendingAction = new PushAbilityAction(this);
    }

    public void WaitTurn() {
        this.pendingAction = new WaitAction(this);
    }

    // Good
    public void SetDialogMessage(string message) {
        this.dialogText.text = message;
        ShowDialogBox();
    }
    
    public void ShowDialogBox() {
        dialogBox.SetActive(true);
    }
    
    public void HideDialogBox() {
        dialogBox.SetActive(false);
    }
}
