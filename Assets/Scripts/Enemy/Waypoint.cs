using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Start is called before the first frame update
    #region Variables

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public Attacking dronebehaviour;

    public float speed = 2f;
    int fromWayPointIndex;
    private float percentTravelledBetweenWaypoints;

    public float waitTime = 1f;
    float nextMoveTime;

    [Range(0, 2)]
    public float easeAmount;

    public Vector3 newpos;

    public bool cyclic;

    private bool dontmove;
    #endregion

    private void Start()
    {
        dronebehaviour = GetComponent<Attacking>();

        if (localWaypoints != null)
        {
            globalWaypoints = new Vector3[localWaypoints.Length];

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + transform.position;
            }
        }

      
    }

    private void Update()
    {
        if (dronebehaviour.dronestate != Attacking.Dronestate.FoundHim && speed > 0.2 && Time.timeScale >=1&& localWaypoints != null) ;
        {
            Vector3 objectVelocity = CalculateObjectVelocity();

            if (!dontmove)
            {
                transform.Translate(objectVelocity);
            }
           
        }
     
    }

    float Ease(float x)
    {
        float a = easeAmount + 1;
        return (Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a)));
    }

    Vector3 CalculateObjectVelocity()
    {

        if (globalWaypoints.Length > 0)
        {
             if (Time.time < nextMoveTime)
        {
          
            return Vector3.zero;
        }
        if (dronebehaviour.dronestate == Attacking.Dronestate.FoundHim)
        {
            
            fromWayPointIndex %= globalWaypoints.Length;//ADD
            int toWayPointIndex = (fromWayPointIndex + 1) % globalWaypoints.Length; //CHANGE
            float distanceBetweenWayPoints = Vector3.Distance(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex]);
            percentTravelledBetweenWaypoints += Time.deltaTime * ((speed/1.2f) / distanceBetweenWayPoints);
            percentTravelledBetweenWaypoints = Mathf.Clamp01(percentTravelledBetweenWaypoints); //ADD

            float easedPercentTravelled = Ease(percentTravelledBetweenWaypoints);

            Vector3 newPosition = Vector3.Lerp(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex], easedPercentTravelled);

            if (percentTravelledBetweenWaypoints >= 1)
            {
                percentTravelledBetweenWaypoints = 0;
                fromWayPointIndex++;

                if (!cyclic)
                {
                    if (fromWayPointIndex >= globalWaypoints.Length - 1)
                    {
                        fromWayPointIndex = 0;
                        System.Array.Reverse(globalWaypoints);
                    }
                }


                nextMoveTime = Time.time + waitTime;
            }

            newpos = (newPosition - transform.position);

        }
        else
        {
           
            fromWayPointIndex %= globalWaypoints.Length;//ADD
            int toWayPointIndex = (fromWayPointIndex + 1) % globalWaypoints.Length; //CHANGE
            float distanceBetweenWayPoints = Vector3.Distance(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex]);
                percentTravelledBetweenWaypoints += Time.deltaTime * (speed / distanceBetweenWayPoints);
                percentTravelledBetweenWaypoints = Mathf.Clamp01(percentTravelledBetweenWaypoints); //ADD

                float easedPercentTravelled = Ease(percentTravelledBetweenWaypoints);

                Vector3 newPosition = Vector3.Lerp(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex], easedPercentTravelled);

                if (percentTravelledBetweenWaypoints >= 1)
                {
                    percentTravelledBetweenWaypoints = 0;
                    fromWayPointIndex++;

                    if (!cyclic)
                    {
                        if (fromWayPointIndex >= globalWaypoints.Length - 1)
                        {
                            fromWayPointIndex = 0;
                            System.Array.Reverse(globalWaypoints);
                        }
                    }


                    nextMoveTime = Time.time + waitTime;
                }

                newpos = (newPosition - transform.position);
                
        }
        return newpos;
        }
        else
        {
            dontmove = true;
            return transform.localPosition;
        }
       
    }

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.25f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWayPointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointPos - Vector3.up * size, globalWayPointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointPos - Vector3.left * size, globalWayPointPos + Vector3.left * size);
            }
        }
    }
}

