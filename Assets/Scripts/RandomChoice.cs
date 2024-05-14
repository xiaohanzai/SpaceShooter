using System;
using System.Collections.Generic;

public class RandomChoice
{
    public static List<T> RandomSample<T>(T[] array, int n, bool replace = false)
    {
        if (n > array.Length && !replace)
        {
            throw new ArgumentException("Sample size larger than population size and replace is set to false.");
        }

        List<T> result = new List<T>();

        Random rand = new Random();

        if (replace)
        {
            for (int i = 0; i < n; i++)
            {
                int index = rand.Next(array.Length);
                result.Add(array[index]);
            }
        }
        else
        {
            List<T> tempList = new List<T>(array);

            for (int i = 0; i < n; i++)
            {
                int index = rand.Next(tempList.Count);
                result.Add(tempList[index]);
                tempList.RemoveAt(index);
            }
        }

        return result;
    }
}
