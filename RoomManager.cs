using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    private Dungeon theDungeon;

    void Start()
    {
        Core.thePlayer = new Player("Julian");
        this.theDungeon = new Dungeon();
        this.setupRoom();
    }

    private void resetRoom()
    {
        for (int i = 0; i < theDoors.Length; i++)
        {
            theDoors[i].SetActive(false);
        }
    }

    private void setupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        if (currentRoom != null)
        {
            theDoors[0].SetActive(currentRoom.hasExit("north"));
            theDoors[1].SetActive(currentRoom.hasExit("south"));
            theDoors[2].SetActive(currentRoom.hasExit("east"));
            theDoors[3].SetActive(currentRoom.hasExit("west"));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMovePlayer("north");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMovePlayer("west");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMovePlayer("east");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMovePlayer("south");
        }
    }

    private void TryMovePlayer(string direction)
    {
        if (Core.thePlayer != null && Core.thePlayer.getCurrentRoom() != null)
        {
            Core.thePlayer.getCurrentRoom().tryToTakeExit(direction);
            setupRoom(); // Update room visuals after movement
        }
    }
}