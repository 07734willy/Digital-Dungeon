using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour {

    public string message;

    public void DisplayDialogMessage() {
        Player player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
        player.SetDialogMessage(message);
    }
}
