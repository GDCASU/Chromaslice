using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{

    private GameObject AI1;
    private GameObject AI2;
    private GameObject player1;
    private GameObject player2;
    private Vector3 futureAI1Position;
    private Vector3 futureAI2Position;
    private Vector3 futurePlayer1Position;
    private Vector3 futurePlayer2Position;
    private float playerRadius;
    private float currentEvaluation;
    private Vector3 currentAI1Direction;
    private Vector3 currentAI2Direction;
    private float maxSpeed;
    private float maxRopeLength;

    // Use this for initialization
    void Start()
    {
        AI1 = this.gameObject.GetComponent<Team>().player1;
        AI2 = this.gameObject.GetComponent<Team>().player2;
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        player1 = Players[0];
        player2 = Players[1];
        playerRadius = AI1.GetComponent<SphereCollider>().radius;
        maxRopeLength = 5; //get max rope length
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(1.0f / Time.deltaTime);
        currentEvaluation = Mathf.NegativeInfinity;
        currentAI1Direction = Vector3.zero;
        currentAI2Direction = Vector3.zero;
        maxSpeed = player1.GetComponent<PlayerController>().maxSpeed;

        //Calculate next step for enemy players
        futurePlayer1Position = player1.transform.position + player1.GetComponent<Rigidbody>().velocity * Time.deltaTime;
        futurePlayer2Position = player2.transform.position + player2.GetComponent<Rigidbody>().velocity * Time.deltaTime;

        //Find the closest player to the 2 AI players
        Vector3 averageAIPosition = (AI1.transform.position + AI2.transform.position) / 2;
        GameObject closestPlayer = (Vector3.Distance(averageAIPosition, futurePlayer1Position) < Vector3.Distance(averageAIPosition, futurePlayer2Position)) ? player1 : player2;

        //Maximize distance from AI players and closest point to player rope and minimize distance from closest player to the AI rope
        //For now check cardinal directions, more directions might be checked in the future for more accurate AI movement

        bool validMove;
        for (int i = 0; i < 16; i++)
        {
            validMove = true;
            futureAI1Position = AI1.transform.position + Quaternion.Euler(0, i * 22.5f, 0) * Vector3.forward * maxSpeed * Time.deltaTime;

            //Check if AI1 will collide with a wall or will be above a hole
            Vector3 newAI1Direction = futureAI1Position - AI1.transform.position;
            Vector3 normalizedAI1Direction = Vector3.Normalize(newAI1Direction);
            List<Vector3> AI1Positions = new List<Vector3>();
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, -90, 0) * normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, -45, 0) * normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, -22.5f, 0) * normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, 22.5f, 0) * normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, 45, 0) * normalizedAI1Direction * playerRadius);
            AI1Positions.Add(AI1.transform.position + Quaternion.Euler(0, 90, 0) * normalizedAI1Direction * playerRadius);
            foreach (Vector3 pos in AI1Positions)
            {
                RaycastHit[] hitInfo;
                hitInfo = Physics.RaycastAll(pos, newAI1Direction, Vector3.Magnitude(newAI1Direction));
                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Hazzard"))
                    {
                        validMove = false;
                        break;
                    }
                }
                if (validMove)
                {
                    hitInfo = Physics.RaycastAll(pos + newAI1Direction, Vector3.down);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.CompareTag("Hazzard"))
                        {
                            validMove = false;
                            break;
                        }
                    }
                }
                if (!validMove)
                {
                    break;
                }
            }
            if (!validMove)
            {
                continue;
            }
            for (int j = 0; j < 16; j++)
            {
                futureAI2Position = AI2.transform.position + Quaternion.Euler(0, j * 22.5f, 0) * Vector3.forward * maxSpeed * Time.deltaTime;

                //Check if AI2 will collide with a wall or will be above a hole
                Vector3 newAI2Direction = futureAI2Position - AI2.transform.position;
                Vector3 normalizedAI2Direction = Vector3.Normalize(newAI2Direction);
                List<Vector3> AI2Positions = new List<Vector3>();
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, -90, 0) * normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, -45, 0) * normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, -22.5f, 0) * normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, 22.5f, 0) * normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, 45, 0) * normalizedAI2Direction * playerRadius);
                AI2Positions.Add(AI2.transform.position + Quaternion.Euler(0, 90, 0) * normalizedAI2Direction * playerRadius);
                foreach (Vector3 pos in AI2Positions)
                {
                    RaycastHit[] hitInfo;
                    hitInfo = Physics.RaycastAll(pos, newAI2Direction, Vector3.Magnitude(newAI2Direction));
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Hazzard"))
                        {
                            validMove = false;
                            break;
                        }
                    }
                    if (validMove)
                    {
                        hitInfo = Physics.RaycastAll(pos + newAI2Direction, Vector3.down);
                        foreach (RaycastHit hit in hitInfo)
                        {
                            if (hit.collider.CompareTag("Hazzard"))
                            {
                                validMove = false;
                                break;
                            }
                        }
                    }
                    if (!validMove)
                    {
                        break;
                    }
                }
                if (!validMove)
                {
                    continue;
                }

                //Both moves are valid, check if combination is good
                float newEvaluation = Evaluate(futureAI1Position, futureAI2Position, (closestPlayer == player1) ? futurePlayer1Position : futurePlayer2Position, (closestPlayer != player1) ? futurePlayer1Position : futurePlayer2Position);
                if (newEvaluation > currentEvaluation)
                {
                    currentEvaluation = newEvaluation;
                    currentAI1Direction = newAI1Direction;
                    currentAI2Direction = newAI2Direction;
                }
            }
        }
        //AI1.GetComponent<Rigidbody>().AddForce(currentAI1Direction.normalized * maxSpeed, ForceMode.Impulse);
        AI1.GetComponent<Rigidbody>().velocity = currentAI1Direction.normalized * maxSpeed;
        AI2.GetComponent<Rigidbody>().velocity = currentAI2Direction.normalized * maxSpeed;
        /**/
    }

    float Evaluate(Vector3 AI1Position, Vector3 AI2Position, Vector3 closestPlayerPosition, Vector3 farthestPlayerPosition)
    {
        float AI1DistanceToRope = DistanceFromPointToLine(AI1Position, closestPlayerPosition, Vector3.Normalize(closestPlayerPosition - farthestPlayerPosition)); //Good
        float AI2DistanceToRope = DistanceFromPointToLine(AI2Position, closestPlayerPosition, Vector3.Normalize(closestPlayerPosition - farthestPlayerPosition)); //Good
        float closestPlayerDistanceToRope = DistanceFromPointToLine(closestPlayerPosition, AI1Position, Vector3.Normalize(AI1Position - AI2Position)); //Bad?
        float AI1DistanceToMidpoint = Vector3.Distance((closestPlayerPosition + farthestPlayerPosition) / 2, AI1Position);
        float AI2DistanceToMidpoint = Vector3.Distance((closestPlayerPosition + farthestPlayerPosition) / 2, AI1Position);
        float closestPlayerDistanceToMidpoint = Vector3.Distance((AI1Position + AI2Position) / 2, closestPlayerPosition);
        float angle = Mathf.Abs(90 - Mathf.Abs(Vector3.Angle((AI1Position + AI2Position) / 2 - closestPlayerPosition, AI1Position - AI2Position)));
        return 50 / closestPlayerDistanceToRope + 100 / closestPlayerDistanceToMidpoint + ((Vector3.Magnitude(AI1Position - AI2Position) < maxRopeLength) ? 0.1f * (AI1DistanceToRope + AI2DistanceToRope + AI1DistanceToMidpoint + AI2DistanceToMidpoint) : -20 * Vector3.Magnitude(AI1Position - AI2Position)) - angle;
    }

    float DistanceFromPointToLine(Vector3 point, Vector3 pointOnLine, Vector3 line)
    {
        return Vector3.Magnitude((pointOnLine - point) - Vector3.Dot((pointOnLine - point), line) * line);
    }
}
