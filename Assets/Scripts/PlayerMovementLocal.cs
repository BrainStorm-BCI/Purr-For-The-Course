using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;


namespace HelloWorld

    //Right now we randomly move the object every frame.
{
    public class PlayerMovementLocal : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(new Vector3(0f, 0f, 0f));
        private NetworkVariable<bool> isTurn = new NetworkVariable<bool>(false);
        public static int mode;
        public int uniqueID;



        protected virtual void Awake()
        {
            //Add me when I awake - this is called on both the clients and the host, so everyone will know me
            //ActivePlayers.Add(this);
        }
        protected virtual void OnDestroy()
        {
            //Cleanup - this is important since static members will persist until application is quit
            //ActivePlayers.Remove(this);
        }

       

        public void Move()
        {
            
            //you are a client, so call a server function to change the synced position variable.
                
               

            SubmitPositionIncrementRequest();
            
        }

      

      
        void SubmitPositionIncrementRequest()
        {
            //Call a func that calculates what you wanna do to the position and on the next frame when update is called we'll move it there
            Position.Value += new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
        }

      
        void SubmitPositionRequest(Vector3 newPosition)
        {
            //Call a func that calculates what you wanna do to the position and on the next frame when update is called we'll move it there
            Position.Value = newPosition;
        }



        
        public void SubmitTurnRequest(bool p)
        {
            isTurn.Value = p;


        }

     

        public bool getBoolTurn()
        {
            return isTurn.Value;
        }

       

       

        public void MoveAbsolute(Vector3 v)
        {
           
            SubmitPositionRequest(v);
            
        }


        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
        }

        public Vector3 getPosition()
        {
            return Position.Value;
        }

        public int getMode()
        {
            return mode;
        }
        void Update()
        {

            transform.position = Position.Value;
            if (getBoolTurn())
            {
                SubmitPositionIncrementRequest();
            }
            
        }
    }
}