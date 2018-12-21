using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable1 : MonoBehaviour
{
    public Transform interactionLocation;
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];
    public ReactionCollection defoultReactionCollection;

    public void Interact()
    {
        for(int i = 0; i < conditionCollections.Length; i++)
        {
            if (conditionCollections[i].CheckAndReact()) return;
        }
        defoultReactionCollection.React();
    }
}