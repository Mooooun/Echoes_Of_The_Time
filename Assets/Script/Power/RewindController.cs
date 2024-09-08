using UnityEngine;
using System.Collections;

public class RewindController : MonoBehaviour
{
    public TimeBody timeBody; // Reference to the TimeBody script
    public float rewindTime = 5f; // Rewind time control
    public GameObject noirEtblancSysteme; // Noir et blanc system

    private Coroutine rewindCoroutine;

    void Start()
    {
        noirEtblancSysteme.SetActive(false);
        timeBody.SetRewindTime(rewindTime); // Set the rewind time in TimeBody
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        if (rewindCoroutine == null)
        {
            timeBody.StartRewind();
            noirEtblancSysteme.SetActive(true);
            rewindCoroutine = StartCoroutine(RewindDuration());
        }
    }

    public void StopRewind()
    {
        if (rewindCoroutine != null)
        {
            StopCoroutine(rewindCoroutine);
            rewindCoroutine = null;
        }
        timeBody.StopRewind();
        noirEtblancSysteme.SetActive(false);
    }

    private IEnumerator RewindDuration()
    {
        yield return new WaitForSeconds(rewindTime); // Wait for the rewind time
        StopRewind(); // Automatically stop the rewind and noirEtblancSysteme
    }
}
