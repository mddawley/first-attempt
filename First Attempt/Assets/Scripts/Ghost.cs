﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    //- Ghost type
    public enum GhostType
    {
        Red,
        Pink,
        Blue,
        Orange
    }
    public GhostType ghostType = GhostType.Red;

    //- Move speed
    public float moveSpeed = 5.9f;
    public float normalMoveSpeed = 5.9f;
    public float frightenedModeMoveSpeed = 2.9f;
    public float consumedMoveSpeed = 15f;

    private float previousMoveSpeed;

    //- Ghost release timers
    public int pinkyReleaseTimer = 5;
    public int inkyReleaseTimer = 14;
    public int clydeReleaseTimer = 21;
    public float ghostReleaseTimer = 0;

    //- Mode timers
    public int scatterModeTimer1 = 7;
    public int chaseModeTimer1 = 20;
    public int scatterModeTimer2 = 7;
    public int chaseModeTimer2 = 20;
    public int scatterModeTimer3 = 5;
    public int chaseModeTimer3 = 20;
    public int scatterModeTimer4 = 5;
    public int frightenedModeDuration = 10;
    public int startBlinkingAt = 7;

    private int modeChangeIteration = 1;
    private float modeChangeTimer = 0;
    private float frightenedModeTimer = 0;
    private float blinkTimer = 0;

    //- Location
    public Node startingPosition;
    public Node homeNode;
    public Node ghostHouse;
    public bool isInGhostHouse = false;

    //- Animation related
    public RuntimeAnimatorController ghostUp;
    public RuntimeAnimatorController ghostDown;
    public RuntimeAnimatorController ghostLeft;
    public RuntimeAnimatorController ghostRight;
    public RuntimeAnimatorController ghostWhite;
    public RuntimeAnimatorController ghostBlue;
    public Sprite eyesUp;
    public Sprite eyesDown;
    public Sprite eyesLeft;
    public Sprite eyesRight;

    private bool frightenedModeIsWhite = false;

    //- Audio related
    private AudioSource backgroundAudio;

    //- Game modes
    public enum Mode
    {
        Chase,
        Scatter,
        Frightened,
        Consumed
    }
    Mode currentMode = Mode.Scatter;
    Mode previousMode;

    //-Movement
    public Node targetNode;
    public Node currentNode, previousNode;
    private Vector2 direction, nextDirection;

    //- Pac-Man reference
    private GameObject pacMan;

    // Use this for initialization
    void Start()
    {
        backgroundAudio = GameObject.Find("Game").transform.GetComponent<AudioSource>();

        pacMan = GameObject.FindGameObjectWithTag("PacMan");

        Node node = GetNodeAtPosition(transform.localPosition);
        Debug.Log(node);

        if (node != null)
        {
            currentNode = node;
        }

        if (isInGhostHouse)
        {
            direction = Vector2.up;
            targetNode = currentNode.neighbors[0];
        }

        else
        {
            //- Below altered to allow Red Ghost to begin centered
            direction = Vector2.left;
            currentNode = startingPosition;
            targetNode = ChooseNextNode();
        }

        previousNode = currentNode;

        UpdateAnimatorController();

    }

    public void Restart()
    {
        if (transform.name == "Ghost_Blinky")
        {
            transform.position = new Vector2(13.5f, 19);
            direction = Vector2.left;
        }
            
        else
        {
            transform.position = startingPosition.transform.position;
            isInGhostHouse = true;            
        }

        ghostReleaseTimer = 0;
        modeChangeIteration = 1;
        modeChangeTimer = 0;

        currentNode = startingPosition;

        if (isInGhostHouse)
        {
            direction = Vector2.up;
            targetNode = currentNode.neighbors[0];
        }

        else
        {
            direction = Vector2.left;
            targetNode = GetNodeAtPosition(new Vector2 (9, 19));
        }

        previousNode = currentNode;
        UpdateAnimatorController();        
    }
	
	// Update is called once per frame
	void Update()
    {
        ModeUpdate();

        Move();

        ReleaseGhosts();

        CheckCollision();

        CheckIsInGhostHouse();
	}

    void CheckIsInGhostHouse()
    {
        if (currentMode == Mode.Consumed)
        {
            GameObject tile = GetTileAtPosition(transform.position);

            if (tile != null)
            {
                if (tile.transform.GetComponent<Tile>() != null)
                {
                    if (tile.transform.GetComponent<Tile>().isGhostHouse)
                    {
                        moveSpeed = normalMoveSpeed;

                        Node node = GetNodeAtPosition(transform.position);

                        if (node != null)
                        {
                            currentNode = node;

                            direction = Vector2.up;
                            targetNode = currentNode.neighbors[0];

                            previousNode = currentNode;

                            currentMode = Mode.Chase;

                            UpdateAnimatorController();                         
                        }
                    }
                }
            }
        }
    }

    void CheckCollision()
    {
        Rect ghostRect = new Rect(transform.position, transform.GetComponent<SpriteRenderer>().sprite.bounds.size / 4);
        Rect pacManRect = new Rect(pacMan.transform.position, pacMan.transform.GetComponent<SpriteRenderer>().sprite.bounds.size / 4);

        if (ghostRect.Overlaps(pacManRect))
        {
            if (currentMode == Mode.Frightened)
            {
                Consumed();
                return; 
            }

            else if (currentMode != Mode.Frightened && currentMode != Mode.Consumed)
            {
                //- Pac-Man should die
                GameObject.Find("Game").transform.GetComponent<GameBoard>().Restart();
            }            
        }
    }

    void Consumed()
    {
        currentMode = Mode.Consumed;
        previousMoveSpeed = moveSpeed;
        moveSpeed = consumedMoveSpeed;
        UpdateAnimatorController();
    }

    void UpdateAnimatorController()
    {
        if (currentMode != Mode.Frightened && currentMode != Mode.Consumed)
        {
            if (direction == Vector2.up)
            {
                transform.GetComponent<Animator>().runtimeAnimatorController = ghostUp;
            }

            else if (direction == Vector2.down)
            {
                transform.GetComponent<Animator>().runtimeAnimatorController = ghostDown;
            }

            else if (direction == Vector2.left)
            {
                transform.GetComponent<Animator>().runtimeAnimatorController = ghostLeft;
            }

            else if (direction == Vector2.right)
            {
                transform.GetComponent<Animator>().runtimeAnimatorController = ghostRight;
            }

            else
            {
                transform.GetComponent<Animator>().runtimeAnimatorController = ghostLeft;
            }
        }

        else if (currentMode == Mode.Frightened)
        {
            transform.GetComponent<Animator>().runtimeAnimatorController = ghostBlue;
        }

        else if (currentMode == Mode.Consumed)
        {
            transform.GetComponent<Animator>().runtimeAnimatorController = null;

            if (direction == Vector2.up)
            {
                transform.GetComponent<SpriteRenderer>().sprite = eyesUp;
            }

            else if (direction == Vector2.down)
            {
                transform.GetComponent<SpriteRenderer>().sprite = eyesDown;
            }

            else if (direction == Vector2.left)
            {
                transform.GetComponent<SpriteRenderer>().sprite = eyesLeft;
            }

            else if (direction == Vector2.right)
            {
                transform.GetComponent<SpriteRenderer>().sprite = eyesRight;
            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null && !isInGhostHouse)
        {
            if (OverShotTarget())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);

                if (otherPortal != null)
                {
                    transform.localPosition = otherPortal.transform.position;

                    currentNode = otherPortal.GetComponent<Node>();
                }

                targetNode = ChooseNextNode();

                previousNode = currentNode;

                currentNode = null;

                UpdateAnimatorController();
            }

            else
            {
                transform.localPosition += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    void ModeUpdate()
    {
        if (currentMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;

            if (modeChangeIteration == 1)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer1)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }

                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer1)
                {
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }

            else if (modeChangeIteration == 2)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer2)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }

                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer2)
                {
                    modeChangeIteration = 3;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }

            else if (modeChangeIteration == 3)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer3)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;                    
                }

                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer3)
                {
                    modeChangeIteration = 4;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }

            else if (modeChangeIteration == 4)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer4)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
            }        
        }

        else if (currentMode == Mode.Frightened)
        {
            frightenedModeTimer += Time.deltaTime;

            if (frightenedModeTimer >=  frightenedModeDuration)
            {
                backgroundAudio.clip = GameObject.Find("Game").transform.GetComponent<GameBoard>().backgroundAudioNormal;
                backgroundAudio.Play();

                frightenedModeTimer = 0;
                ChangeMode(previousMode);
            }

            if (frightenedModeTimer >= startBlinkingAt)
            {
                blinkTimer += Time.deltaTime;

                if (blinkTimer >= 0.1f)
                {
                    blinkTimer = 0f;

                    if (frightenedModeIsWhite)
                    {
                        transform.GetComponent<Animator>().runtimeAnimatorController = ghostBlue;
                        frightenedModeIsWhite = false;
                    }

                    else
                    {
                        transform.GetComponent<Animator>().runtimeAnimatorController = ghostWhite;
                        frightenedModeIsWhite = true;
                    }
                }
            }
        }
    }

    void ChangeMode (Mode m)
    {
        if (currentMode == Mode.Frightened)
        {
            moveSpeed = previousMoveSpeed;
       
        }

        if (m == Mode.Frightened)
        {
            previousMoveSpeed = moveSpeed;
            moveSpeed = frightenedModeMoveSpeed;
        }

        if (currentMode != m)
        {
            previousMode = currentMode;
            currentMode = m;
        }
        UpdateAnimatorController();
    }
    
    public void StartFrightenedMode()
    {
        if (currentMode != Mode.Consumed)
        {
            frightenedModeTimer = 0;
            backgroundAudio.clip = GameObject.Find("Game").transform.GetComponent<GameBoard>().backgroundAudioFrightened;
            backgroundAudio.Play();
            ChangeMode(Mode.Frightened);
        }        
    }

    Vector2 GetRedGhostTargetTile ()
    {
        Vector2 pacManPosition = pacMan.transform.localPosition;
        Vector2 targetTile = new Vector2(Mathf.RoundToInt(pacManPosition.x), Mathf.RoundToInt(pacManPosition.y));

        return targetTile;
    }

    Vector2 GetPinkGhostTargetTile ()
    {
        //- Four tiles ahead Pac-Man
        //- Taking account Position and Orientation
        Vector2 pacManPosition = pacMan.transform.localPosition;
        Vector2 pacManOrientation = pacMan.GetComponent<PacMan>().orientation;

        int pacManPositionX = Mathf.RoundToInt(pacManPosition.x);
        int pacManPositionY = Mathf.RoundToInt(pacManPosition.y);

        Vector2 pacManTile = new Vector2(pacManPositionX, pacManPositionY);
        Vector2 targetTile = pacManTile + (4 * pacManOrientation);

        return targetTile;
    }

    Vector2 GetBlueGhostTargetTile ()
    {
        //- Select the position two tiles in front of Pac-Man
        //- Draw Vector from Blinky to that position
        //- Double the length of the vector
        Vector2 pacManPosition = pacMan.transform.localPosition;
        Vector2 pacManOrientation = pacMan.GetComponent<PacMan>().orientation;

        int pacManPositionX = Mathf.RoundToInt(pacManPosition.x);
        int pacManPositionY = Mathf.RoundToInt(pacManPosition.y);

        Vector2 pacManTile = new Vector2(pacManPositionX, pacManPositionY);

        Vector2 targetTile = pacManTile + (2 * pacManOrientation);

        //- Temporary Blinky Position
        Vector2 tempBlinkyPosition = GameObject.Find("Ghost_Blinky").transform.localPosition;

        int blinkyPositionX = Mathf.RoundToInt(tempBlinkyPosition.x);
        int blinkyPositionY = Mathf.RoundToInt(tempBlinkyPosition.y);

        tempBlinkyPosition = new Vector2(blinkyPositionX, blinkyPositionY);

        float distance = GetDistance(tempBlinkyPosition, targetTile);
        distance *= 2;

        targetTile = new Vector2(tempBlinkyPosition.x + distance, tempBlinkyPosition.y + distance);

        return targetTile;
    }

    Vector2 GetOrangeGhostTargetTile ()
    {
        //- Calculate the distance from Pac-Man
        //- If the distance is greater than eight tiles targeting is the same as Blinky
        //- If the distance is less than eight tiles, then target is his home node, so same as scatter mode.

        Vector2 pacManPosition = pacMan.transform.localPosition;

        float distance = GetDistance(transform.localPosition, pacManPosition);
        Vector2 targetTile = Vector2.zero;

        if (distance > 8)
        {
            targetTile = new Vector2(Mathf.RoundToInt(pacManPosition.x), Mathf.RoundToInt(pacManPosition.y));
        }

        else if (distance < 8)
        {
            targetTile = homeNode.transform.position;
        }

        return targetTile;
    }

    void ReleasePinkGhost()
    {
        if (ghostType == GhostType.Pink && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseBlueGhost()
    {
        if (ghostType == GhostType.Blue && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseOrangeGhost()
    {
        if (ghostType == GhostType.Orange && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseGhosts()
    {
        ghostReleaseTimer += Time.deltaTime;

        if (ghostReleaseTimer > pinkyReleaseTimer)
            ReleasePinkGhost();

        if (ghostReleaseTimer > inkyReleaseTimer)
            ReleaseBlueGhost();

        if (ghostReleaseTimer > clydeReleaseTimer)
            ReleaseOrangeGhost();
    }

    Vector2 GetTargetTile()
    {
        Vector2 targetTile = Vector2.zero;

        if (ghostType == GhostType.Red)
        {
            targetTile = GetRedGhostTargetTile();
        }

        else if (ghostType == GhostType.Pink)
        {
            targetTile = GetPinkGhostTargetTile();
        }

        else if (ghostType == GhostType.Blue)
        {
            targetTile = GetBlueGhostTargetTile();
        }

        else if (ghostType == GhostType.Orange)
        {
            targetTile = GetOrangeGhostTargetTile();
        }
        return targetTile;
    }

    Vector2 GetRandomTile()
    {
        int x = Random.Range(0, 28);
        int y = Random.Range(0, 36);

        return new Vector2(x, y);
    }

    Node ChooseNextNode()
    {
        Vector2 targetTile = Vector2.zero;

        if (currentMode == Mode.Chase)
        {
            targetTile = GetTargetTile();
        }
        
        else if (currentMode == Mode.Scatter)
        {
            targetTile = homeNode.transform.position;
        }

        else if (currentMode == Mode.Frightened)
        {
            targetTile = GetRandomTile();
        }

        else if (currentMode == Mode.Consumed)
        {
            targetTile = ghostHouse.transform.position;
        }

        Node moveToNode = null;

        Node[] foundNodes = new Node[4];
        Vector2[] foundNodesDirection = new Vector2[4];

        int nodeCounter = 0;

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections [i] != direction * -1)
            {
                if (currentMode != Mode.Consumed)
                {
                    GameObject tile = GetTileAtPosition(currentNode.transform.position);

                    if (tile.transform.GetComponent<Tile>().isGhostHouseEntrance == true)
                    {
                        //- Found a ghost house, don't want to allow monvement
                        if (currentNode.validDirections[i] != Vector2.down)
                        {
                            foundNodes[nodeCounter] = currentNode.neighbors[i];
                            foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                            nodeCounter++;
                        }
                    }

                    else
                    {
                        foundNodes[nodeCounter] = currentNode.neighbors[i];
                        foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                        nodeCounter++;
                    }
                }

                else
                {
                    foundNodes[nodeCounter] = currentNode.neighbors[i];
                    foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                    nodeCounter++;
                }                
            }
        }

        if (foundNodes.Length == 1)
        {
            moveToNode = foundNodes[0];
            direction = foundNodesDirection[0];
        }

        if (foundNodes.Length > 1)
        {
            float leastDistance = 100000f;

            for (int i = 0; i < foundNodes.Length; i++)
            {
                if (foundNodesDirection [i] != Vector2.zero)
                {
                    float distance = GetDistance(foundNodes[i].transform.position, targetTile);

                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        moveToNode = foundNodes[i];
                        direction = foundNodesDirection[i];
                        
                    }
                }
            }
        }

        return moveToNode;
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
        {
            if (tile.GetComponent<Node>() != null)
            {
                return tile.GetComponent<Node>();
            }            
        }

        return null;
    }

    GameObject GetTileAtPosition(Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").transform.GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
            return tile;

        return null;
    }   

    GameObject GetPortal(Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").transform.GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
        {
            if (tile.GetComponent<Tile> ().isPortal)
            {
                GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver;
                return otherPortal;
            }
        }

        return null;
    }
    
    float LengthFromNode(Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }

    float GetDistance(Vector2 posA, Vector2 posB)
    {
        float dx = posA.x - posB.x;
        float dy = posA.y - posB.y;

        float distance = Mathf.Sqrt(dx * dx + dy * dy);

        return distance;
    }
}
