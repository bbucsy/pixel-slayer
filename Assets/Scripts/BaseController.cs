using System.Collections;
using System.Collections.Generic;
using CharacterBehaviour;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public virtual void OnCharacterDeath()
    {
        this.enabled = false;
        
        var behaviours = gameObject.GetComponents<ICharacterBehaviour>();

        foreach (var script in behaviours)
        {
            var mono = (MonoBehaviour) script;
            Destroy(mono);
        }

        var characterCollider = GetComponent<Collider2D>();

        if (characterCollider !=  null) characterCollider.enabled = false;

        var characterBody = GetComponent<Rigidbody2D>();
        if(characterBody != null)
        {
            characterBody.simulated = false;
        }

        //Destroy(this);
    }
}