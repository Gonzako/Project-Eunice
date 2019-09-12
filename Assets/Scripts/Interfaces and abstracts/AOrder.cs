using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AOrder : IOrder
{
    public abstract OrderResult Perform(GameObject actor, GameObject target);

    public abstract OrderResult Perform(GameObject actor);

}
