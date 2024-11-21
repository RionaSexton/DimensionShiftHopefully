using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMistAttack : MonoBehaviour
{
   public Animator myAnim;

   void OnTriggerEnter2D(Collider2D col){

        if(col.gameObject.tag == "Player"){
            myAnim.SetTrigger("Mist");
        }
   }
}
