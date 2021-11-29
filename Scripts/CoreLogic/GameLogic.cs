using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public PlayerList PlayerList;

    public GameState GameState;

    public GameEvent EndGameEvent;
    public AttackResultEvent AttackResult;
    public PlayerEvent ChangeTurnEvent;

    private int _count = 0;

    private float _initialHPPlayerMax, _initialEnergyPlayerMax;

    private FightMaster fightM;

    public IEnumerator Start()
    {

        fightM = FindObjectOfType<FightMaster>();

        // Guardo la vida y energía inicial de cada uno
        _initialHPPlayerMax = PlayerList.Players[0].HP;
        _initialEnergyPlayerMax = PlayerList.Players[0].Energy;
        yield return null;

    }


    public void ChangeTurn()
    {
        if (fightM.GetReady != false)
        {
            var next = _count;
            _count = (_count + 1) % 2;
            GameState.CurrentPlayer = PlayerList.Players[next];
            ChangeTurnEvent.Raise(PlayerList.Players[next]);

        }

    }

    //si resulta mayor pues es un buen fitnees
    float CalculateFitnees()
    {
        if (PlayerList.Players[1].HP < 0)
        {
          return -PlayerList.Players[1].HP - PlayerList.Players[0].HP;
        }
        return PlayerList.Players[1].HP - PlayerList.Players[0].HP;
    }
    IEnumerator EndGameTest()
    {
       
        if (fightM.GetReady==true && PlayerList.Players[0].HP <= 0  ||
            fightM.GetReady==true && PlayerList.Players[1].HP <= 0)
        {
            var wait = new WaitForSeconds(3);
            Debug.LogWarning("Acabo la partida");

            //GameState.IsFinished = true;
            //EndGameEvent.Raise();

            //fightM.StopAllCoroutines();
            fightM.currentFighter.fitness = CalculateFitnees();

            fightM.GetReady = false;
          

            //0 la IA
            PlayerList.Players[0].HP = _initialHPPlayerMax;
            PlayerList.Players[0].Energy = _initialEnergyPlayerMax;

            yield return wait;
            fightM.StarCombat();
            

        }
       
    }
    
    public void OnAttackDone(Attack att)
    {
        if (!PlayerList.Players.Any(p => p.HP <= 0))
        {
            //Debug.Log($"Received Attack {att}");
            var hitRoll = Dice.PercentageChance();
            var result = ScriptableObject.CreateInstance<AttackResult>();
            result.IsHit = false;
            result.Attack = att;
            result.Energy = att.AttackMade.Energy;


            if (att.Source.Energy >= att.AttackMade.Energy && hitRoll <= att.AttackMade.HitChance)
            {
                result.IsHit = true;

                result.Damage = Dice.RangeRoll(att.AttackMade.MinDam, att.AttackMade.MaxDam + 1);


                att.Target.HP -= result.Damage;

            }

            if (att.Source.Energy >= att.AttackMade.Energy)
            {
                att.Source.Energy -= result.Energy;
            }

            //Debug.Log($"With Result \n    {result}");
            AttackResult.Raise(result);
            ChangeTurn();
        }

        StartCoroutine(EndGameTest());
        

    }
   public void UselessChampion()
   {
        //fightM.StopAllCoroutines();
        fightM.currentFighter.fitness = -1000;
        fightM.GetReady = false;
        //creamos otro enemigo
        PlayerList.Players[0].HP = _initialHPPlayerMax;
        PlayerList.Players[0].Energy = _initialEnergyPlayerMax;

        Debug.LogError("luchador no valido");
        
        fightM.StarCombat();
   }

}