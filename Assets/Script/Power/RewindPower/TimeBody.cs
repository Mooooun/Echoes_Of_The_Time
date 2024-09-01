using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour {

    bool isRewinding = false;

    public float recordTime = 5f;

    [SerializeField]
    public GameObject noirEtblancSysteme;

    List<PointInTime> pointsInTime;

    Rigidbody rb;

    // Use this for initialization
    void Start () {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
        noirEtblancSysteme.SetActive(false);
    }
    
    // Update is called once per frame
    void Update () {
        // Key handling code removed.
    }

    void FixedUpdate ()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Rewind ()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        } else
        {
            StopRewind();
        }
    }

    void Record ()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    public void StartRewind ()
    {
        isRewinding = true;
        rb.isKinematic = true;
        noirEtblancSysteme.SetActive(true);
    }

    public void StopRewind ()
    {
        isRewinding = false;
        rb.isKinematic = false;
        noirEtblancSysteme.SetActive(false);
    }
}
