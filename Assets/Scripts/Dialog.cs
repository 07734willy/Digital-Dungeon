using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour {

    public string message;
    public string inspect;

    public void DisplayDialogMessage() {
        Player player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
        if (HasMessage()) {
            player.SetDialogMessage(message);
        }
    }

    public void DisplayInspection() {
        Player player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
        if (CanInspect()) {
            player.SetDialogMessage(inspect);
        }
    }

    public bool HasMessage() {
        return message != "";
    }

    public bool CanInspect() {
        return inspect != "";
    }
}
