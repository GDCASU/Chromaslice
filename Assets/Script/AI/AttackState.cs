using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This class is the AttackState of the AI
 * Version:         1.1.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */

// Create the Scriptable Object
[CreateAssetMenu(menuName = "AI/States/Attack")]

public class AttackState : State
{
    // Positions of all players
    Vector3 AI1Pos;
    Vector3 AI2Pos;
    Vector3 enemy1Pos;
    Vector3 enemy2Pos;

    // Potentially useful distance values
    private float distAi1ToEnemyRope;
    private float distAi2ToEnemyRope;
    private float distAiRopeToEnemy;

    public override void Enter(StateMachine sm)
    {
        Debug.Log("Entering State: Attack");

        Conditions = new List<Func<bool>>() { Condition1, Condition2, Condition3 };
        StateTransitions.CreateStateTransitions(this, sm.AllStates, Conditions);

        team = sm.GetTeam();
        enemyTeam = sm.GetEnemyTeam();
    }

    public override void Execute(StateMachine sm)
    {
        // Set positions of players
        AI1Pos = team.player1.transform.position;
        AI2Pos = team.player2.transform.position;
        enemy1Pos = enemyTeam.player1.transform.position;
        enemy2Pos = enemyTeam.player2.transform.position;
    }

    public override void Exit(StateMachine sm)
    {
        Debug.Log("Exiting State: Attack");
    }

    // Condition for transitioning to IDLE STATE
    protected override bool Condition1()
    {
        Debug.Log("Evaluating Condition 1");
        return true;
    }

    // Condition for transitioning to ATTACK STATE
    protected override bool Condition2()
    {
        Debug.Log("Evaluating Condition 2");
        return true;
    }

    // Condition for transitioning to POWERUP STATE
    protected override bool Condition3()
    {
        Debug.Log("Evaluating Condition 3");
        return true;
    }

    private void EvaluateDistances()
    {
        // Honestly this is all probably just spaghetti code. 

        //// Get the minimum length of points on AI rope line to both enemies
        //Vector3 point1, point2, closer, point;
        //float dist1 = Vector3.Distance(enemy1Pos, point1 = ClosestPointOnTeamRope(enemy1Pos));
        //float dist2 = Vector3.Distance(enemy2Pos, point2 = ClosestPointOnTeamRope(enemy2Pos));
        //if(dist1 <= dist2)
        //{
        //    distAiRopeToEnemy = dist1;
        //    closer = enemy1Pos;

        //    if (dist1 <= 0)
        //        point = enemy1Pos;
        //    else
        //        point = point1;
        //    Debug.Log("Point 1");
        //}
        //else
        //{
        //    distAiRopeToEnemy = dist2;
        //    closer = enemy2Pos;

        //    if (dist1 <= 0)
        //        point = enemy2Pos;
        //    else
        //        point = point2;
        //    Debug.Log("Point 2");
        //}

        //distAi1ToEnemyRope = Vector3.Distance(AI1Pos, ClosestPointOnEnemyRope(AI1Pos));
        //distAi2ToEnemyRope = Vector3.Distance(AI2Pos, ClosestPointOnEnemyRope(AI2Pos));

        //// Move the AI
        //Vector3 closerAi, other;

        //if (distAi1ToEnemyRope <= distAi2ToEnemyRope)
        //{
        //    closerAi = AI1Pos;
        //    other = AI2Pos;
        //}
        //else
        //{
        //    closerAi = AI2Pos;
        //    other = AI1Pos;
        //}
        //float aiAndCloserAngle = Mathf.Atan2(closer.z - closerAi.z, closer.x - closerAi.x) * 180 / Mathf.PI;

        //Debug.Log(aiAndCloserAngle);

        //Vector3 dir1 = closer - point;

        //Rigidbody rb = team.player1.GetComponent<Rigidbody>();
        //Rigidbody rb2 = team.player2.GetComponent<Rigidbody>();

        //Vector3 target1 = new Vector3(Math.Sign(dir1.x), 0, Math.Sign(dir1.z)).normalized * 10;
        //Vector3 accel1 = new Vector3((target1.x - rb.velocity.x) * .25f, 0, (target1.z - rb.velocity.z) * .25f);
        //accel1.y = 0;

        //Vector3 target2 = new Vector3(Math.Sign(dir1.x), 0, Math.Sign(dir1.z)).normalized * 10;
        //Vector3 accel2 = new Vector3((target2.x - rb2.velocity.x) * .25f, 0, (target2.z - rb2.velocity.z) * .25f);
        //accel2.y = 0;

        

        //if (Math.Abs(aiAndCloserAngle) < 90 + 20 && Math.Abs(aiAndCloserAngle) > 90 - 20)
        //{
        //    Debug.Log("True");
        //    closer -= Vector3.forward * 5;


        //    Vector3 newTarget = new Vector3((closer.x - closerAi.x), 0, closer.z - closerAi.z).normalized * 10;
        //    Vector3 newAccel = new Vector3((newTarget.x - rb.velocity.x) * .25f, 0, (newTarget.z - rb.velocity.z) * .25f);
        //    newAccel.y = 0;
        //    rb.AddForce(newAccel, ForceMode.VelocityChange);
        //    rb2.AddForce(accel2, ForceMode.VelocityChange);
        //}
        //else
        //{
        //    rb.AddForce(accel1, ForceMode.VelocityChange);
        //    rb2.AddForce(accel2, ForceMode.VelocityChange);
        //}
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

    private void EvaluatePowerup()
    {
        float dist1 = Vector3.Distance(AI1Pos, ClosestPointOnTeamRope(AI1Pos));
        float dist2 = Vector3.Distance(AI2Pos, ClosestPointOnTeamRope(AI2Pos));


        if (team.GetComponent<Team>().CurrentPowerUp != null)
        {
            PowerUp p = team.GetComponent<Team>().CurrentPowerUp.GetComponent<PowerUp>();

            if (p is SpeedPowerUp)
            {
                if(dist1 > 5 || dist2 > 5)
				    p.Activate();
            }
            else if (p is InvincibilityPowerUp)
            {
                if (dist1 <= 2 || dist2 <= 2)
                    p.Activate();
            }
            else if (p is RopePowerUp)
            {
                if (dist1 > 5 || dist2 > 5)
                    p.Activate();
            }
            else if (p is KnockbackPowerUp)
            {
                if (dist1 <= 2 || dist2 <= 2)
                    p.Activate();
            }
        }
    }
}
