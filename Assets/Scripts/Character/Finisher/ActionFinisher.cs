using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActionFinisher {
    public int finishTurn;

    abstract public bool Execute();
}