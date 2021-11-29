
using System;

[Serializable]
public class Fighter : IComparable<Fighter>
{
    public float fitness;
    public float heatlhPoint;
    public float energy;

    public float hitChance;
    public int minDam;
    public int maxDam;


    public int[] actions;  //0 soft,1 Dummy,2 hard,3 Rest

    //Constructor
    //cuanto más alto sea el fitnes,la diferencia de vida entre ambos jugadores mejor será nuestro luchador
    public Fighter(float hp,float e,float hitC,int minD,int maxD,int []act)
    {
        energy = e;
        heatlhPoint = hp;
        hitChance = (float)(Math.Round(hitC,2));
        minDam = minD;
        maxDam = maxD;

        actions = new int[4];
        actions = act;

    }

    public int CompareTo(Fighter other)
    {
     
        return fitness.CompareTo(other.fitness);
    }

}
