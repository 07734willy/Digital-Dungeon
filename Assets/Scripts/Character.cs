using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAction {
    public enum ActionType {
        None = 0,
        Animation = 1,
        Waiting = 2,
        Movement = 3,
        Attack = 4,
        Use = 5,
        Interact = 6
    }
    public ActionType action;
    public Vector2 movement;
    public Vector2 coordinates;
    public Vector2 attack;
    // add Use when items are added
    public Vector2 interact;

    public bool animating;
    public float startTime;
    public float duration;
}

abstract public class Character : MonoBehaviour {

    public float inputDelay = 0.2f;
    protected TurnAction lastAction;
    protected TurnAction pendingAction;
    protected GameManager gameManager;

    virtual protected void Awake() {
        lastAction = new TurnAction();
        lastAction.action = TurnAction.ActionType.None;
        lastAction.animating = false;
    }
    
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddCharacter(this);
    }

    public void SetAction(TurnAction action) {
        lastAction = action;
        lastAction.startTime = Time.time;
        switch (lastAction.action) {
            case TurnAction.ActionType.Movement:
                lastAction.duration = 0.4f;
                lastAction.animating = true;
                break;
            default:
                break;
        }
    }

    virtual protected void Update() {
        if (lastAction.animating) {
            switch (lastAction.action) {
                case TurnAction.ActionType.Movement:
                    transform.position = Vector2.Lerp(lastAction.coordinates, lastAction.coordinates + lastAction.movement, (Time.time - lastAction.startTime) / lastAction.duration);
                    break;
                default:
                    break;
            }
            if (lastAction.startTime + lastAction.duration < Time.time) {
                lastAction.animating = false;
            } else if (Time.time - lastAction.startTime < lastAction.duration * (1f - inputDelay)) {
                pendingAction.action = TurnAction.ActionType.Animation;
            }
        }
    }

    public Vector2 GetCoodinates() {
        return (Vector2)transform.position;
        // May need to round off the coordinates to prevent floating-point related errors later. Just in case:
        //return new Vector2(Mathf.round(transform.position.x), Mathf.round(transform.position.y));
    }

    abstract public TurnAction RequestAction();
}
