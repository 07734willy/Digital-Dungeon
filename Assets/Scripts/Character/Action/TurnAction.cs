using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TurnAction {
    public bool isComplete;
    public float startTime;
    public float duration;
    public bool consumeTurn = true;
    protected Character character;
    protected GameManager gameManager;

    abstract public bool Check();

    abstract public void Animate();

    abstract public bool Execute();
}