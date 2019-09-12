using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Selectable : MonoBehaviour, ISelectable
{

    #region Properties
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                if (isSelected)
                {
                    OnSelect();
                }
                else
                {
                    OnDeselect();
                }
            }

        }
    }
    #endregion

    #region Events
    public event Action OnThisSelected = delegate { };
    public event Action OnThisDeselected = delegate { };
    public event Action<RaycastHit> OnThisAction = delegate { };
    #endregion

    #region PrivateFields
    private bool isSelected;

    #endregion



    private void OnSelect()
    {
        OnThisSelected.Invoke();
        ClickSorter.OnAction += OnAction;

    }

    private void OnDeselect()
    {
        OnThisDeselected.Invoke();
        ClickSorter.OnAction -= OnAction;

    }

    private void OnAction(RaycastHit target)
    {
        OnThisAction.Invoke(target);

    }

    private void Start()
    {



    }

}
