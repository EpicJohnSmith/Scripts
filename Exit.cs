using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Exit
{
    private string direction;
    private Room destination;

    public Exit(string direction, Room destination)
    {
        this.direction = direction;
        this.destination = destination;
    }

    // Getter for the direction
    public string getDirection()
    {
        return this.direction;
    }

    // Getter for the destination room
    public Room getDestination()
    {
        return this.destination;
    }
}
