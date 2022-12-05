using HelloWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLocal : MonoBehaviour
{


    //public static int mode = 1;
    //public static bool useGui = true;
    public static int turnCounter = 0;
    public GameObject prefab;

    public Transform TransformToLookAtAfterTurnEnds;

    float NextTurnTime;
    // The duration of each turn in turn-based mode
    public float TurnDuration = 3;
    public static float timeRemaining;
    public static int NextPlayerIndex = 0;
    public int gameWinnerIndex = -1;
    public static int numOfPlayersDone = 0;
    public int mode;
    int CurrentPlayerIndex
    {
        get
        {
            return (NextPlayerIndex + players.Count - 1) % players.Count;
        }
    }
    public static List<GameObject> players = new List<GameObject>();
    public static List<GameObject> playersOriginal = new List<GameObject>();



    private void Start()
    {
        mode = 1;
    }

    //Start with GUI options in loading screen
    void OnGUI()
    {
        if (mode == 1)
        {
            //Draw all GUI objects

            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            
            
                StatusLabels();

                SubmitNewPosition();
            

            GUILayout.EndArea();
        }
        if(mode == 4)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            levelDoneGUI();
            GUILayout.EndArea();

        }
    }


    public void levelDoneGUI()
    {

        if(gameWinnerIndex != -1) //if this was set (ie multiplayer version)
        {
            GUILayout.Label("Player " + gameWinnerIndex + " Won!");
        }

        int i = 1;
        foreach (GameObject p in playersOriginal)
        {
            GUILayout.Label("Player " +  i + " took  " + p.GetComponent<PlayerMovementLocal>().getTurnCount() + " turns!");
            i += 1;
        }

        if (GUILayout.Button("Add a player!"))
        {
            GameObject p = Instantiate(prefab, new Vector3(players.Count * 4, 0, 0), Quaternion.identity);
            players.Add(p);
            playersOriginal.Add(p);

        }
        if (GUILayout.Button("Play Again?"))
        {
            players.Clear();
            foreach (GameObject p in playersOriginal)
            {
                //TODO: Maybe call a func within playerMovement that cleans up everything
                players.Add(p);
            }
                mode = 2;

        }


    }


    static void StatusLabels()
    {
       

        
            GUILayout.Label("Number of players ready: " + players.Count);

            System.String temp2 = "";
            foreach (GameObject p in players)
            {
                temp2 += " " + p.GetInstanceID();



            }
            GUILayout.Label("Players: " + temp2);

        

    }

    public void SubmitNewPosition()
    {
        if (GUILayout.Button("Add a player!"))
        {
            GameObject p = Instantiate(prefab, new Vector3(players.Count*4, 0, 0), Quaternion.identity);
            players.Add(p);
            playersOriginal.Add(p);

        }
        if (GUILayout.Button("Start Game!"))
        {
            mode = 2;

        }



    }

    public void initialPositions()
    {
        //Just place em a bit apart for now and get the cameras going.

        //placing
        int i = 0;
        gameWinnerIndex = -1;
        Vector3 p1Pos = new Vector3(0, 0f, 0f);

        foreach (GameObject p in playersOriginal)
        {
            p.GetComponent<PlayerMovementLocal>().MoveAbsolute(new Vector3(i, 0f, 0f));



            //updateCamera(p.gameObject);
            i += 3;

        }

        Camera.main.GetComponent<Camera>().gameObject.transform.position = new Vector3(p1Pos.x, p1Pos.y + 4, p1Pos.z - 10);
        Camera.main.GetComponent<Camera>().transform.LookAt(p1Pos);


        // Set the next turn time and player index
        NextTurnTime = Time.time + TurnDuration;
        timeRemaining = 0;
        mode = 3;




    }


    public void transitionCamerasAfterTurnEnds(int pIndex)
    {
        // TODO: change active virtual camera to new player
        Camera.main.GetComponent<Camera>().transform.LookAt(players[pIndex].transform);

    }

    public void levelDone()
    {

    }
    public void gameLoop()
    {

        turnCounter = 109;

        if (timeRemaining <= 0)
        {
            turnCounter += 1;
            // It is time to switch turns
            // Set the next player's turn
            if (players.Count == 1)
            {
                players[0].GetComponent<PlayerMovementLocal>().SubmitTurnRequest(false);
                if (players[0].GetComponent<PlayerMovementLocal>().getIsInHole())
                {
                    //last player finally got it in!
                    mode = 4;
                }
                else
                {
                    players[0].GetComponent<PlayerMovementLocal>().SubmitTurnRequest(true);
                    transitionCamerasAfterTurnEnds(NextPlayerIndex);
                }
              
            }
            else
            {
                

                // Set the current player's turn to false
                players[CurrentPlayerIndex].GetComponent<PlayerMovementLocal>().SubmitTurnRequest(false);
                if(players[CurrentPlayerIndex].GetComponent<PlayerMovementLocal>().getIsInHole())
                { //take care, someone just got it in! 
                    if(gameWinnerIndex == -1)
                    {
                        gameWinnerIndex = CurrentPlayerIndex + 1;
                    }
                    players.RemoveAt(CurrentPlayerIndex);
                    if(NextPlayerIndex >=1)
                    {
                        //Subtract to make up for lost element
                        NextPlayerIndex -= 1;
                    }
                   
                   
                }

                transitionCamerasAfterTurnEnds(NextPlayerIndex);
                players[NextPlayerIndex].GetComponent<PlayerMovementLocal>().SubmitTurnRequest(true);
                
            }
            // Set the next turn time and player index
            timeRemaining = TurnDuration;
            
            NextPlayerIndex = (NextPlayerIndex + 1) % players.Count;

        }
        else if (players[CurrentPlayerIndex].GetComponent<PlayerController>().getIsHit())
        {
            Debug.Log("Player is hitting the ball: " + CurrentPlayerIndex);
        }
        else
        {
            if(players.Count != 1)
            {//dont give time limit if only 1 player!
                timeRemaining -= Time.deltaTime;
            }
            Debug.Log("Time Remaining: " + timeRemaining);
        }
    }

    public static void SetTimeRemainingZero()
    {
        timeRemaining = 0.0f;
    }

    public Transform getTransformToLookAtAfterTurnEnds()
    {
        // TODO: Get jazzes jizz
        return TransformToLookAtAfterTurnEnds;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 2)
        {
            //setup players
            initialPositions();
        }

        if (mode ==3)
        {
            //Play the game!
            gameLoop();
        }

        if(mode ==4 )
        {
            //Game has finished!
            levelDone();
        }
        
    }
}
