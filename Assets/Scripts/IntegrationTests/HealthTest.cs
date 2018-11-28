using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : IntegrationTest {
    public override string description {
        get {
            return "health was not decremented";
        }
    }

    public override IEnumerator Run() {
        this.status = TestManager.Status.running;

        Player player = GameObject.Find("Player").GetComponent<Player>();
        int health = player.health;
        player.ReceiveDamage(200);

        if (player.health < health) {
            this.status = TestManager.Status.passed;
        } else {
            this.status = TestManager.Status.failed;
        }
        yield return null;
    }
}
