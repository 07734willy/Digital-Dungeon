using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
    public int healLevel = 1;
    public int spinLevel = 1;


    override protected void Awake()
    {
        base.Awake();

        this.abilityLevel = new Dictionary<AbilityClass, int>() {
            { AbilityClass.Spin, spinLevel },
            { AbilityClass.Heal, healLevel }
        };
    }

    public override TurnAction RequestAction()
    {
        Debug.Assert(currentAction.isComplete);
        //Get the overall distance to determine which pathfinding to use
        if (getDistance() <= 4)
        {
            //Set the alert level to "alert"
            setAlertLevel(1);
            if (getDistance() <= 2.0001)
            {
                //Set the alert level to "engaged"
                setAlertLevel(2);
            }
            /* Implement condition for ranged attack here */
            if (this.rangedWeapon != null)
            {
                if (this.checkRangedAttack())
                {
                    if (getDistance() <= this.rangedWeapon.range)
                    {
                        Debug.Log("Ranged attack launched!");
                        return new RangedAttackAction(this, this.gameManager.GetPlayer(), this.movementSpeed, this.instantTurn);
                    }
                }
            }
            /* If not within range for ranged attack, get path movement */
            if (shortestPath().Equals(Vector2.zero) == false)
            {
                switch (gameManager.difficulty)
                {
                    case GameManager.Difficulty.Easy:
                        return new MovementAction(this, easyPath(), this.movementSpeed, this.instantTurn);
                    default:
                        return new MovementAction(this, shortestPath(), this.movementSpeed, this.instantTurn);
                }
            }

            /* If too close for path movement, enemy is within distance of a melee attack */
            if (getDistance() <= 1.0001)
            {
                int n = Random.Range(0, 3);

                switch (n) {
                    case 0:
                        Debug.Log("Enemy used heal ability!");
                        return new HealAbilityAction(this);
                        break;
                    default:
                        Debug.Log("Enemy attacked!");
                        return new MeleeAttackAction(this, this.gameManager.GetPlayer(), this.movementSpeed, this.instantTurn);
                        break;
                }
            }

            /* Just wait */
            return new WaitAction(this);
        }
        //Otherwise, set alert level to "unalert" and move randomly
        setAlertLevel(0);
        //return getRandomMovement();
        return new WaitAction(this);
    }
}
