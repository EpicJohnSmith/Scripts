using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    public GameObject mmRoomPrefab;
    private Dungeon theDungeon;
    private Dictionary<Room, GameObject> miniMapRooms = new Dictionary<Room, GameObject>();
    private Dictionary<string, Vector3> directionOffsets = new Dictionary<string, Vector3>
    {
        { "north", new Vector3(0, 0, 1.2f) },
        { "south", new Vector3(0, 0, -1.2f) },
        { "east", new Vector3(1.2f, 0, 0) },
        { "west", new Vector3(-1.2f, 0, 0) }
    };

    void Start()
    {
        Core.thePlayer = new Player("Mike");
        this.theDungeon = new Dungeon();
        this.setupRoom();
        CreateMiniMapRoom(Core.thePlayer.getCurrentRoom(), Vector3.zero);
    }

    private void resetRoom()
    {
        foreach (GameObject door in theDoors)
        {
            door.SetActive(false);
        }
    }

    private void setupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        theDoors[0].SetActive(currentRoom.hasExit("north"));
        theDoors[1].SetActive(currentRoom.hasExit("south"));
        theDoors[2].SetActive(currentRoom.hasExit("east"));
        theDoors[3].SetActive(currentRoom.hasExit("west"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) MoveToRoom("north");
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveToRoom("west");
        else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveToRoom("east");
        else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveToRoom("south");
    }

    private void MoveToRoom(string direction)
    {
        Room previousRoom = Core.thePlayer.getCurrentRoom();
        bool didChangeRoom = previousRoom.tryToTakeExit(direction);

        if (didChangeRoom)
        {
            Room newRoom = Core.thePlayer.getCurrentRoom();
            if (!miniMapRooms.ContainsKey(newRoom))
            {
                Vector3 newPos = miniMapRooms[previousRoom].transform.position + directionOffsets[direction];
                CreateMiniMapRoom(newRoom, newPos);
            }
            setupRoom();
        }
    }

    private void CreateMiniMapRoom(Room room, Vector3 position)
    {
        GameObject newMMRoom = Instantiate(mmRoomPrefab, position, Quaternion.identity);
        miniMapRooms[room] = newMMRoom;
    }
}
