﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameBoard : MonoBehaviour {

    private static int boardWidth = 280;
    private static int boardHeight = 360;

<<<<<<< HEAD
    public int totalPellets = 0;
    public int totalPowerPellets = 0;
    public int totalGhostHouseNodes = 0;
=======
    private bool didStartDeath = false;
    private bool didStartConsumed = false;

    public static int playerOneLevel = 1;
    public static int playerTwoLevel = 1;    

    public int totalPellets = 0;    
>>>>>>> master
    public int score = 0;
    public static int playerOneScore = 0;
    public static int playerTwoScore = 0;

    public static int ghostConsumedRunningScore;

    public static bool isPlayerOneUp = true;
    public bool shouldBlink = false;

    public float blinkIntervalTime = 0.1f;
    private float blinkIntervalTimer = 0;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;
    public AudioClip backgroundAudioPacManDeath;
    public AudioClip consumedGhostAudioClip;

    public Sprite mazeBlue;
    public Sprite mazeWhite;

    public Text playerText;
    public Text readyText;

    public Text highScoreText;
    public Text playerOneUp;
    public Text playerTwoUp;
    public Text playerOneScoreText;
    public Text playerTwoScoreText;
    public Image playerLives2;
    public Image playerLives3;

    public Text consumedGhostScoreText;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    public Image[] levelImages;

    private bool didIncrementLevel = false;

    bool didSpawnBonusItem1_player1;
    bool didSpawnBonusItem2_player1;
    bool didSpawnBonusItem1_player2;
    bool didSpawnBonusItem2_player2;

    // Use this for initialization
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;
            int posX = Mathf.RoundToInt(pos.x * 10);            
            int posY = Mathf.RoundToInt(pos.y * 10);            

            if (o.name != "PacMan" && o.name != "Nodes" && o.name != "NonNodes" && o.name != "Maze" && o.name != "Pellets" && o.tag != "Ghost" && o.tag != "ghostHome" && o.name != "Canvas" && o.tag != "UIElements")
            {
                if (o.GetComponent<Tile>() != null)
                {
<<<<<<< HEAD
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
=======
                    if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSuperPellet)
                    {
                        totalPellets++;
                    }
                }

                board[(int)pos.x, (int)pos.y] = o;
            }            
        }

        if (isPlayerOneUp)
        {
            if (playerOneLevel == 1)
            {
                GetComponent<AudioSource>().Play();
            }
        }

        else
        {
            if (playerTwoLevel == 1)
            {
                GetComponent<AudioSource>().Play();
            }
        }

        StartGame();
    }

    void Update()
    {
        UpdateUI();

        CheckPelletsConsumed();

        CheckShouldBlink();

        BonusItems();
    }

    void BonusItems()
    {
        if (GameMenu.isOnePlayerGame)
        {
            SpawnBonusItemForPlayer(1);
        }

        else
        {
            if (isPlayerOneUp)
            {
                SpawnBonusItemForPlayer(1);
            }

            else
            {
                SpawnBonusItemForPlayer(2);
            }
        }
    }

    void SpawnBonusItemForPlayer (int playernum)
    {
        if (playernum == 1)
        {
            if (GameMenu.playerOnePelletsConsumed >= 70 && GameMenu.playerOnePelletsConsumed < 170)
            {
                if (!didSpawnBonusItem1_player1)
                {
                    didSpawnBonusItem1_player1 = true;
                    SpawnBonusItemForLevel(playerOneLevel);
                }                
            }

            else if (GameMenu.playerOnePelletsConsumed >= 170)
            {
                if (!didSpawnBonusItem2_player1)
                {
                    didSpawnBonusItem2_player1 = true;
                    SpawnBonusItemForLevel(playerOneLevel);
                }
            }
        }

        else
        {
            if (GameMenu.playerTwoPelletsConsumed >= 70 && GameMenu.playerTwoPelletsConsumed < 170)
            {
                if (!didSpawnBonusItem1_player2)
                {
                    didSpawnBonusItem1_player2 = true;
                    SpawnBonusItemForLevel(playerTwoLevel);
                }
            }

            else if (GameMenu.playerTwoPelletsConsumed >= 170)
            {
                if (!didSpawnBonusItem2_player2)
                {
                    didSpawnBonusItem2_player2 = true;
                    SpawnBonusItemForLevel(playerTwoLevel);
                }
            }
        }
    }

    void SpawnBonusItemForLevel (int level)
    {
        GameObject bonusitem = null;

        if (level == 1)
        {
            bonusitem = Resources.Load("Prefabs/cherry", typeof(GameObject)) as GameObject;
        }

        else if (level == 2)
        {
            bonusitem = Resources.Load("Prefabs/strawberry", typeof(GameObject)) as GameObject;
        }

        else if (level == 3)
        {
            bonusitem = Resources.Load("Prefabs/peach", typeof(GameObject)) as GameObject;
        }

        else if (level == 4)
        {
            bonusitem = Resources.Load("Prefabs/peach", typeof(GameObject)) as GameObject;
        }

        else if (level == 5)
        {
            bonusitem = Resources.Load("Prefabs/apple", typeof(GameObject)) as GameObject;
        }

        else if (level == 6)
        {
            bonusitem = Resources.Load("Prefabs/apple", typeof(GameObject)) as GameObject;
        }

        else if (level == 7)
        {
            bonusitem = Resources.Load("Prefabs/grapes", typeof(GameObject)) as GameObject;
        }

        else if (level == 8)
        {
            bonusitem = Resources.Load("Prefabs/grapes", typeof(GameObject)) as GameObject;
        }

        else if (level == 9)
        {
            bonusitem = Resources.Load("Prefabs/galaxian", typeof(GameObject)) as GameObject;
        }

        else if (level == 10)
        {
            bonusitem = Resources.Load("Prefabs/galaxian", typeof(GameObject)) as GameObject;
        }

        else if (level == 11)
        {
            bonusitem = Resources.Load("Prefabs/bell", typeof(GameObject)) as GameObject;
        }

        else if (level == 12)
        {
            bonusitem = Resources.Load("Prefabs/bell", typeof(GameObject)) as GameObject;
        }

        else 
        {
            bonusitem = Resources.Load("Prefabs/key", typeof(GameObject)) as GameObject;
        }

        Instantiate(bonusitem);
    }

    void UpdateUI()
    {
        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();

        int currentLevel;

        if (isPlayerOneUp)
        {
            currentLevel = playerOneLevel;

            if (GameMenu.livesPlayerOne == 3)
            {
                playerLives3.enabled = true;
                playerLives2.enabled = true;
            }

            else if (GameMenu.livesPlayerOne == 2)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = true;
            }

            else if (GameMenu.livesPlayerOne == 1)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = false;
            }
        }

        else
        {
            currentLevel = playerTwoLevel;

            if (GameMenu.livesPlayerTwo == 3)
            {
                playerLives3.enabled = true;
                playerLives2.enabled = true;
            }

            else if (GameMenu.livesPlayerTwo == 2)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = true;
            }

            else if (GameMenu.livesPlayerTwo == 1)
            {
                playerLives3.enabled = false;
                playerLives2.enabled = false;
            }
        }

        for (int i =0; i < levelImages.Length; i++)
        {
            Image li = levelImages[i];
            li.enabled = false;
        }

        for (int i = 1; i < levelImages.Length +1; i++)
        {
            if (currentLevel >= i)
            {
                Image li = levelImages[i - 1];
                li.enabled = true;
            }
        }
    }

    void CheckPelletsConsumed ()
    {
        if (isPlayerOneUp)
        {
            //- Player one is playing
            if (totalPellets == GameMenu.playerOnePelletsConsumed)
            {
                PlayerWin(1);
            }
        }

        else
        {
            //- Player two is playing
            if (totalPellets == GameMenu.playerTwoPelletsConsumed)
            {
                PlayerWin(2);
            }
        }
    }

    void PlayerWin (int playerNum)
    {
        if (playerNum == 1)
        {
            if (!didIncrementLevel)
            {
                didIncrementLevel = true;
                playerOneLevel++;                
                StartCoroutine(ProcessWin(2));
            }
        }

        else
        {
            if (!didIncrementLevel)
            {
                didIncrementLevel = true;
                playerTwoLevel++;
                StartCoroutine(ProcessWin(2));
            }            
        }        
    }

    IEnumerator ProcessWin (float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().canMove = false;
        pacMan.transform.GetComponent<Animator>().enabled = false;

        transform.GetComponent<AudioSource>().Stop();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().canMove = false;
            ghost.transform.GetComponent<Animator>().enabled = false;
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(BlinkBoard(2));
        
    }

    IEnumerator BlinkBoard (float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        //- Blink Board
        shouldBlink = true;

        yield return new WaitForSeconds(delay);

        //- Restart the game at the next level
        shouldBlink = false;
        StartNextLevel();
    }

    private void StartNextLevel()
    {
        StopAllCoroutines();
        if (isPlayerOneUp)
        {
            ResetPelletsForPlayer(1);
            GameMenu.playerOnePelletsConsumed = 0;
            GameMenu.ghostReleasePelletCounterP1 = 0;
            didSpawnBonusItem1_player1 = false;
            didSpawnBonusItem2_player1 = false;
        }

        else
        {
            ResetPelletsForPlayer(2);
            GameMenu.playerTwoPelletsConsumed = 0;
            GameMenu.ghostReleasePelletCounterP2 = 0;
            didSpawnBonusItem1_player2 = false;
            didSpawnBonusItem2_player2 = false;
        }

        GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeBlue;

        didIncrementLevel = false;

        StartCoroutine(ProcessStartNextLevel(1));
    }

    IEnumerator ProcessStartNextLevel (float delay)
    {
        playerText.transform.GetComponent<Text>().enabled = true;
        readyText.transform.GetComponent<Text>().enabled = true;

        if (isPlayerOneUp)
            StartCoroutine(StartBlinking(playerOneUp));
        else
            StartCoroutine(StartBlinking(playerTwoUp));

        RedrawBoard();

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestartShowObjects(1));        
    }

    private void CheckShouldBlink()
    {
        if (shouldBlink)
        {
            if (blinkIntervalTimer < blinkIntervalTime)
            {
                blinkIntervalTimer += Time.deltaTime;
            }

            else
            {
                blinkIntervalTimer = 0;

                if (GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite == mazeBlue)
                {
                    GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeWhite;
                }

                else
                {
                    GameObject.Find("Maze").transform.GetComponent<SpriteRenderer>().sprite = mazeBlue;
                }
            }
        }
    }

    public void StartGame()
    {
        if (GameMenu.isOnePlayerGame)
        {
            playerTwoUp.GetComponent<Text>().enabled = false;
            playerTwoScoreText.GetComponent<Text>().enabled = false;
        }

        else
        {
            playerTwoUp.GetComponent<Text>().enabled = true;
            playerTwoScoreText.GetComponent<Text>().enabled = true;
        }

        if (isPlayerOneUp)
        {
            StartCoroutine(StartBlinking(playerOneUp));
        }

        else
        {
            StartCoroutine(StartBlinking(playerTwoUp));
        }

        //- Hide All Ghosts

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
            ghost.transform.GetComponent<Ghost>().canMove = false;
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;
        pacMan.transform.GetComponent<Animator>().enabled = false;
        pacMan.transform.GetComponent<PacMan>().canMove = false;

        StartCoroutine(ShowObjectsAfter(2.25f));
    }

    public void StartConsumed (Ghost consumedGhost)
    {
        if (!didStartConsumed)
        {
            didStartConsumed = true;

            //- Pause all the ghosts
            GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

            foreach (GameObject ghost in o)
            {
                ghost.transform.GetComponent<Ghost>().canMove = false;
            }

            //- Pause Pac-Man
            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove = false;

            //- Hide Pac-Man
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            //- Hide the consumed ghost
            consumedGhost.transform.GetComponent<SpriteRenderer>().enabled = false;

            //- Stop background music
            transform.GetComponent<AudioSource>().Stop();

            //- Display score at ghost location
            Vector2 pos = consumedGhost.transform.position;
            Debug.Log(pos);

            Vector2 viewPortPoint = Camera.main.WorldToViewportPoint(pos);
            Debug.Log(viewPortPoint);

            consumedGhostScoreText.GetComponent<RectTransform>().anchorMin = viewPortPoint;
            consumedGhostScoreText.GetComponent<RectTransform>().anchorMax = viewPortPoint;

            consumedGhostScoreText.text = ghostConsumedRunningScore.ToString();

            consumedGhostScoreText.GetComponent<Text>().enabled = true;

            //- Play the consumed sound
            transform.GetComponent<AudioSource>().PlayOneShot(consumedGhostAudioClip);

            //- Wait for the audio clip to finish
            StartCoroutine(ProcessConsumedAfter(0.75f, consumedGhost));
        }
    }

    public void StartConsumedBonusItem (GameObject bonusItem, int scorevalue)
    {
        Vector2 pos = bonusItem.transform.position;
        Vector2 viewPortPoint = Camera.main.WorldToViewportPoint(pos);

        consumedGhostScoreText.GetComponent<RectTransform>().anchorMin = viewPortPoint;
        consumedGhostScoreText.GetComponent<RectTransform>().anchorMax = viewPortPoint;

        consumedGhostScoreText.text = scorevalue.ToString();

        consumedGhostScoreText.GetComponent<Text>().enabled = true;

        Destroy(bonusItem.gameObject);

        StartCoroutine(ProcessConsumedBonusItem(0.75f));
    }

    IEnumerator ProcessConsumedBonusItem (float delay)
    {
        yield return new WaitForSeconds(delay);

        consumedGhostScoreText.GetComponent<Text>().enabled = false;
    }

    IEnumerator StartBlinking (Text blinkText)
    {
        yield return new WaitForSeconds(0.25f);

        blinkText.GetComponent<Text>().enabled = !blinkText.GetComponent<Text>().enabled;
        StartCoroutine(StartBlinking(blinkText));
    }

    IEnumerator ProcessConsumedAfter (float delay, Ghost consumedGhost)
    {
        yield return new WaitForSeconds(delay);

        //- Hide the score
        consumedGhostScoreText.GetComponent<Text>().enabled = false;

        //- Show Pac-Man
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;

        //- Show Consumed Ghost
        consumedGhost.transform.GetComponent<SpriteRenderer>().enabled = true;

        //- Resume all ghosts
        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().canMove = true;
        }

        //- Resume Pac-Man
        pacMan.transform.GetComponent<PacMan>().canMove = true;

        //- Start Background Music
        transform.GetComponent<AudioSource>().Play();

        didStartConsumed = false;
    }

    IEnumerator ShowObjectsAfter (float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;            
        }

        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<SpriteRenderer>().sprite = pacMan.transform.GetComponent<PacMan>().idleSprite;
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;

        playerText.transform.GetComponent<Text>().enabled = false;

        StartCoroutine(StartGameAfter(2));
    }

    IEnumerator StartGameAfter (float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {            
            ghost.transform.GetComponent<Ghost>().canMove = true;
        }

        GameObject pacMan = GameObject.Find("PacMan");        
        pacMan.transform.GetComponent<PacMan>().canMove = true;

        readyText.transform.GetComponent<Text>().enabled = false;

        transform.GetComponent<AudioSource>().clip = backgroundAudioNormal;
        transform.GetComponent<AudioSource>().Play();
    }

    public void StartDeath()
    {
        if (!didStartDeath)
        {
            StopAllCoroutines();

            if (GameMenu.isOnePlayerGame)
            {
                playerOneUp.GetComponent<Text>().enabled = true;
            }

            else
            {
                playerOneUp.GetComponent<Text>().enabled = true;
                playerTwoUp.GetComponent<Text>().enabled = true;
            }

            GameObject bonusItem = GameObject.Find("bonusItem");

            if (bonusItem)
                Destroy(bonusItem.gameObject);

            didStartDeath = true;

            GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

            foreach (GameObject ghost in o)
            {
                ghost.transform.GetComponent<Ghost>().canMove = false;
            }

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<PacMan>().canMove = false;

            pacMan.transform.GetComponent<Animator>().enabled = false;

            transform.GetComponent<AudioSource> ().Stop();

            StartCoroutine(ProcessDeathAfter(2));
        }
    }

    IEnumerator ProcessDeathAfter (float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

        StartCoroutine(ProcessDeathAnimation(1.9f));
    }

    IEnumerator ProcessDeathAnimation (float delay)
    {
        GameObject pacMan = GameObject.Find("PacMan");

        pacMan.transform.localScale = new Vector3(1, 1, 1);
        pacMan.transform.localRotation = Quaternion.Euler (0, 0, 0);

        pacMan.transform.GetComponent<Animator>().runtimeAnimatorController = pacMan.transform.GetComponent<PacMan>().deathAnimation;
        pacMan.transform.GetComponent<Animator>().enabled = true;

        transform.GetComponent<AudioSource>().clip = backgroundAudioPacManDeath;
        transform.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(delay);

        StartCoroutine(ProcessRestart(1));
    }

    IEnumerator ProcessRestart (float delay)
    {
        if (isPlayerOneUp)
            GameMenu.livesPlayerOne -= 1;
        else
            GameMenu.livesPlayerTwo -= 1;

        if (GameMenu.livesPlayerOne == 0 && GameMenu.livesPlayerTwo == 0)
        {
            playerText.transform.GetComponent<Text>().enabled = true;

            readyText.transform.GetComponent<Text>().text = "GAME OVER";
            readyText.transform.GetComponent<Text>().color = Color.red;

            readyText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            StartCoroutine(ProcessGameOver(2));
        }

        else if (GameMenu.livesPlayerOne == 0 || GameMenu.livesPlayerTwo == 0 && !GameMenu.isOnePlayerGame)
        {
            if (GameMenu.livesPlayerOne == 0)
            {
                playerText.transform.GetComponent<Text>().text = "PLAYER ONE";
            }

            else if (GameMenu.livesPlayerTwo == 0)
            {
                playerText.transform.GetComponent<Text>().text = "PLAYER TWO";
            }

            readyText.transform.GetComponent<Text>().text = "GAME OVER";
            readyText.transform.GetComponent<Text>().color = Color.red;

            readyText.transform.GetComponent<Text>().enabled = true;
            playerText.transform.GetComponent<Text>().enabled = true;

            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            yield return new WaitForSeconds(delay);

            if (!GameMenu.isOnePlayerGame)
                isPlayerOneUp = !isPlayerOneUp;

            if (isPlayerOneUp)
                StartCoroutine(StartBlinking(playerOneUp));
            else
                StartCoroutine(StartBlinking(playerTwoUp));

            RedrawBoard();

            if (isPlayerOneUp)
                playerText.transform.GetComponent<Text>().text = "PLAYER ONE";
            else
                playerText.transform.GetComponent<Text>().text = "PLAYER TWO";

            readyText.transform.GetComponent<Text>().text = "READY!";
            readyText.transform.GetComponent<Text>().color = new Color(240f / 255f, 207f / 255f, 101f / 255f);

            yield return new WaitForSeconds(delay);

            StartCoroutine(ProcessRestartShowObjects(2));
        }

        else
        {
            GameObject pacMan = GameObject.Find("PacMan");
            pacMan.transform.GetComponent<SpriteRenderer>().enabled = false;

            transform.GetComponent<AudioSource>().Stop();

            if (!GameMenu.isOnePlayerGame)
                isPlayerOneUp = !isPlayerOneUp;

            if (isPlayerOneUp)
                StartCoroutine(StartBlinking(playerOneUp));
            else
                StartCoroutine(StartBlinking(playerTwoUp));

            if (!GameMenu.isOnePlayerGame)
            {
                if (isPlayerOneUp)
                    playerText.transform.GetComponent<Text>().text = "PLAYER ONE";
                else
                    playerText.transform.GetComponent<Text>().text = "PLAYER TWO";
>>>>>>> master
            }

            playerText.transform.GetComponent<Text>().enabled = true;
            readyText.transform.GetComponent<Text>().enabled = true;

            RedrawBoard();

            yield return new WaitForSeconds(delay);

            StartCoroutine(ProcessRestartShowObjects(1));
        }                
    }

    IEnumerator ProcessGameOver (float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("GameMenu");
    }

    IEnumerator ProcessRestartShowObjects (float delay)
    {
        playerText.transform.GetComponent<Text>().enabled = false;

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<SpriteRenderer>().enabled = true;
            ghost.transform.GetComponent<Animator>().enabled = true;
            ghost.transform.GetComponent<Ghost>().MoveToStartingPosition();
        }

