using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
       
    [Header("Zone Options")]
    public int numberOfZones = 60;
    public int moveAmount = 10;
    public GameObject parent;

    [Header("Zone Sets")]
    public GameObject[] startZones;
    public GameObject[] straightZones; 
    public GameObject[] curvedZones;
    public GameObject[] crossZones;
    public GameObject[] deadEndZones;

    private GameObject walker;

    private List<Zone> zones = new List<Zone>(); 
    private List<Vector2> takenPositions = new List<Vector2>(); 
    private List<Vector2> flaggedPositions = new List<Vector2>(); // will keep track of times when a walker crosses another's path

    private int direction;
    private int lastDirection;
    private int zoneCounter = 1;
    private int zonesPrinted = 0;

    private Vector2 newPos;

    private Zone.Origin zoneOrigin;
    private Zone.Origin prevZone;

    private void Start()
    {
        // First we will place a startzone where the game manger is and add that position to the taken list
        int randZone = Random.Range(0, startZones.Length);

        GameObject instance = Instantiate(startZones[randZone], transform.position, Quaternion.identity);

        instance.transform.parent = parent.transform;

        takenPositions.Insert(0, new Vector2(transform.position.x, transform.position.y));

        walker = GameObject.FindGameObjectWithTag("road walker");
                              
        direction = 5; // 1 or 2 is left, 3 or 4 is right, 5 is down

        lastDirection = direction;

    }

    private void Update()
    {
        
        while (zoneCounter <= numberOfZones)
        {
                        
            //Instantiate(filler, walker.transform.position, Quaternion.identity); // this is just for testing

            // since the walker is already a space away from the starting location we will put a zone here
            if (lastDirection == 1 || lastDirection == 2) // The last direction was left so the zoneOrigin is right
            {

                zoneOrigin = Zone.Origin.Right;

            }
            else if (lastDirection == 3 || lastDirection == 4) // The last direction was right so the zoneOrigin is left
            {

                zoneOrigin = Zone.Origin.Left;

            }
            else // The last direction was down so the zoneOrigin is above
            {

                zoneOrigin = Zone.Origin.Above;

            }

            takenPositions.Insert(0, new Vector2(walker.transform.position.x, walker.transform.position.y));

            Zone instance = new Zone(walker.transform.position, zoneOrigin);

            zones.Insert(0, instance);

            // check the if the last direction was left or right to make sure we dont move backwards
            if (lastDirection == 1 || lastDirection == 2) // The last direction was left
            {

                direction = Random.Range(1, 6);

                // We cant go right so if the direction is picked to be 3 or 4 we switch it to go left or down
                if (direction == 3)
                {

                    direction = 1;

                }
                else if (direction == 4)
                {

                    direction = 5;

                }

            }
            else if (lastDirection == 3 || lastDirection == 4) // The last direction was right
            {

                // Similar to above, we cant go left so we can just have it pick between 3 and 5
                direction = Random.Range(3, 6);

            }
            else
            {

                direction = Random.Range(1, 6);

            }

            // now that we picked a new direction do the same process of moving gameManger the as the Start method
            if (direction == 1 || direction == 2) // Move Left
            {

                newPos = new Vector2(walker.transform.position.x - moveAmount, walker.transform.position.y);
                walker.transform.position = newPos;

            }
            else if (direction == 3 || direction == 4) // Move Right
            {

                newPos = new Vector2(walker.transform.position.x + moveAmount, walker.transform.position.y);
                walker.transform.position = newPos;

            }
            else // Move Down
            {

                newPos = new Vector2(walker.transform.position.x, walker.transform.position.y - moveAmount);
                walker.transform.position = newPos;

            }

            lastDirection = direction;
            zoneCounter++;

        }

        // now that all of our zone locations have been picked we want to use the zone origin to start instantiating 
        // the correct zones. if we start at the at zones[0] that will be the last zone added.         
        while (zonesPrinted < zones.Count)
        {

            // if zonesPrinted is 0 we will print a deadEndZone and set the prevZone
            if (zonesPrinted == 0)
            {

                if (zones[zonesPrinted].zoneOrigin == Zone.Origin.Above) // spawn a deadEnd with rotation 180
                {

                    int randZone = Random.Range(0, deadEndZones.Length);
                    GameObject instance = Instantiate(deadEndZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 180);
                    prevZone = Zone.Origin.Down;
                    instance.transform.parent = parent.transform;

                }
                else if (zones[zonesPrinted].zoneOrigin == Zone.Origin.Left) // spawn a deadEnd with rotation 90
                {

                    int randZone = Random.Range(0, deadEndZones.Length);
                    GameObject instance = Instantiate(deadEndZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, -90);
                    prevZone = Zone.Origin.Right;
                    instance.transform.parent = parent.transform;

                }
                else if (zones[zonesPrinted].zoneOrigin == Zone.Origin.Right) // spawn a deadEnd with rotation 270
                {

                    int randZone = Random.Range(0, deadEndZones.Length);
                    GameObject instance = Instantiate(deadEndZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 90);
                    prevZone = Zone.Origin.Left;
                    instance.transform.parent = parent.transform;

                }                

            }
            else
            {
                                
                if (prevZone == Zone.Origin.Down && zones[zonesPrinted].zoneOrigin == Zone.Origin.Right) 
                {
                    // spawn a curved road without rotation
                    int randZone = Random.Range(0, curvedZones.Length);
                    GameObject instance = Instantiate(curvedZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    prevZone = Zone.Origin.Left;
                    instance.transform.parent = parent.transform;

                }
                else if (prevZone == Zone.Origin.Down && zones[zonesPrinted].zoneOrigin == Zone.Origin.Above)
                {
                    // spawn a straight road with rotation 90
                    int randZone = Random.Range(0, straightZones.Length);
                    GameObject instance = Instantiate(straightZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 90);
                    prevZone = Zone.Origin.Down;
                    instance.transform.parent = parent.transform;

                }
                else if (prevZone == Zone.Origin.Right && zones[zonesPrinted].zoneOrigin == Zone.Origin.Left)
                {
                    // spawn a straight road without rotation
                    int randZone = Random.Range(0, straightZones.Length);
                    GameObject instance = Instantiate(straightZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    prevZone = Zone.Origin.Right;
                    instance.transform.parent = parent.transform;

                }
                else if (prevZone == Zone.Origin.Down && zones[zonesPrinted].zoneOrigin == Zone.Origin.Left)
                {
                    // spawn a curved road with rotation 270
                    int randZone = Random.Range(0, curvedZones.Length);
                    GameObject instance = Instantiate(curvedZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 270);
                    prevZone = Zone.Origin.Right;
                    instance.transform.parent = parent.transform;

                }
                else if(prevZone == Zone.Origin.Left && zones[zonesPrinted].zoneOrigin == Zone.Origin.Right)
                {
                    // spawn a straight road without rotation
                    int randZone = Random.Range(0, straightZones.Length);
                    GameObject instance = Instantiate(straightZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    prevZone = Zone.Origin.Left;
                    instance.transform.parent = parent.transform;

                }
                else if (prevZone == Zone.Origin.Left && zones[zonesPrinted].zoneOrigin == Zone.Origin.Above)
                {
                    // spawn a curved road with rotaion 180
                    int randZone = Random.Range(0, curvedZones.Length);
                    GameObject instance = Instantiate(curvedZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 180);
                    prevZone = Zone.Origin.Down;
                    instance.transform.parent = parent.transform;

                }
                else if (prevZone == Zone.Origin.Right && zones[zonesPrinted].zoneOrigin == Zone.Origin.Above)
                {
                    // spawn a curved road with rotaion 90
                    int randZone = Random.Range(0, curvedZones.Length);
                    GameObject instance = Instantiate(curvedZones[randZone], takenPositions[zonesPrinted], Quaternion.identity);
                    instance.transform.Rotate(0, 0, 90);
                    prevZone = Zone.Origin.Down;
                    instance.transform.parent = parent.transform;

                }

            }

            zonesPrinted++;

        }        

    }

}
