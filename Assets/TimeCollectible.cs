using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollectible : MonoBehaviour
{
    /// <summary>
    /// A Basic collectible script that increases the available rewindable time when collected
    /// determined by the timeRefreshAmount
    /// </summary>
    public float timeRefreshAmount;
    [SerializeField]
    private GameObject RoomContainer;
    [SerializeField]
    private ZController zController;
    [SerializeField]

    /// <summary>
    /// Finds all active zControllers
    /// </summary>
    void Start()
    {
        // RoomContainer = GameObject.Find("Moving");
        // zController = RoomContainer.GetComponentInChildren<ZController>();
    }

    /// <summary>
    /// Resets the delta time for each <see cref="ZController"/>s under this controller's control.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(zController.timeAllowance);
            zController.ReduceDeltaTime(timeRefreshAmount);
            
            Destroy(this.gameObject);
        }
    
    }

    /// <summary>
    /// Checks if current <see cref="IRewinder"/> changes
    /// </summary>
    void Update()
    {
       
        // if(!zController.gameObject.activeSelf)
        // {
        //     zController = GameObject.FindObjectOfType<ZController>();
        // }    
    }
}
