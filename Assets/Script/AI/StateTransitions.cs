using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Description:     This is a ScriptableObject that holds the transitions from
 *                  a BaseState to another state. It also holds the priorities
 *                  of transitioning to a particular state. This is where
 *                  the transition conditions are evaluated
 * Version:         1.0.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */
 /* @author:        Diego Wilde
  * @date:          Feb 2, 2018
  * @description:   Moved priority control to State.cs from here, and removed and cleaned up 
  *                 the code that was in here
  **/
// Create the ScriptableObject
[CreateAssetMenu (menuName = "AI/StateTransitions")]

public class StateTransitions : ScriptableObject
{
    private List<Transition> AllTransitions;
    private State BaseState;

    //DW: removed - unused
    /**    private int[] Priorities;    **/

    // Set the data for creating and accessing the BaseState's transitions
    public void CreateStateTransitions(State baseState, List<State> allStates, List<Func<bool>> Conditions)
    {
        AllTransitions = new List<Transition>();
        BaseState = baseState;
        //DW: removed - unused
        /** Priorities = ReturnPriorities();    **/

        // Create the list of Transition objects
        // DW: edited to get priority array from State.cs
        for (int i = 0; i < allStates.Count; i++)
            AllTransitions.Add(new Transition(allStates[i], BaseState.priorities[i], Conditions[i]));
    }

    // Evaluate all the transitions for the state
    public State EvaluateTransitions()
    {
        // List that holds only the Transitions that are possible as specified by the condition
        List<Transition> validTransitions = new List<Transition>();
        foreach(Transition t in AllTransitions)
        {
            // Only conditions that evaluate TRUE are valid
            if (t.EvaluateTransition())
                validTransitions.Add(t);
        }

        // If no transition is valid, return null
        if (validTransitions.Count == 0)
            return null;

        // If only 1 transition is valid, return the state to transition to
        else if (validTransitions.Count == 1)
            return validTransitions[0].NextState;

        // If multiple transitions are possible, choose the one with the highest priority
        else
        {
            Transition highestPriority = new Transition();

            // Find the transition with the highest priority and return the next state
            for(int i = 0; i < validTransitions.Count; i++)
            {
                if (validTransitions[i].Priority < highestPriority.Priority)
                    highestPriority = validTransitions[i];
            }
            return highestPriority.NextState;
        }
    }

//DW: old way of determining priorities; no longer needed
/**

    // These are only sample priorities and should be adjusted later.
    // Currently the states should immediately switch to another state
    private int[] ReturnPriorities()
    {
        int[] priorities = null;

        if (BaseState is IdleState)
        {
            int[] temp = { 10, 1, 3 };
            priorities = temp;
        }
        else if (BaseState is AttackState)
        {
            int[] temp = { 10, 5, 3 };
            priorities = temp;
        }
        else if (BaseState is PowerUpState)
        {
            int[] temp = { 1, 3, 5 };
            priorities = temp;
        }

        return priorities;
    }
    **/
}