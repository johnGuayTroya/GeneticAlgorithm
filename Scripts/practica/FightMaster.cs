using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class FightMaster : MonoBehaviour
{
    bool ready;
    [Range(10, 20)]
     float healthPoints;
    [Range(100, 200)]
     float energy;
    [Range(0,1)]
     float hitChance;
    [Range(0, 3)]
     float minDamage;
    [Range(3, 8)]
     float maxDamage;

     public bool GetReady{ get { return ready; } set { ready = value; } }

    [Header("Genetic params")]
    public int numberGeneration;
    public int populationSize;


    public FightGen geneticfighter;
    public Fighter  currentFighter;

    private GameLogic gameL;
    private PlayerList fighters;
    public GameObject[] actionBtn; //0 soft,1 Dummy,2 hard,3 Rest

    private void Start()
    {
        gameL = FindObjectOfType<GameLogic>();
        fighters = gameL.PlayerList;
        actionBtn = new GameObject[4];
       
        geneticfighter = new FightGen(numberGeneration,populationSize);
        //Time.timeScale = 50f;

        StartCoroutine(SearchBtn());
        

    }
    
    public void GetResult(float data)
    {
        Debug.Log($"Result {data}");
        currentFighter.fitness = data;
        ready = true;
    }

    public void StarCombat()
    {
        
       
        ready = true;
        ChooseFighter();
        StartCoroutine(StartCmb());
    
    }
  public  IEnumerator StartCmb()
    {

        while (ready)
        {
            
            ComboAction();
            yield return null;
        }

    }

    //elegimos a nuestro luchador,en caso de no haber mñas terminamos el programa
    void ChooseFighter()
    {
        currentFighter = geneticfighter.GetNext();
        if (currentFighter != null)
        {
            //lo que hacemos es pasar las estadístics de nuestros individuos a nuestro actual luchador
            fighters.Players[1].HP = currentFighter.heatlhPoint;
            fighters.Players[1].Energy = currentFighter.energy;
            //asignamos las variables de ataque de nuestro luchador de aquellas acciones que el individuo puede realizar
            for(byte i = 0; i < currentFighter.actions.Length; ++i)
            {
                if (currentFighter.actions[i] == 1)
                {
                    fighters.Players[1].Attacks[i].HitChance = currentFighter.hitChance;
                    fighters.Players[1].Attacks[i].MinDam = currentFighter.minDam;
                    fighters.Players[1].Attacks[i].MaxDam = currentFighter.maxDam;
                }
            }
            
        }
        else
        {

            currentFighter = geneticfighter.GetFittest();
            ready = false;
        }
    }

    //HAY QUE PULIR ESTO PARA QUE HAGAMOS MAs DAÑO QUOZA MUTACION DE FUERZA ETC
    //QUE EN CADA TIPO DE DAÑO DE MÑAS POTENCIA ETC
    //cogemos de nuestro individuo aquellos ataques que puede efecturar
    void ComboAction()
    {
        for(byte j = 0; j < 4; ++j)
        {
            var att = actionBtn[j].GetComponent<ActionButtonAttack>();
            
            // si nuestro luhcador posee esa habilidad la ejecuta
            if (currentFighter.actions[j] == 1)
            {
                //si se queda sin energia pasamos al siguiente
                if (fighters.Players[1].Energy < 0) { gameL.UselessChampion(); }
                else { att.OnClick(); } 
                
            }

        }
       
    }
    
    
    //buscamos los botones de control
    IEnumerator SearchBtn()
    {
        string gameobj = "Soft-Dummy-Hard-Rest";
        string[] cache = gameobj.Split('-');
        var wait = new WaitForSeconds(0.5f);
 
        yield return wait;
        for (int i = 0; i < actionBtn.Length; i++)
        {
            actionBtn[i] = GameObject.Find(cache[i]);
            actionBtn[i].GetComponent<Button>().image.enabled = false;
            actionBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }


        yield return wait;
        StarCombat();

    }

}
