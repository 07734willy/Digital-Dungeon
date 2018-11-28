using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IntegrationTest : MonoBehaviour {

    public int timeout = 3;

    public abstract string description {
        get;
    }

    public TestManager.Status status = TestManager.Status.waiting;

    public abstract IEnumerator Run();

}
