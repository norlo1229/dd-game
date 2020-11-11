using UnityEngine;

public class Dice
{
    public static int Roll(int sides, int number)
    {
        int final_value = 0;

        for(int i = 0; i < number; i++)
        {
            final_value += Random.Range(1, sides);
        }

        return final_value;
    }
}
