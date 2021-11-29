using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FightGen 
{

    public List<Fighter> myfighters;

    public int currentGeneration;
    public int maxGeneration;

    public string Summary;

    private int index;
    public FightGen(int numberOfGenerations, int populationSize)
    {
        currentGeneration = 0;
        maxGeneration = numberOfGenerations;
        GenerateRandomPopulation(populationSize);
        Summary = "";
    }


    //creamos a nuestros luchadores con estadsiticas aleatorias
    public void GenerateRandomPopulation(int size)
    {
        myfighters = new List<Fighter>();
        for (int i = 0; i < size; i++)
        {
            myfighters.Add(new Fighter
                (
                Random.Range(10,21),
                Random.Range(100,201),
                Random.Range(0.0f,1.001f),
                Random.Range(0,4),
                Random.Range(3,9),
                RandomActions()));
        }
        StartGeneration();
    }

    // //0 soft,1 Dummy,2 hard,3 Rest
    int[] RandomActions()
    {
        int[] rand= new int[4];
        //generamos valores entre 0 y 1,0 no se realiza uno si
        for(byte i = 0; i < 4; ++i)
        {
            rand[i] = Random.Range(0, 2);
        }

        return rand;
    
    }

    public void StartGeneration()
    {
        index = 0;
        currentGeneration++;
    }

    public Fighter GetNext()
    {
        if (index == myfighters.Count)
        {
            EndGeneration();
            if (currentGeneration >= maxGeneration)
            {
                Debug.Log(Summary);
                return null;
            }
            StartGeneration();
        }
        //devolvemos el siguiente de nuestra lista
        return myfighters[index++];
    }


    //ordenamos a nuestros luchadores  y devolmeos al mejor
    public Fighter GetFittest()
    {
        myfighters.Sort();
       //cojo el ultimo porque es con el que mayor vida me quedo
        return myfighters[myfighters.Count-1];
    }
    public void EndGeneration()
    {
        //ordenamos la lista de población,este mñetodo pone los mejores valores los primeros
        myfighters.Sort();
        //myfighters.Reverse();
        Summary += $"{GetFittest().fitness};";
        if (currentGeneration < maxGeneration)
        {
            //Crossover(); //intercambiar los dos mejores y mezclar sus acciones 
            //Mutation();  
            CrucePlano();
        }
    }


    private void CrucePlano()
    {
        //selección
        //cogemos a nuestros dos mejores
        Fighter ind1 = myfighters[myfighters.Count-1];
        Fighter ind2 = myfighters[myfighters.Count - 2];

         //mezclamos sus estats

        //Cruce Plano
        //sacamos un intervalo de vida
        float healPointMax = Math.Max(ind1.heatlhPoint, ind2.heatlhPoint);
        float healPointMin = Math.Min(ind1.heatlhPoint, ind2.heatlhPoint);
        //sacamos un intervalor de energia
        float energyMax = Math.Max(ind1.energy, ind2.energy);
        float energtMin = Math.Min(ind1.energy, ind2.energy);
        //de hitC
        float maxHc = Math.Max(ind1.hitChance, ind2.hitChance);
        float minHc = Math.Min(ind1.hitChance, ind2.hitChance);
        //de porcentajes
        float minD = Math.Min(ind1.minDam, ind2.minDam);
        float maxD = Math.Max(ind1.maxDam, ind2.maxDam);
        //cogemos valores aleatorios de habilidades
        int[] newActions = new int[4];
        for (byte i = 0; i < 4; ++i)
        {
            int rand = Random.Range(0, 11);
            if (rand < 6) { newActions[i] = ind1.actions[i]; }
            else { newActions[i] = ind1.actions[i]; }
        }

        var new1 = new Fighter
          (Random.Range(healPointMin, healPointMax),
           Random.Range(energtMin, energyMax),
           Random.Range(minHc, maxHc),
           (int)minD,
           (int)maxD,
           newActions
          ); 


        //Operador de reemplazo
        WorstReplacement(new1);
  
    }

    //eliminamos los dos últimos y los reemplazamos por dos nuevos
    void WorstReplacement(Fighter new1)
    {

        myfighters.RemoveAt(0);
        //añadimos los nuevos al final
        myfighters.Add(new1);
      

    }


}

