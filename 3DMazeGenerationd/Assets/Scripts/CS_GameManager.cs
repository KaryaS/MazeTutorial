using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will simply begin the game and restart it.
public class CS_GameManager : MonoBehaviour {

    //Add a reference of the prefab so the gamemanager can
    //hold an instance of it
    public CS_Maze mazePrefab;

    private CS_Maze mazeInstance;

    // Use this for initialization
    private void Start()
    {
        BeginGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Restart the game when the space bar is pressed
            RestartGame();
        }
    }

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as CS_Maze;
        //Generates/Creates the maze
        StartCoroutine(mazeInstance.Generate());
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
       }
}
