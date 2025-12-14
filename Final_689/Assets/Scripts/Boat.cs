using UnityEngine;
using UnityEngine.Apple;

public class Boat
{
    public string Destination;
    public int Amount;
    public string Resource;
    public int TurnsLeft;
    
    public Boat(string destination, string resource, int turnsLeft, int amount = 10)
    {
       Destination = destination;
       Resource = resource;
       TurnsLeft = turnsLeft;
       Amount = amount;
    }
}
