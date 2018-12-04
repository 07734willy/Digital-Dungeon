using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Trivia : MonoBehaviour {
    public string nextlevel;

    void Start()
    {
        StartCoroutine(loadBar());
    }

    IEnumerator loadBar()
    {
        // System.Threading.Thread.Sleep(5000);

        string trivmessage;

        int n = Random.Range(0, 6);

        switch (n)
        {
            case 0:
                trivmessage = "The Purdue CS department has 9 different tracks a student can pursue after completing the core requirement classes.";
                break;
            case 1:
                trivmessage = "There are 21 core requirement classes one must take in order to qualify for the CS undergraduate degree.";
                break;
            case 2:
                trivmessage = "The Purdue Computer Science tracks are Computational Science and Engineering, Computer Graphics Database and Information Systems, Foundations of Computer Science, Software Engineering, Systems Programming, Machine Intelligence, Programming Language, and Security.";
                break;
            case 3:
                trivmessage = "The core required classes for a Computer Science degree are CS180, CS182, CS240, CS250, CS251, and CS252.";
                break;
            case 4:
                trivmessage = "The Lawson Building doors are open from 7:00AM to 10:30PM";
                break;
            case 5:
                trivmessage = "AMCS (Association of Multicultural Computer Scientists) is dedicated to providing support and facilitate multicultural students' academic, social, and professional development.";
                break;
            default:
                trivmessage = "Purdue's Computer Science Department was the first CS department founded in the country, in 1962.";
                break;
                // will add more trivia
        }

        GameObject.Find("Trivia").GetComponent<Text>().text = trivmessage;
        for (int i = 0; i < 270; i += 5) {
            GameObject.Find("Loading Bar").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(i, 30, 1);
            yield return new WaitForSeconds(0.001F*(Random.Range(0,1)));
        }

        string leveltoload = PlayerPrefs.GetString("levelname", null);

        Debug.Log(PlayerPrefs.GetString("levelname", null));
        Debug.Assert(leveltoload != null);
		
		//GameObject.Find("SaveManager").GetComponent<SaveManager>().SaveData();

        SceneManager.LoadScene(leveltoload);
    }

    public void setNextLevel(string leveltoload)
    {
        nextlevel = leveltoload;
    }
}
