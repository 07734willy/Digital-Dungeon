using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trivia : MonoBehaviour {

	void Start()
    {
        string trivmessage;

        int n = Random.Range(0, 2);

        switch (n)
        {
            case 0:
                trivmessage = "The Purdue CS department has 9 different tracks a student can pursue after completing the core requirement classes.";
                break;
            case 1:
                trivmessage = "There are 21 core requirement classes one must take in order to qualify for the CS undergraduate degree.";
                break;
            default:
                trivmessage = "Purdue's Computer Science Department was the first CS department founded in the country, in 1962.";
                break;
            // will add more trivia
        }

        GameObject.Find("Trivia").GetComponent<Text>().text = trivmessage;
    }
}
