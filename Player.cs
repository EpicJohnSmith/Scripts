using UnityEngine;

public class Player : Inhabitant
{

    private Room currentRoom;

    public Player(string name) : base(name)
    {
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