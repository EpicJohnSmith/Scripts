using UnityEngine;

public class Player
{
    private string name;
    private Room currentRoom;

    public Player(string name)
    {
        this.name = name;
        this.currentRoom = null;
    }

    public Room getCurrentRoom()
    {
        return this.currentRoom;
    }
    
    public void setCurrentRoom(Room r)
    {
        this.currentRoom = r;
    }
    
}