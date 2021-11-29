using System;

[Serializable]
public class Individual : IComparable<Individual>
{
    //aquí están las caracteristicas que van a tenenr nuestros individuos

    public float degreeX;
    public float degreeY;
    public float strength;

    public float fitness;

    //Constructor
    public Individual(float dX,float dY,float s)
    {
        fitness = +1000f;
        degreeX = dX;
        degreeY = dY;
        strength = s;
    }

    public int CompareTo(Individual other)
    {
        return fitness.CompareTo(other.fitness);
    }
}
