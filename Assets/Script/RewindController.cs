using UnityEngine;

public class RewindController : MonoBehaviour {

    public TimeBody timeBody; // Reference to the TimeBody script

    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
            timeBody.StartRewind();

        if (Input.GetKeyUp(KeyCode.Return))
            timeBody.StopRewind();
    }
}
