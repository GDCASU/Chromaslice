using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private GameObject test;

    // Positions of all players
    Vector3 AI1Pos;
    Vector3 AI2Pos;
    Vector3 enemy1Pos;
    Vector3 enemy2Pos;

    private float distAi1ToEnemyRope;
    private float distAi2ToEnemyRope;
    private float distAiRopeToEnemy;

    public AttackState(GameObject t)
    {
        test = t;
    }

    public override void Start(StateMachine sm)
    {
        team = sm.GetTeam();
        enemyTeam = sm.GetEnemyTeam();
    }

    public override void Update(StateMachine sm)
    {
        // Set positions of players
        AI1Pos = team.player1.transform.position;
        AI2Pos = team.player2.transform.position;
        enemy1Pos = enemyTeam.player1.transform.position;
        enemy2Pos = enemyTeam.player2.transform.position;

        EvaluateDistances();
    }

    public override void Exit(StateMachine sm)
    {

    }

    public override State EvaluateTransitions(StateMachine sm)
    {
        return null;
    }

    private void EvaluateDistances()
    {
        // Get the closer player to the AI (Don't know if this is needed or not)

        //GameObject closerPlayer;
        //float avgDist1 = Vector3.Distance(AI1Pos, enemy1Pos) + Vector3.Distance(AI2Pos, enemy1Pos);
        //float avgDist2 = Vector3.Distance(AI1Pos, enemy2Pos) + Vector3.Distance(AI2Pos, enemy2Pos);
        //if (avgDist1 <= avgDist2)
        //    closerPlayer = enemyTeam.player1;
        //else
        //    closerPlayer = enemyTeam.player2;

        // Get the minimum length of points on AI rope line to both enemies
        Vector3 point1, point2;
        float dist1 = Vector3.Distance(enemyTeam.player1.transform.position, point1 = ClosestPointOnTeamRope(enemyTeam.player1.transform.position));
        float dist2 = Vector3.Distance(enemyTeam.player2.transform.position, point2 = ClosestPointOnTeamRope(enemyTeam.player2.transform.position));
        distAiRopeToEnemy = Mathf.Min(dist1,dist2);
        Debug.Log("Distance: " + distAiRopeToEnemy);

        // Set test object position (Not needed in final version)
        if (Vector3.Distance(point1, enemyTeam.player1.transform.position) <= Vector3.Distance(point2, enemyTeam.player2.transform.position))
            test.transform.position = point1;
        else
            test.transform.position = point2;

        dist1 = Vector3.Distance(AI1Pos, point1 = ClosestPointOnEnemyRope(AI1Pos));
        dist2 = Vector3.Distance(AI2Pos, point2 
            = ClosestPointOnEnemyRope(AI2Pos));
        float distEnemyRopeToAi = Mathf.Min(dist1, dist2);

       // GameObject.Instantiate(test, )

    }

    // Returns the point on the rope between the AI to the closer player
    private Vector3 ClosestPointOnTeamRope(Vector3 point)
    {
        Vector3 v1 = point - AI1Pos;
        Vector3 v2 = (AI2Pos - AI1Pos).normalized;

        float d = Vector3.Distance(AI1Pos, AI2Pos);
        float t = Vector3.Dot(v2, v1);

        if (t <= 0)
            return AI1Pos;
        if (t >= d)
            return AI2Pos;

        Vector3 v3 = v2 * t;
        return AI1Pos + v3;
    }

    // Returns the point on the rope between the AI to the closer player
    private Vector3 ClosestPointOnEnemyRope(Vector3 point)
    {
        Vector3 v1 = point - enemy1Pos;
        Vector3 v2 = (enemy2Pos - enemy1Pos).normalized;

        float d = Vector3.Distance(enemy1Pos, enemy2Pos);
        float t = Vector3.Dot(v2, v1);

        if (t <= 0)
            return enemy1Pos;
        if (t >= d)
            return enemy2Pos;

        Vector3 v3 = v2 * t;
        return enemy1Pos + v3;
    }
}