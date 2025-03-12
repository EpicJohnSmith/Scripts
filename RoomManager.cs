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
}
    // Update is called once per frame
    // This should be in a MonoBehaviour script attached to your Player GameObject
void Update()
{
    if(Input.GetKeyDown(KeyCode.UpArrow))
    {
        TryMovePlayer("north");
    }
    else if(Input.GetKeyDown(KeyCode.LeftArrow))
    {
        TryMovePlayer("west");
    }
    else if(Input.GetKeyDown(KeyCode.RightArrow))
    {
        TryMovePlayer("east");
    }
    else if(Input.GetKeyDown(KeyCode.DownArrow))
    {
        TryMovePlayer("south");
    }
}

private void TryMovePlayer(string direction)
{
    if (this.thePlayer != null && this.thePlayer.GetCurrentRoom() != null)
    {
        this.thePlayer.GetCurrentRoom().tryToTakeExit(direction);
    }
}