using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

    private static int boardWidth = 280;
    private static int boardHeight = 360;

    public int totalPellets = 0;
    public int totalPowerPellets = 0;
    public int totalGhostHouseNodes = 0;
    public int score = 0;
    public int pacManLives = 3;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

	// Use this for initialization
	void Start ()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;
            int posX = Mathf.RoundToInt(pos.x * 10);            
            int posY = Mathf.RoundToInt(pos.y * 10);            

            if (o.name != "PacMan" && o.name != "Nodes" && o.name != "NonNodes" && o.name != "Maze" && o.name != "Pellets" && o.tag !="Ghost" && o.tag != "ghostHome")
            {
                if (o.GetComponent<Tile>() != null)
                {
                    if (o.GetComponent<Tile>().isPellet)
                    {
                        totalPellets++;                        
                    }

                    if (o.GetComponent<Tile>().isSuperPellet)
                    {
                        totalPowerPellets++;
                    }

                    if (o.GetComponent<Tile>().isGhostHouse)
                    {
                        totalGhostHouseNodes++;
                    }
                }

                board[posX, posY] = o;
            }
        }

        print("Total Pellets: " + totalPellets);
        print("Total Power Pellets: " + totalPowerPellets);
        print("Total Ghost House Nodes: " + totalGhostHouseNodes);
    }

    public void Restart()
    {
        pacManLives -= 1;

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