<<<<<<< HEAD
        print("Total Pellets: " + totalPellets);
        print("Total Power Pellets: " + totalPowerPellets);
        print("Total Ghost House Nodes: " + totalGhostHouseNodes);
=======
        GameObject pacMan = GameObject.Find("PacMan");

        pacMan.transform.GetComponent<Animator>().enabled = false;
        pacMan.transform.GetComponent<SpriteRenderer>().enabled = true;
        pacMan.transform.GetComponent<PacMan>().MoveToStartingPosition();

        yield return new WaitForSeconds(delay);

        Restart();
>>>>>>> master
    }

    public void Restart()
    {
        int playerLevel = 0;

        if (isPlayerOneUp)
            playerLevel = playerOneLevel;
        else
            playerLevel = playerTwoLevel;

        GameObject.Find("PacMan").GetComponent<PacMan>().SetDefficultyForLevel(playerLevel);

        GameObject[] obj = GameObject.FindGameObjectsWithTag("Ghost");
        
        foreach (GameObject ghost in obj)
        {
            ghost.transform.GetComponent<Ghost>().SetDifficultyForLevel(playerLevel);
        }

        readyText.transform.GetComponent<Text>().enabled = false;        
        
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.transform.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }

        transform.GetComponent<AudioSource>().clip = backgroundAudioNormal;
        transform.GetComponent<AudioSource>().Play();

        didStartDeath = false;
    }

    void ResetPelletsForPlayer (int playerNum)
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            if (o.GetComponent<Tile>() !=null)
            {
                if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSuperPellet)
                {
                    if (playerNum == 1)
                    {
                        o.GetComponent<Tile>().didConsumePlayerOne = false;
                    }

                    else
                    {
                        o.GetComponent<Tile>().didConsumePlayerTwo = false;
                    }
                }
            }
        }
    }

    void RedrawBoard()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            if (o.GetComponent<Tile>() != null)
            {
                if (o.GetComponent<Tile>().isPellet || o.GetComponent<Tile>().isSuperPellet)
                {
                    if (isPlayerOneUp)
                    {
                        if (o.GetComponent<Tile>().didConsumePlayerOne)
                            o.GetComponent<SpriteRenderer>().enabled = false;
                        else
                            o.GetComponent<SpriteRenderer>().enabled = true;
                    }

                    else
                    {
                        if (o.GetComponent<Tile>().didConsumePlayerTwo)
                            o.GetComponent<SpriteRenderer>().enabled = false;
                        else
                            o.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }
}
