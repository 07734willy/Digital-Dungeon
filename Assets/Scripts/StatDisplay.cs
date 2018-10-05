using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    private string displayedStat;
    public Text textStat;
    public Character statOrigin;
    public int whichStat; //determines which stat to be displayed

    // Use this for initialization
    void Start()
    {
        textStat = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (whichStat)
        {
            case 1: //health
                displayedStat = "HP: " + statOrigin.health.ToString() + "/" + statOrigin.maxHealth.ToString();
                textStat.text = displayedStat;
                break;
            case 2: //speed
                displayedStat = "SPD: " + statOrigin.movementSpeed.ToString();
                textStat.text = displayedStat;
                break;
            case 3: //evasion
                displayedStat = "EVA: " + statOrigin.evasion.ToString();
                textStat.text = displayedStat;
                break;
            default:
                displayedStat = "Statistics";
                textStat.text = displayedStat;
                break;
        }
    }
}
