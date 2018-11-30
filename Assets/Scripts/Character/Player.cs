using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : Character {

    public Text dialogText;
    public GameObject dialogBox;
    public int fogDistance = 2;
    public AbilityClass selectedAbility = Character.AbilityClass.Heal;
	public string[] logs = new string[5];
    private string[] achievements = {
    "Tutorial", 
    "Level 1", 
    "Level 2", 
    "Level 3", 
    "Level 4", 
    "Level 5",
    "Final Level",
    "Game Complete Easy",
    "Game Complete Normal",
    "Game Complete Hard",
    "Game Complete Extreme",
    "First Weapon",
    "First Purchase",
    "First Enemy Defeated",
    "First Sale"
    };


    private bool loaded = false;
    override protected void Awake() {
        base.Awake();
        this.abilityLevel = new Dictionary<AbilityClass, int>() {
            { AbilityClass.Spin, 1 },
            { AbilityClass.Heal, 1 },
            { AbilityClass.Teleport, 1 }
        };
    }
    // Update is called once per frame
    override protected void Update () {
        if (!loaded) {
            gameManager.GetSaveManager().LoadData();
            loaded = true;
        }

        base.Update();
        DisplayStats();
        DisplayDialog();

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
        
        if(this.IsOnCooldown(selectedAbility)) {
            GameObject.Find("ability_cooldown").GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GameObject.Find("ability_cooldown").GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void DisplayStats() {
		GameObject.Find("AmmunitionValue").GetComponent<Text>().text = this.arrows.ToString();
		GameObject.Find("UIGoldValue").GetComponent<Text>().text =     this.gold.ToString();
		GameObject.Find("UILevel").GetComponent<Text>().text =         this.level.ToString();
        GameObject.Find("UIHealthValue").GetComponent<Text>().text =   this.health.ToString() +"/"+this.maxHealth.ToString();
        GameObject.Find("UIEvasionValue").GetComponent<Text>().text = (this.evasion*100).ToString();
        GameObject.Find("UIArmorValue").GetComponent<Text>().text =    this.armor.ToString();
        GameObject.Find("UIKeyValue").GetComponent<Text>().text =    this.keys.ToString();
    }

    public void DisplayDialog() {
        for (int i = 0; i < 5; i++) {
            GameObject.Find("logText " + i.ToString()).GetComponent<Text>().text = logs[i];
        }
    }

    public override TurnAction RequestAction() {
        SnapToGrid();
        Debug.Assert(currentAction.isComplete);
        currentAction = pendingAction;
        pendingAction = new NullAction(this);
        if (!(currentAction is NullAction)) {
			//Debug.Log(currentAction); 
            //prints the action, but can clutter log
            //that currently has some game information in it
            HideDialogBox();
        }
        return currentAction;
    }


    // Good
    public void UseAbility1() {
        Debug.Log("healing ablity used");
		shiftLogBox();
		logs[0]="healing ability used!";
        //SetDialogMessage("dialog text updated");
        this.pendingAction = new HealAbilityAction(this);
    }

    public void UseAbility2() {
        Debug.Log("spin ability used");
	shiftLogBox();
		logs[0]="spin ability used!";
        this.pendingAction = new SpinAbilityAction(this);
    }

    public void UseAbility3() {
        Debug.Log("teleport ability used");
		shiftLogBox();
		logs[0]="teleport ability used!";
        this.pendingAction = new TeleportAbilityAction(this);
    }

    public void UseAbility4() {
        Debug.Log("fury ability used");
		shiftLogBox();
		logs[0]="fury ability used!";
        this.pendingAction = new FuryAbilityAction(this);
    }

    public void UseAbility5() {
        Debug.Log("equilibrium ability used");
		shiftLogBox();
		logs[0]="Equilibrium ability used!";
        this.pendingAction = new EquilibriumAbilityAction(this);
    }

    public void UseAbility6() {
        Debug.Log("push ability used");
		shiftLogBox();
		logs[0]="Push ability used!";
        this.pendingAction = new PushAbilityAction(this);
    }
	
    // optional method for ability button
    public void UseAbility() {
        switch(selectedAbility)
        {
            case AbilityClass.Heal:
                UseAbility1();
                break;
            case AbilityClass.Spin:
                UseAbility2();
                break;
            case AbilityClass.Teleport:
                UseAbility3();
                break;
            case AbilityClass.Fury:
                UseAbility4();
                break;
            case AbilityClass.Equilibrium:
                UseAbility5();
                break;
            case AbilityClass.Push:
                UseAbility6();
                break;
            default:
                UseAbility1();
                break;
        }
    }

    //Abilities casted to int, ranging from 1-n
    public void SwitchAbility() {
        for(int i = (int)selectedAbility; i < 7; i++)
        {
            if(i != 6 && this.getAbilityLevel((AbilityClass)(i+1)) > 0)
            {
                selectedAbility = (AbilityClass)(i+1);
                break;
            }
            else if(i == 6 && this.getAbilityLevel((AbilityClass)1) > 0)
            {
                selectedAbility = (AbilityClass)1;
                break;
            }
        }

        GameObject.Find("CurrentAbility").GetComponent<Text>().text = (this.selectedAbility).ToString();
    }

    public void WaitTurn() {
        this.pendingAction = new WaitAction(this);
    }

    // Good
    public void SetDialogMessage(string message) {
        //this.dialogText.text = message;
        int index = message.IndexOf("\n");
        string remainder = null;
        if (index != -1 && index < 32) {
            remainder = message.Substring(index + 1);
            message = message.Substring(0, index);
        } else if (message.Length > 32) {
            remainder = message.Substring(32);
            message = message.Substring(0, 32);
        }
		shiftLogBox();
		logs[0] = message;
        DisplayDialog();
        if (remainder != null) {
            SetDialogMessage(remainder);
        }
       // ShowDialogBox();
    }
    public void shiftLogBox(){
		for(int i = 4; i > 0; i--){
			logs[i] = logs[i-1];
			
		}
	}
    public void ShowDialogBox() {
        dialogBox.SetActive(true);
    }
    
    public void HideDialogBox() {
        dialogBox.SetActive(false);
    }
	
	public override void ReceiveDamage (int damage){
		float multiplier = 0f;
		switch (gameManager.difficulty) {
			case GameManager.Difficulty.Easy: multiplier = 0.75f;
				break;
			case GameManager.Difficulty.Hard: multiplier = 1.25f;
				break;
			case GameManager.Difficulty.Extreme: multiplier = 1.75f;
				break;
			default: multiplier = 1f;
				break;
		}
		damage = (int)(damage * multiplier);
		base.ReceiveDamage(damage);
	}

    public void completeAchievement(string cheev){
        Debug.Assert(achievements.Contains(cheev));
        //Debug.Log(PlayerPrefs.GetInt("achievementsEarned", 0));
        if(PlayerPrefs.GetInt(cheev, 0) == 0){
            PlayerPrefs.SetInt(cheev, 1);
            string achievementMessage = string.Format("You just earned the {0} achievement!", cheev);
            SetDialogMessage(achievementMessage);
            PlayerPrefs.SetInt("achievementsEarned",PlayerPrefs.GetInt("achievementsEarned",0) + 1);
            //Debug.Log(PlayerPrefs.GetInt("achievementsEarned", 1));
        }
    }

    public void displayAchievement(string cheev){
        Debug.Assert(achievements.Contains(cheev));
        if(PlayerPrefs.GetInt(cheev, 0) == 0){
                string achievementMessage = string.Format("You have not yet earned the {0} achievement.", cheev);
                SetDialogMessage(achievementMessage);
            }else{
                string achievementMessage = string.Format("Congratulations on earning the {0} achievement!", cheev);
                SetDialogMessage(achievementMessage);
            }
    }

    public void wipeAchievements(){
        foreach (string s in achievements){
            PlayerPrefs.SetInt(s, 0);
        }
        PlayerPrefs.SetInt("achievementsEarned", 0);
    }
}
