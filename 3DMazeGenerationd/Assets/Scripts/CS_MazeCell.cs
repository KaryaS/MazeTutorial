using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Going to create a flat maze by filling a grid stored in a 2D Array
public class CS_MazeCell : MonoBehaviour {

    //Adds a coordinate system/vector to the cell
    public IntVector2 coordinates;
    //This will store how often an edge has been set
    private int iInitialisedEdgeCount;
    //The edges of the cell will be stored in a private array
    //This will hold the four edges of the cells
    private CS_MazeCellEdge[] edges = new CS_MazeCellEdge[MazeDirections.Count];
    //Allows the retrieval of the edge of the cell dependant on the direction input(where the
    //edge is local to the cell
    public CS_MazeCellEdge GetEdge(MazeDirection a_direction)
    {
        return edges[(int)a_direction];
    }
    //Simple function to return if all the edges on a cell have been initialised
    public bool bIsFullyRealised
    {
        get
        {
            return iInitialisedEdgeCount == MazeDirections.Count;
        }
    }

    //Allows us the set the edge of a cell in a direction local the the centre of the cell
    public void SetEdge(MazeDirection a_direction, CS_MazeCellEdge a_edge)
    {
        //Converts the direction enum into an integer to be used to retrieve the corresponding edge
        edges[(int)a_direction] = a_edge;
        //This will allow us to keep track of how often an edge has been set
        iInitialisedEdgeCount += 1;
    }
    //Retrieves an unbiased random uninitialised direction
    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            //Set a skip function
            int skips = Random.Range(0, MazeDirections.Count - iInitialisedEdgeCount);
            //Loop through if a hole is found and skips = 0 then thats the direction chosen
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (MazeDirection)i;
                    }
                    //else decrease the number of skips by one
                    skips -= 1;
                }
            }
            //Fall-back if a mistake is made, we will be able to know by this output in the output log
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }
}
