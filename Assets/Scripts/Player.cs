using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Update is called once per frame
	void Update () {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // If both controls are held- they "cancel" just like up+down or left+right would.
        if (movement.x != 0 && movement.y != 0) {
            movement = Vector2.zero;
        }
        if (movement != Vector2.zero) {
            movement.Normalize();
        }

        //Vector2 moveDirection = new Vector2(xmove, ymove);
        Debug.Log(gameManager.requestMovement(this, movement));
        //moveDirection = transform.TransformDirection(moveDirection);
    }

}
