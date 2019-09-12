using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface IOrder
{

    OrderResult Perform(GameObject actor);

}
