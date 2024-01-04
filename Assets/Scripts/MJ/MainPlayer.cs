using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    private int hp;
    private int tension;
    private int stamina;

    public int Hp
    { 
        get { return hp; } 
        set 
        {  
            hp = value; 
            if(hp <= 0)
            {
                ScenesManager.Instance.Die();
            }
        } 
    }



}
