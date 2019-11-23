using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This code will keep track of the connections between cells
//Here we are going go give each cell their own uni-directional edge as its more felxible
//than if we wer eto create a single bi-directional edge between two cells
//The class is abstract as MazePassage and MazeWall components extern from MazeCellEdge
public abstract class CS_MazeCellEdge : MonoBehaviour {

    public CS_MazeCell cell, otherCell;
    public MazeDirection direction;

    //Initialise method to make the edges children of the cell and place them at the same coordinates
    public void Initialize(CS_MazeCell a_cell, CS_MazeCell a_otherCell, MazeDirection a_direction)
    {
        //Setting the definitions of the local variables to those that have been input into the function
        this.cell = a_cell;
        this.otherCell = a_otherCell;
        this.direction = a_direction;

        a_cell.SetEdge(a_direction, this);
        transform.parent = a_cell.transform;
        transform.localPosition = Vector3.zero;
        //This prevents all the walls from facing north and face their actual direction
        transform.localRotation = direction.ToRotation();
    }


}
