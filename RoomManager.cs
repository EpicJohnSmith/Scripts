using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    private Dungeon theDungeon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Core.thePlayer = new Player("Julian");
        this.theDungeon = new Dungeon();
        this.setupRoom();
    }

    //disable all doors
    private void resetRoom()
    {
        this.theDoors[0].SetActive(false);
        this.theDoors[1].SetActive(false);
        this.theDoors[2].SetActive(false);
        this.theDoors[3].SetActive(false);
    }

    //show the doors appropriate to the current room
    private void setupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        this.theDoors[0].SetActive(currentRoom.hasExit("north"));
        this.theDoors[1].SetActive(currentRoom.hasExit("south"));
        this.theDoors[2].SetActive(currentRoom.hasExit("east"));
        this.theDoors[3].SetActive(currentRoom.hasExit("west"));
    }

    // Update is called once per frame
    void Update()
    if (Core.thePlayer != null && Core.thePlayer.getCurrentRoom() != null)
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentRoom.tryToTakeExit("north");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentRoom.tryToTakeExit("west");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentRoom.tryToTakeExit("east");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentRoom.tryToTakeExit("south");
        }
    }
}