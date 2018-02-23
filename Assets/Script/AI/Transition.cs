using System;

/*
 * Description:     This class holds information about a particular transition to another state
 * Version:         1.0.0
 * Author:          Zachary Schmalz
 * Date:            11/9/2017
 */

public class Transition
{
    public State NextState;
    public int Priority;
    private Func<bool> TransitionCondition;

    // Create the transition and set the transition data
    public Transition(State nextState = null, int priority = 100, Func<bool> condition = null)
    {
        NextState = nextState;
        Priority = priority;
        TransitionCondition = condition;
    }

    // Execute the condition that this transition is related to
    // Valid conditions return TRUE, invalid conditions return FALSE
    public bool EvaluateTransition()
    {
        return TransitionCondition();
    }
}