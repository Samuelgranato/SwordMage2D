using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDistRanWay : Condition
{
    Transform agent;
    Transform target;
    float maxDist;

    public ConditionDistRanWay(Transform ag, Transform ta, float dist)
    {
        agent = ag;
        target = ta;
        maxDist = dist;
    }

    public override bool Test()
    {
        return (Vector2.Distance(agent.position, target.position) > maxDist) ;
    }



}