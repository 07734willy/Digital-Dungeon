using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IntegrationTest : MonoBehaviour {

    public int timeout = 3;

    protected GameManager gameManager;
    protected Player player;

    public abstract string description {
        get;
    }

    public void Start() {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.player = GameObject.Find("Player").GetComponent<Player>();
    }

    public TestManager.Status status = TestManager.Status.waiting;

    public abstract IEnumerator Run();

}
