using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IMovableAgent
{

    NavMeshAgent AgentComponent { get; set; }
    Vector3 DesiredLocation { get; set; }


};

