﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public AudioClip chomp1;
    public AudioClip chomp2;

    public RuntimeAnimatorController chompAnimation;
    public RuntimeAnimatorController deathAnimation;

    public Vector2 orientation;

    public float speed = 6.0f;

    public Sprite idleSprite;

    public bool canMove = true;

    private bool playedChomp1 = false;

    private AudioSource audio;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;

    private Node currentNode, previousNode, targetNode;

    public Node StartingNode;

    // Use this for initialization
    void Start()
    {
        audio = transform.GetComponent<AudioSource>();

        currentNode = StartingNode;

        direction = Vector2.left;
        orientation = Vector2.left;

        ChangePosition(direction);

        if (GameBoard.isPlayerOneUp)
        {
            SetDefficultyForLevel(GameBoard.playerOneLevel);
        }
         else
        {
            SetDefficultyForLevel(GameBoard.playerTwoLevel);
        }
    }

    public void SetDefficultyForLevel (int level)
    {
        if (level == 1)
        {
            speed = 6;
        }

        else if (level == 2)
        {
            speed = 7;
        }

        else if (level == 3)
        {
            speed = 8;
        }

        else if (level == 4)
        {
            speed = 9;
        }

        else if (level == 5)
        {
            speed = 10;
        }
    }

    public void MoveToStartingPosition ()
    {
        transform.position = new Vector2(13.5f, 7);

        transform.GetComponent<SpriteRenderer>().sprite = idleSprite;

        direction = Vector2.left;
        orientation = Vector2.left;

        UpdateOrientation();
    }

    public void Restart()
    {
        canMove = true;

        currentNode = StartingNode;
        
        nextDirection = Vector2.left;

        transform.GetComponent<Animator>().runtimeAnimatorController = chompAnimation;
        transform.GetComponent<Animator>().enabled = true;

        ChangePosition(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            CheckInput();

            Move();

            UpdateOrientation();

            UpdateAnimationState();

            ConsumePellet();
        }        
    }

    void PlayChompSound()
    {
        if (playedChomp1)
        {
            //- Play chomp 2, set playedChomp1 to false
            audio.PlayOneShot(chomp2);
            playedChomp1 = false;
        }

        else
        {
            //- Play chomp1, set playedChomp1 to true
            audio.PlayOneShot(chomp1);
            playedChomp1 = true;
        }
    }

    void CheckInput() {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
        }

        else if (Input.GetKey(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }
    }

    void ChangePosition(Vector2 d)
    {
        if (d != direction)
            nextDirection = d;

        if (currentNode != null)
        {
            Node moveToNode = CanMove(d);

            if (moveToNode != null)
            {
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {
            if (nextDirection == direction * -1)
            {
                direction *= -1;

                Node tempNode = targetNode;

                targetNode = previousNode;

                previousNode = tempNode;
            }

            if (OverShotTarget ())
            {
                currentNode = targetNode;

                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);

                if (otherPortal != null)
                {
                    transform.localPosition = otherPortal.transform.position;

                    currentNode = otherPortal.GetComponent<Node>();
                }

                Node moveToNode = CanMove(nextDirection);

                if (moveToNode != null)
                    direction = nextDirection;

                if (moveToNode == null)
                    moveToNode = CanMove(direction);

                if (moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }

                else
                {
                    direction = Vector2.zero;
                }                      
            }

            else
            {
                transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
            }
        }       
    }

    void MoveToNode (Vector2 d)
    {
        Node moveToNode = CanMove(d);

        if (moveToNode != null)
        {
            transform.localPosition = moveToNode.transform.position;
            currentNode = moveToNode;
        }
    }

    void UpdateOrientation()
    {
        if (direction == Vector2.left)
        {
            orientation = Vector2.left;
            transform.localScale = new Vector3(-1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else if (direction == Vector2.right)
        {
            orientation = Vector2.right;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else if (direction == Vector2.up)
        {
            orientation = Vector2.up;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        else if (direction == Vector2.down)
        {
            orientation = Vector2.down;
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void UpdateAnimationState ()
    {
        if (direction == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

        else
        {
            GetComponent<Animator>().enabled = true;
        }
    }

    void ConsumePellet()
    {
        GameObject o = GetTileAtPosition(transform.position);

        if (o != null)
        {
            Tile tile = o.GetComponent<Tile>();

            if (tile != null)
            {
                bool didConsume = false;

                if (GameBoard.isPlayerOneUp)
                {
                    if (!tile.didConsumePlayerOne && (tile.isPellet || tile.isSuperPellet))
                    {
                        didConsume = true;
                        tile.didConsumePlayerOne = true;

                        if (tile.isSuperPellet)
                            GameBoard.playerOneScore += 50;
                        else
                            GameBoard.playerOneScore += 10;

                        GameMenu.playerOnePelletsConsumed++;
                        GameMenu.ghostReleasePelletCounterP1++;
                    }

                    if (tile.isBonusItem)
                        ConsumedBonusItem(1, tile);
                }

                else
                {
                    if (!tile.didConsumePlayerTwo && (tile.isPellet || tile.isSuperPellet))
                    {
                        didConsume = true;
                        tile.didConsumePlayerTwo = true;

                        if (tile.isSuperPellet)
                            GameBoard.playerTwoScore += 50;
                        else
                            GameBoard.playerTwoScore += 10;

                        GameMenu.playerTwoPelletsConsumed++;
                        GameMenu.ghostReleasePelletCounterP2++;
                    }

                    if (tile.isBonusItem)
                        ConsumedBonusItem(2, tile);
                }

                if (didConsume)
                {
                    o.GetComponent<SpriteRenderer>().enabled = false;

                    PlayChompSound();

                    if (tile.isSuperPellet)
                    {
                        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");

                        foreach (GameObject go in ghosts)
                        {
                            go.GetComponent<Ghost>().StartFrightenedMode();
                        }
                    }
                }
            }
        }
    }

    void ConsumedBonusItem (int playerNum, Tile bonusItem)
    {
        if (playerNum == 1)
        {
            GameBoard.playerOneScore += bonusItem.pointValue;
        }

        else
        {
            GameBoard.playerTwoScore += bonusItem.pointValue;
        }

        GameObject.Find("Game").transform.GetComponent<GameBoard>().StartConsumedBonusItem(bonusItem.gameObject, bonusItem.pointValue);        
    }

    Node CanMove(Vector2 d)
    {
        Node moveToNode = null;

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
<<<<<<< HEAD
            if (currentNode.isGhostHouseEntrance == true && i == 2)
            {
                moveToNode = null;
                break;
            }

            else if (currentNode.validDirections[i] == d)
=======
            if (d.y == -1)
            {
                if (!currentNode.GetComponent<Tile>().isGhostHouseEntrance)
                {
                    if (currentNode.validDirections[i] == d)
                    {
                        moveToNode = currentNode.neighbors[i];
                        break;
                    }
                }

                else
                {
                    return moveToNode;
                }
            }

            if (currentNode.validDirections [i] == d)
>>>>>>> master
            {
                moveToNode = currentNode.neighbors[i];
                break;
            }
        }

        return moveToNode;
    }

    GameObject GetTileAtPosition (Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
            return tile;

        return null;
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").transform.GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
        {
            return tile.GetComponent<Node>();
        }

        return null;
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode(Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

    GameObject GetPortal (Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x * 10);
        int tileY = Mathf.RoundToInt(pos.y * 10);

        GameObject tile = GameObject.Find("Game").transform.GetComponent<GameBoard>().board[tileX, tileY];

        if (tile != null)
        {
            if (tile.GetComponent<Tile>() != null)
            {
                if (tile.GetComponent<Tile>().isPortal)
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver;
                    return otherPortal;
                }
            }
        }

        return null;
    }
}
