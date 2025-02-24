using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{public GameObject northDoor;
    public GameObject southDoor;
    public GameObject westDoor;
    public GameObject eastDoor;

    void Start()
    {
        UpdateDoors();
    }

    void Update()
    {
        // Continuously check for updates to the doors
        UpdateDoors();
    }

    void UpdateDoors()
    {
        if (northDoor != null) northDoor.SetActive(Core.northDoor);
        if (southDoor != null) southDoor.SetActive(Core.southDoor);
        if (westDoor != null) westDoor.SetActive(Core.westDoor);
        if (eastDoor != null) eastDoor.SetActive(Core.eastDoor);
    }
}