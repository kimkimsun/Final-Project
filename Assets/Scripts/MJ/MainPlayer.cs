using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System.Runtime.InteropServices.WindowsRuntime;

public class MainPlayer : MonoBehaviour
{
    public FirstPersonController playerMove;
    private StateMachine<MainPlayer> playerSM;

    [SerializeField]
    private int hp;
    private int tension;
    [SerializeField]
    private float stamina;

    public float Stamina
    {
        get { return stamina; }
        set 
        { 
            stamina = value; 
            if(stamina >= 100)
            {
                stamina = 100;
                playerMove.MoveSpeed = 4.0f;
                playerMove.SprintSpeed = 6.0f;
            }
            if (stamina <= 0)
            {
                stamina = 0;
                playerMove.MoveSpeed = 2.5f;
                playerMove.SprintSpeed = 0f;
            }
        } 
    }
    public int Tension
    {
        get { return tension; }
        set 
        {
            tension = value;
            //텐션은 몬스터와 마주칠시 줄어들음
            if(tension <=50)
            {
                //UI 변화
                //심장소리
                playerSM.SetState("Exhaustion");
            }
        }
    }

    public int Hp
    { 
        get { return hp; } 
        set 
        {  
            hp = value;
            if (hp <=30)
            {
                playerSM.SetState("Moribund");
            }
            if (hp <= 0)
            {
                hp = 0;
                ScenesManager.Instance.Die();
            }
        } 
    }

    private void Start()
    {

        playerMove = GetComponent<FirstPersonController>();
        playerSM = new StateMachine<MainPlayer>();
        playerSM.owner = this;

        playerSM.AddState("Idle", new IdleState());
        playerSM.AddState("Exhaustion", new ExhaustionState());
        playerSM.AddState("Moribund", new MoribundState());
        playerSM.SetState("Idle");

        hp = 100;
        tension = 100;
        stamina = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerSM.SetState("Exhaustion");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerSM.SetState("Moribund");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerSM.SetState("Idle");
        }

    }


}
