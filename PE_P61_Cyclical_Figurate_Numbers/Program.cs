// Author: James Norcross
// Date 04/09/16
// Purpose: Project Euler Problem 61
// Description:  Finds sum of 6 element cyclic set of 4 digit numbers one each from following figurate series; 
// triangle, square, pentagonal, hexagonal, heptagonal, octagonal (see series definitions in functions)

using System;
using System.Collections.Generic;


namespace PE_P61_Cyclical_Figurate_Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            /* populate 2D array 100 x 6 
             * first dimension i represents the possible values for first 2 digits of 4 digit number
             * second dimension j represents the 6 'orders'; triangle, square, pentagonal, hexagonal, heptagonal, octagonal
             * array elements are list of all numbers of order j that have first digits i
             * 
             * note: had to use lists for array elements because in some cases have multiple order j numbers with 
             * first 2 digits i
             */
            List<int>[,] firstDigitsArray = FillFirstDigitsArray();

            // solutions is a list of individual solution
            // each solution is a 2 element array with solution[0] being an individual solution list and
            // solution[1] being the list of orders associated with solution[0]
            List<List<int>[]> solutions = new List<List<int>[]>();

            // create initial solution sets from items in Triangle row
            // note can start solutions with any 'order' since final solution must be cyclic
            for (int i = 10; i < 100; i++)
            {
                foreach (int number in firstDigitsArray[i, 0])
                {
                    List<int>[] solution = new List<int>[2];
                    solution[0] = new List<int>();
                    solution[1] = new List<int>();
                    solution[0].Add(number);
                    solution[1].Add(0);
                    solutions.Add(solution);
                }
            }

            // go through each possible solution in solutions.  In each case check to find possible
            // 'nextElements' in the solution and make a list of these (including their orders)
            List<int> orders = new List<int>();
            List<int[]> nextElements = new List<int[]>();

            // build up all the possible solutions by repeatedly growing each one up to final
            // step where solutions will have 6 elements, one for each 'order'.  Since started with
            // order zero only have to loop for 5 more orders
            for (int k = 0; k < 5; k++)
            {

                int numSolutions = solutions.Count;

                // going to step through all individual solutions in solutions list
                for (int i = 0; i < numSolutions; i++)
                {

                    List<int>[] sol = solutions[0];

                    // find matchDigits for that solution, last 2 digits of last element of sol[0]
                    int matchDigits = LastDigits(sol[0][sol[0].Count - 1]);

                    // reinitialize orders
                    orders.Clear();
                    for (int j = 1; j < 6; j++)
                        orders.Add(j);

                    // reinitialize nextElements
                    nextElements.Clear();

                    // determine which orders to include in next possible list by removing those already used
                    foreach (int n in sol[1])
                    {
                        orders.Remove(n);
                    }

                    // create list of values that meet cyclic criteria.  The list will be list of 2 element
                    // arrays with the first element being the value and the second being the order
                    foreach (int order in orders)
                    {
                        foreach (int value in firstDigitsArray[matchDigits, order])
                        {
                            int[] nextElement = new int[2];
                            nextElement[0] = value;
                            nextElement[1] = order;
                            nextElements.Add(nextElement);
                        }
                    }

                    // if nextElements is not empty add the new solution corresponding to each
                    // next element to solutions list
                    if (nextElements.Count > 0)
                    {
                        foreach (int[] nE in nextElements)
                        {
                            List<int>[] solution = new List<int>[2];
                            List<int> solutionList = new List<int>(sol[0]);     // copy current sol since it is what nextElements will add to
                            List<int> ordersList = new List<int>(sol[1]);
                            solutionList.Add(nE[0]);
                            ordersList.Add(nE[1]);
                            solution[0] = solutionList;
                            solution[1] = ordersList;
                            solutions.Add(solution);
                        }
                    }

                    // remove the current solution from the solutions list since have already already
                    // added all solution derived from it to solutions list
                    solutions.Remove(sol);
                }
            }

            int result = 0;

            // find cyclic 6 element set and print it (and its sum)
            foreach (List<int>[] sol in solutions)
            {
                if (FirstDigits(sol[0][0]) == LastDigits(sol[0][sol[0].Count - 1]))
                {
                    Console.Write("Solution ");
                    foreach (int value in sol[0])
                    {
                        Console.Write("{0} ", value);
                        result += value;
                    }
                    Console.Write("Order ");
                    foreach (int order in sol[1])
                    {
                        Console.Write("{0} ", order);
                    }
                    Console.WriteLine();
                    Console.WriteLine("The sum of the 6 elements of the cyclic set is {0}", result);
                }
            }



            Console.ReadLine();
        }

       

        // fills array
        static List<int>[,] FillFirstDigitsArray()
        {
            List<int>[,] result = new List<int>[100, 6];

            // initialize array with empty lists
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    result[i, j] = new List<int>();
                }
            }

            // populate row 0 with triagonal numbers
            int n = 0;
            while (true)
            {
                int value = Triangle(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 0].Add(value);    // add triangle number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            // populate row 1 with square numbers
            n = 0;
            while (true)
            {
                int value = Square(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 1].Add(value);    // add square number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            // populate row 2 with pentagonal numbers
            n = 0;
            while (true)
            {
                int value = Pentagonal(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 2].Add(value);    // add pentagonal number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            // populate row 3 with hexagonal numbers
            n = 0;
            while (true)
            {
                int value = Hexagonal(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 3].Add(value);    // add hexagonal number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            // populate row 4 with heptagonal numbers
            n = 0;
            while (true)
            {
                int value = Heptagonal(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 4].Add(value);    // add hexagonal number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            // populate row 5 with octagonal numbers
            n = 0;
            while (true)
            {
                int value = Octagonal(n);

                if (value < 10000)
                {
                    if (value > 1000)
                    {
                        result[FirstDigits(value), 5].Add(value);    // add hexagonal number to list at array position corresponding to its first digits
                    }
                    n++;
                }
                else
                    break;
            }

            return result;
        }

        // calculates triangle number 
        static int Triangle(int n)
        {
            return (n * (n + 1)) / 2;
        }

        // calculates square number
        static int Square(int n)
        {
            return n * n;
        }

        // calculates pentagonal number
        static int Pentagonal(int n)
        {
            return (n * ((3 * n) - 1)) / 2;
        }

        // calculates hexagonal number
        static int Hexagonal(int n)
        {
            return n * ((2 * n) - 1);
        }

        // calculates heptagonal number
        static int Heptagonal(int n)
        {
            return (n * ((5 * n) - 3)) / 2;
        }

        // calculates octagonal number
        static int Octagonal(int n)
        {
            return n * ((3 * n) - 2);
        }

        // finds first 2 digits of four digit number n
        static int FirstDigits(int n)
        {
            return n / 100;
        }

        // returns last 2 digits of number
        static int LastDigits(int n)
        {
            return n % 100;
        }
    }
}
