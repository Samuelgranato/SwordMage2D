using UnityEngine;

public class ConditionAngry : Condition
{
    Transform agent;
    Transform target;
    float maxDist;

    public ConditionAngry(bool newgothit)
    {


    }

    public override bool Test()
    {
        return true;
    }


}