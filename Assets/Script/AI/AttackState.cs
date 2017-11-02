using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private GameObject test;
    private GameObject test2;

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
        //test2 = GameObject.Instantiate(test);
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
        // Get the minimum length of points on AI rope line to both enemies
        Vector3 point1, point2, closer, point;
        float dist1 = Vector3.Distance(enemy1Pos, point1 = ClosestPointOnTeamRope(enemy1Pos));
        float dist2 = Vector3.Distance(enemy2Pos, point2 = ClosestPointOnTeamRope(enemy2Pos));
        if(dist1 <= dist2)
        {
            distAiRopeToEnemy = dist1;
            closer = enemy1Pos;

            if (dist1 <= 0)
                point = enemy1Pos;
            else
                point = point1;
            Debug.Log("Point 1");
        }
        else
        {
            distAiRopeToEnemy = dist2;
            closer = enemy2Pos;

            if (dist1 <= 0)
                point = enemy2Pos;
            else
                point = point2;
            Debug.Log("Point 2");
        }
        //distAiRopeToEnemy = Mathf.Min(dist1,dist2);

        // Set test object position (Not needed in final version)
        if (Vector3.Distance(point1, enemy1Pos) <= Vector3.Distance(point2, enemy2Pos))
            test.transform.position = point1;
        else
            test.transform.position = point2;

        distAi1ToEnemyRope = Vector3.Distance(AI1Pos, ClosestPointOnEnemyRope(AI1Pos));
        distAi2ToEnemyRope = Vector3.Distance(AI2Pos, ClosestPointOnEnemyRope(AI2Pos));



        Vector3 dir1 = closer - point;

        Rigidbody rb = team.player1.GetComponent<Rigidbody>();
        Rigidbody rb2 = team.player2.GetComponent<Rigidbody>();

        Vector3 target1 = new Vector3(Math.Sign(dir1.x), 0, Math.Sign(dir1.z)).normalized * 40;
        Vector3 accel1 = new Vector3((target1.x - rb.velocity.x) * .25f, 0, (target1.z - rb.velocity.z) * .25f);
        accel1.y = 0;

        Vector3 target2 = new Vector3(Math.Sign(dir1.x), 0, Math.Sign(dir1.z)).normalized * 40;
        Vector3 accel2 = new Vector3((target2.x - rb2.velocity.x) * .25f, 0, (target2.z - rb2.velocity.z) * .25f);
        accel2.y = 0;

        Debug.Log(closer + " and " + point);

        rb.AddForce(accel1, ForceMode.VelocityChange);
        rb2.AddForce(accel2, ForceMode.VelocityChange);
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