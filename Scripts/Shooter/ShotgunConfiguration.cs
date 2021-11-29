using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShotgunConfiguration : MonoBehaviour
{
    [Range(0, 90)]
    public float xDegrees;
    [Range(-30,30)]
    public float yDegrees;  // de -20 a 20

    public float strength;

    public Rigidbody ShotSpherePrefab;
    public Transform ShotPosition;

    public Transform Target;

    public GeneticAlgorithm Genetic;
    public Individual CurrentIndividual;

    [Header("2=>Cruce PlanoBLXA")]
    [Header("1=>Cruce Aritmetico")]
    [Header("0=>cruce Plano")]
    [Header("--Métodos de cruce------")]
    //representaremos en un array nuestros algoritmos disponibles
    //el 0 = CrucePlano y su estado pondrmeos a true o false
    public bool[] algoritmoDeCruce;      // 0 apagado 1 encendido
    [Header("1=>Mutacion Por Intercambio")]
    [Header("0=>Mutación uniforme")]
    [Header("--Métodos de mutación------")]
    public bool[] algoritmoDeMutacion;  // 0 apagado 1 encendido

    [Header("Genetic params")]
    public int numberGeneration;
    public int populationSize;

    private bool _ready;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 50f;

        Genetic = new GeneticAlgorithm(numberGeneration, populationSize, algoritmoDeCruce,algoritmoDeMutacion);
        
        _ready = true;
    }

    //cuando lo usa la IA
    public void IaShooter(float xD, float yD, float str)
    {
        xDegrees = xD;
        yDegrees = yD;
        strength = str;
    }

    public void GetResult(float data)
    {
        Debug.Log($"Result {data}");
        CurrentIndividual.fitness = data;  //ponemos el nuevo fitnees
        _ready = true;
    }

    public void Shot()
    {
        _ready = false;

        transform.eulerAngles = new Vector3(xDegrees, yDegrees,0);
        var shot = Instantiate(ShotSpherePrefab, ShotPosition);
        shot.gameObject.GetComponent<TargetTrigger>().Target = Target;
        shot.gameObject.GetComponent<TargetTrigger>().OnHitCollider += GetResult;
        shot.isKinematic = false;
        var force = transform.up * strength;
        shot.AddForce(force,ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            CurrentIndividual = Genetic.GetFittest();
            
            Shot();
        }

        if (_ready)
        {
            CurrentIndividual = Genetic.GetNext();
            if (CurrentIndividual != null)
            {
                IaShooter(CurrentIndividual.degreeX, CurrentIndividual.degreeY, CurrentIndividual.strength);
                Shot();
            }
            else
            {
                CurrentIndividual = Genetic.GetFittest();
                _ready = false;
            }
        }
    }
}
