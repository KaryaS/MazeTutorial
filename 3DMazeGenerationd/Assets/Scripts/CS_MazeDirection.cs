using UnityEngine;

//Enum to explicitly define the possible directions the maze could go
public enum MazeDirection
{
    North,
    East,
    South,
    West
}

public static class MazeDirections
{
    //Official way of storing how many directions there are
    //Prevent magic numbers
    public const int Count = 4;
    //Simple code to return a random value between 1 and 4 every time it is called
    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }
    //Set up a Vector system relative to the direction
    private static IntVector2[] vectors = {
        new IntVector2(0, 1),   //North
        new IntVector2(1, 0),   //East
        new IntVector2(0, -1),  //South
        new IntVector2(-1, 0)   //West
    };
    //Convert a direction into a vector2 by returning the stored vector corresponding to the enum value of the direction
    //By adding "this" it forms an extension method, so the function will behave as it it were an instance method of MazeDirections
    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }
    //The exact opposite of the directions in the same index in MazeDirection
    private static MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };
    //A simple function to retrieve the corresponding opposite of the direction input
    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
    }
    //Initially the walls are all facing north
    //This Quaternion array will store the corresponding rotations dependent on the direction
    //Therefore they are laid out in the same order as the direction enumeration
    private static Quaternion[] rotations = {
        Quaternion.identity,            //North
        Quaternion.Euler(0f, 90f, 0f),  //East
        Quaternion.Euler(0f, 180f, 0f), //South
        Quaternion.Euler(0f, 270f, 0f)  //West
    };
    //A Simple function to allow for the retrieval of the rotation quaternion dependant on the directional input
    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }
}