﻿using System;
using System.Collections.Generic;


namespace ListsAndArrays
{
    class Program
    {
        static void Main()
        {
            //int[] numArray = new int[5];
            //numArray[0] = 5;
            //numArray[1] = 2;
            //numArray[2] = 10;
            //numArray[3] = 200;
            //numArray[4] = 5000;

            //int[] numArray1 = new int[] { 5, 2, 10, 200, 5000 };

            //int[] numArray2 = { 5, 2, 10, 200, 5000 };

            //numArray1[0] = 210;

            //Console.WriteLine(numArray[3]);
            //Console.ReadLine();

            List<int> intList = new List<int>();
            intList.Add(4);
            intList.Add(10);
            intList.Remove(10);

            //lists are much more adaptable then arrays. 

            byte[] byteArray = new byte[4000];

            Console.WriteLine(intList[0]);

            Console.ReadLine();

        }
    }
}
