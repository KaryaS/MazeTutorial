using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Maze : MonoBehaviour {

    public IntVector2 iSize;
    //Call for the prefab references to allow for instantiation
    public CS_MazeCell cellPrefab;
    private CS_MazeCell[,] cells;
    public CS_MazePassage passagePrefab;
    public CS_MazeWall wallPrefab;
    public float fGenerationStepDelay;

    //Allos us to retrieve a maze's cell at corresponding coordinates
    //This will allow us to check coordinates during Generate, preventing 
    //it from visiting a cell twice
    public CS_MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    //Making Generate a Coroutine allows us to see the order in which the cells are generated over time
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(fGenerationStepDelay);
        //Creating a 2D array and filling the entirety
        //This fills the grid completely with new cells
        cells = new CS_MazeCell[iSize.x, iSize.z];
        //Creates a temporary list to store the activecells (valid cells) in
        List<CS_MazeCell> activeCells = new List<CS_MazeCell>();
        DoFirstGenerationStep(activeCells);
        //While there are still cells that have not been called
        while (activeCells.Count > 0)
        {
            //Adds a delay to allow for us to see the progress of the maze generation
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }

    private void DoFirstGenerationStep(List<CS_MazeCell> activeCells)
    {
        //Initially adds the first cell chosen to the acitve cells list
        //Whilst creating that cell in the world space
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    //This function will also determine if a wall should be created
    private void DoNextGenerationStep(List<CS_MazeCell> activeCells)
    {
        //Checks whether the coordinates are within the grid and havent already been called
        //Therefore checking for valid cells
        int currentIndex = activeCells.Count - 1;
        CS_MazeCell currentCell = activeCells[currentIndex];
        //Tests to see if all cell edges have been initialised as either a wall or a passage
        //before removing it from the activecells list. This will allow the maze to be a 
        //"Perfect" maze in which there will be no unreachable walled-off sections
        if (currentCell.bIsFullyRealised)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        //If the move is possible (the cell is valid) the cell will be created and added to active cells
        if (ContainsCoordinates(coordinates))
        {
            //Creates a temporary cell that stores the next cell to be called
            CS_MazeCell neighbour = GetCell(coordinates);
            //If the cell hasnt been created yet (doesnt exist yet) then it is created
            //and a passage is created between the cells
            if (neighbour == null)
            {
                //Creates the cell
                neighbour = CreateCell(coordinates);
                //Creates the passage between them so its open space
                CreatePassage(currentCell, neighbour, direction);
                //Adds the neighbour to the active cells(cells created)
                activeCells.Add(neighbour);
            }
            //If the neighbour cell has been created or is outside the grid them it wont be created
            //and a wall will be created between them
            else
            {
                //Creates the wall in between the current cell and the neighbouring one
                CreateWall(currentCell, neighbour, direction);
            }
        }
        //otherwise if the cell isnt valid, it will be removed from active cells
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }


    private CS_MazeCell CreateCell(IntVector2 a_iCoord)
    {
        //Instantiate to create a new cell gameobject - This creates a reference to a clone of an object
        CS_MazeCell newCell = Instantiate(cellPrefab) as CS_MazeCell;
        //Place the new cell in the relative spot in the array
        cells[a_iCoord.x, a_iCoord.z] = newCell;
        newCell.coordinates = a_iCoord;
        //We are going to name the Cell according to its coordinates
        //To give order and allow the cells to be easily recognisable during gameplay
        newCell.name = "Maze Cell " + a_iCoord.x + ", " + a_iCoord.z;
        //Making it a child object of the maze, positions it so the grid is centered.
        newCell.transform.parent = transform;
        //Place the new cell in the relative position in the grid.
        newCell.transform.localPosition = new Vector3(a_iCoord.x - iSize.x * 0.5f + 0.5f, 0f, a_iCoord.z - iSize.z * 0.5f + 0.5f);
        return newCell;
    }
    //Produces a random coordinate
    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, iSize.x), Random.Range(0, iSize.z));
        }
    }
    //This will check whether the coordinates are inside the maze(check for valid coordinates)
    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < iSize.x && coordinate.z >= 0 && coordinate.z < iSize.z;
    }
    //This function will instantiate the passage prefab, initialising it.
    private void CreatePassage(CS_MazeCell a_CurCell, CS_MazeCell a_NeighCell, MazeDirection direction)
    {
        //Creates a new passage from the prefab in the prefab folder
        CS_MazePassage passage = Instantiate(passagePrefab) as CS_MazePassage;
        //Initialises the passage created between the current cell and the neighbouring cell in the direction the generation was checking
        passage.Initialize(a_CurCell, a_NeighCell, direction);
        passage = Instantiate(passagePrefab) as CS_MazePassage;
        passage.Initialize(a_NeighCell, a_CurCell, direction.GetOpposite());
    }
    //This function will instantiate the wall prefab, initialising it.
    private void CreateWall(CS_MazeCell a_CurCell, CS_MazeCell a_NeighCell, MazeDirection direction)
    {
        CS_MazeWall wall = Instantiate(wallPrefab) as CS_MazeWall;
        wall.Initialize(a_CurCell, a_NeighCell, direction);
        //As the neighbouring Cell wont be on the edge of the maze
        if (a_NeighCell != null)
        {
            wall = Instantiate(wallPrefab) as CS_MazeWall;
            wall.Initialize(a_NeighCell, a_CurCell, direction.GetOpposite());
        }
    }
}
