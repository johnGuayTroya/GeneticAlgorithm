using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GeneticAlgorithm
{
   
    public List<Individual> population;
    private int _currentIndex;

    public int CurrentGeneration;
    public int MaxGenerations;

    public string Summary;

    private bool[] algoritmoDeCruce;      // 0 apagado 1 encendido
    private bool[] algoritmoDeMutacion;  // 0 apagado 1 encendido
    public GeneticAlgorithm(int numberOfGenerations, int populationSize,bool []cruce,bool []mutacion)
    {
        
        algoritmoDeCruce = new bool[cruce.Length];
        algoritmoDeMutacion = new bool[mutacion.Length];
        for(Int16 i = 0; i < cruce.Length; i++) { algoritmoDeCruce[i] = cruce[i]; }
        for(Int16 j = 0; j < mutacion.Length; j++) { algoritmoDeMutacion[j] = mutacion[j]; }

        CurrentGeneration = 0;
        MaxGenerations = numberOfGenerations;
        GenerateRandomPopulation(populationSize);
        Summary = "";
    }
    public void GenerateRandomPopulation(int size)
    {
        population = new List<Individual>();
        for (int i = 0; i < size; i++)
        {
            population.Add(new Individual(Random.Range(10f,90f),Random.Range(-30f, 30f), Random.Range(3f, 30f)));
        }
        StartGeneration();
    }

    public Individual GetFittest()
    {
        population.Sort();
        return population[0];
    }

    public void StartGeneration()
    {
        _currentIndex = 0;
        CurrentGeneration++;
    }

    public Individual GetNext()
    {
        if (_currentIndex == population.Count)
        {
            EndGeneration();
            if (CurrentGeneration >= MaxGenerations)
            {
                Debug.Log(Summary);
                return null;
            }
            StartGeneration();
        }
        //devolvemos el siguiente de nuestra lista
        return population[_currentIndex++];
    }

    public void EndGeneration()
    {
        //ordenamos la lista de población,este mñetodo pone los mejores valores los primeros
        population.Sort();
        Summary += $"{GetFittest().fitness};";
        if (CurrentGeneration < MaxGenerations)
        {
            Crossover();
            Mutation();
        }
    }


    public void Crossover() 
    {
      if     (algoritmoDeCruce[0])   { CrucePlano(); }
      else if(algoritmoDeCruce[1])   { CruceAritmetico(0.4f,0.7f); }
      else if(algoritmoDeCruce[2])   { CrucePlanoBLXA(); }
    }

    public void Mutation()
    {

        if      (algoritmoDeMutacion[0]) { MutacionUniforme(0.02f, 0.02f, 0.05f); }
        else if (algoritmoDeMutacion[1]) { MutacionPorIntercambio(); }
       
    }

    #region AlgoritmoDeCruce
    private void CrucePlano()
    {
        //selección
        Individual ind1 = population[0];
        Individual ind2 = population[1];

        //Cruce Plano
        //necesitamos sacar el min y máximo para luego hacer el random
        float minDegX = Math.Min(ind1.degreeX, ind2.degreeX);
        float maxDegX = Math.Max(ind1.degreeX, ind2.degreeX);
        float minDegY = Math.Min(ind1.degreeY, ind2.degreeY);
        float maxDegY = Math.Max(ind1.degreeY, ind2.degreeY);
        float minStr = Math.Min(ind1.strength, ind2.strength);
        float maxStr = Math.Max(ind1.strength, ind2.strength);

        var new1 = new Individual
          (
           Random.Range(minDegX, maxDegX),
           Random.Range(minDegY, maxDegY),
           Random.Range(minStr, maxStr)
          );

        var new2 = new Individual
          (
           Random.Range(minDegX, maxDegX),
           Random.Range(minDegY, maxDegY),
           Random.Range(minStr, maxStr)
          );

        //Operador de reemplazo
        WorstReplacement(new1, new2);
        //JudgmentDay(population.Count);
    }

    //el r puede ir entre dos valores que q
    private void CruceAritmetico(float r1, float r2)
    {


        //seleccion
        Individual ind1 = population[0];
        Individual ind2 = population[1];

        ////Cruce aritmético.

        float cruceA_Xd = ind1.degreeX;
        float cruceA_Yd = ind1.degreeY;
        float cruceA_Str = ind1.strength;

        float cruceB_Xd = ind2.degreeX;
        float cruceB_Yd = ind2.degreeY;
        float cruceB_Str = ind2.strength;

        float aFinalXd = (cruceA_Xd * r1) + (cruceB_Xd * r2);
        float aFinalYd = (cruceA_Yd * r1) + (cruceB_Yd * r2);
        float aFinalStr = (cruceA_Str * r1) + (cruceB_Str * r2);

        float bFinalXd = (cruceA_Xd * r2) + (cruceB_Xd * r1);
        float bFinalYd = (cruceA_Yd * r2) + (cruceB_Yd * r1);
        float bFinalStr = (cruceA_Str * r2) + (cruceB_Str * r1);


        var new1 = new Individual(aFinalXd, aFinalYd, aFinalStr);
        var new2 = new Individual(bFinalXd, bFinalYd, bFinalStr);


        //REEMPLAZO
        WorstReplacement(new1, new2);


    }

    private void CrucePlanoBLXA()
    {
        //SELECCION
        var ind1 = population[0];
        var ind2 = population[1];
        //cruce  plano BLX-a
        //[Ai ʹ a*H ; Bi + a*H], donde H = |Ai – Bi|
        float degrees = 5f;
        float str = 2.5f;

        float minDegX = Math.Min(ind1.degreeX, ind2.degreeX) - degrees;
        float maxDegX = Math.Max(ind1.degreeX, ind2.degreeX) + degrees;
        float minDegY = Math.Min(ind1.degreeY, ind2.degreeY) - degrees;
        float maxDegY = Math.Max(ind1.degreeY, ind2.degreeY) + degrees;
        float minStr = Math.Min(ind1.strength, ind2.strength) - str;
        float maxStr = Math.Max(ind1.strength, ind2.strength) + str;


        //El valor del gen descendiente en la posición i se elige 
        //aleatoriamente entre el intervalo [Ai ʹ a*H ; Bi + a*H], donde H = |Ai – Bi|
        Individual new1 = new Individual(Random.Range(minDegX, maxDegX), Random.Range(minDegY, maxDegY), Random.Range(minStr, maxStr));
        Individual new2 = new Individual(Random.Range(minDegX, maxDegX), Random.Range(minDegY, maxDegY), Random.Range(minStr, maxStr));

        //REEMPLAZO
        WorstReplacement(new1, new2);

    
    }

    #endregion

    #region AlgoritmoMutacion

    /// <summary>
    ///  valorees de prueba son 0.02f
    /// </summary>
    /// <param name="degreXmut"></param>
    /// <param name="degreYmut"></param>
    /// <param name="strMut"></param>

    private void MutacionUniforme(float degreXmut, float degreYmut, float strMut)
    {
        //si el gen es real, se sustituye su valor por otro generado aleatoriamente dentro
        //del dominio de la variable
        foreach (Individual individual in population)
        {
            if (Random.Range(0f, 1f) < degreXmut) { individual.degreeX =  Random.Range (50, 61f);   } //50 y 60
            if (Random.Range(0f, 1f) < degreYmut) { individual.degreeY =  Random.Range (-8f, 1); } //-8 y 0
            if (Random.Range(0f, 1f) < strMut)    { individual.strength = Random.Range(20, 24); } //20 y 24

        }
    }

    /// <summary>
    /// recoremos y cambiamos los genes de los ángulos
    /// </summary>
    private void MutacionPorIntercambio()
    {
        float cache = 0;
        for(int i = 0; i < population.Count; ++i)
        {
          cache = population[i].degreeY;
          population[i].degreeY = population[i].degreeX;
          population[i].degreeX = cache;

        }
    
    }

    #endregion

    #region Operadores de reemplazo

    //eliminamos los dos últimos y los reemplazamos por dos nuevos
    void WorstReplacement(Individual new1,Individual new2)
    {
     
        population.RemoveAt(population.Count - 1);
        population.RemoveAt(population.Count - 1);

        //añadimos los nuevos al final
        population.Add(new1);
        population.Add(new2);

    }
    void JudgmentDay(int size)
    {
      
        //eliminamo a todos menos a uno
        for (int i = 1; i < size; i++)
        {
            population.RemoveAt(population.Count - 1);
        }

        //creamos nuevos individuos
        for (int j = 1; j < size; j++)
        {
            population.Add(new Individual(Random.Range(10f, 90f), Random.Range(-30f, 30f), Random.Range(3f, 30f)));
        }
        

    }



    #endregion
}





