using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

    private static int boardWidth = 28;
    private static int boardHeight = 36;

    public int totalPellets = 0;
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

            if (o.name != "PacMan" && o.name != "Nodes" && o.name != "NonNodes" && o.name != "Maze" && o.name != "Pellets" && o.tag !="Ghost" && o.tag != "ghostHome")
            {
                if (o.GetComponent<Tile> () != null)
                {
                    if (o.GetComponent<Tile> ().isPellet || o.GetComponent<Tile>().isSuperPellet)
                    {
                        totalPellets++;
                    }                    
                }

                board[(int)pos.x, (int)pos.y] = o;
            }

            if (o.tag == "PacMan")
            {
                Debug.Log("GameBoard found PacMan at: " + pos);
            }

            if (o.name == "Ghost_Blinky")
            {
                Debug.Log("GameBoard found Blinky at: " + pos);
            }

            if (o.name == "Ghost_Pinky")
            {
                Debug.Log("GameBoard found Pinky at: " + pos);
            }

            if (o.name == "Ghost_Inky")
            {
                Debug.Log("GameBoard found Inky at: " + pos);
            }

            if (o.name == "Ghost_Clyde")
            {
                Debug.Log("GameBoard found Clyde at: " + pos);
            }
        }
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
