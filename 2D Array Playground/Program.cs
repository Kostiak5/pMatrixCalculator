using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Array_Playground
{
    internal class Program
    {
        static int convertTesting(string inputStr, int upperLimit, int lowerLimit) //funkce pro prevod stringu na int; kontrola, ze uzivatel zadal vstup ve formatu int a ve spravnem rozmezi
        //inputStr = to, co chceme prevest na integer
        //pokud to uloha vyzaduje, zkontrolujeme, zda je int v rozmezi od lowerLimit do upperLimit (vetsinou kontrola, zda je promenna v rozmezi velikosti pole); pokud to uloha nevyzaduje, tuto funkci vyvolame s parametrem upperLimit = -1
        {
            int convertedNum = 0;

            while (!Int32.TryParse(inputStr, out convertedNum) || (upperLimit != -1 && (Int32.Parse(inputStr) >= upperLimit || Int32.Parse(inputStr) < lowerLimit))) //pokud uzivatel zada neciselnou hodnotu, prip. honde mimo rozmezi
            {
                if(!Int32.TryParse(inputStr, out convertedNum))
                {
                    Console.WriteLine("Neplatna hodnota, zadej hodnotu jeste jednou.");
                } else
                {
                    Console.WriteLine("Hodnota neodpovida rozmezi, zadej hodnotu jeste jednou.");
                }
                inputStr = Console.ReadLine();
            }
            
            convertedNum = Int32.Parse(inputStr);
            return convertedNum;
        }
        static void writeArray(int[,] arr, int x, int y) //funkce pro vypis matice
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.Write("\n");
            }
            return;
        }

        static int[,] fillArray(int[,] arr, int x, int y, int fillMethod) //funkce pro vyplneni matice cisly
        {
            Random rd = new Random();

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (fillMethod == 1)
                        arr[i, j] = rd.Next(100);
                    else
                        arr[i, j] = i * y + j + 1;
                }
            }

            writeArray(arr, x, y);
            return arr;
        }

        static int[,] createSecondArray(int[,] arr, int x, int yDefault, bool multiplication) //funkce pro nektere operace, pri kterych je potreba vytvorit druhou matici
        {
            int y = yDefault;
            //v pripade scitani a odcitani musi byt pocet radku a sloupcu obou matic identicky; v pripade nasobeni muze uzivatel libovolne nastavit pocet sloupcu nove matice
            if(multiplication) //tato promenna je 1, pokud jsme funkci vyvolali pro nasledne nasobeni matic
            {
                x = arr.GetLength(1);
                Console.WriteLine("Pocet radku nove matice = pocet sloupcu puvodni matice. (viz pravidla nasobeni matic)");
                Console.WriteLine("Zadej pocet sloupcu nove matice.");
                string yInput = Console.ReadLine();
                y = convertTesting(yInput, Int32.MaxValue, 0);
            }
            Console.WriteLine("Zadej 1, pokud chces novou matici naplnit nahodnymi cisly.");
            Console.WriteLine("Zadej cokoliv jineho, pokud chces matici naplnit za sebou jdoucimi cisly pocinaje jednickou.");
            int[,] secondArr = new int[x, y];
            string fillMethod = Console.ReadLine();

            Console.WriteLine("Nove pole: ");
            if (fillMethod == "1")
                fillArray(secondArr, x, y, 1);
            else
                fillArray(secondArr, x, y, 2);

            
            return secondArr;
        }

        static int[,] swapElement(int[,] arr, int x, int y)
        {
            Console.WriteLine("Zadej radek prvniho prvku, ktery chces prohodit.");
            string axInput = Console.ReadLine();
            int ax = convertTesting(axInput, x, 0);
            Console.WriteLine("Zadej sloupec prvniho prvku, ktery chces prohodit.");
            string ayInput = Console.ReadLine();
            int ay = convertTesting(ayInput, y, 0);

            Console.WriteLine("Zadej radek druheho prvku, ktery chces prohodit.");
            string bxInput = Console.ReadLine();
            int bx = convertTesting(bxInput, x, 0);
            Console.WriteLine("Zadej sloupec druheho prvku, ktery chces prohodit.");
            string byInput = Console.ReadLine();
            int by = convertTesting(byInput, y, 0);

            int temp = arr[ax, ay];
            arr[ax, ay] = arr[bx, by];
            arr[bx, by] = temp;

            Console.WriteLine("Vysledek: ");
            writeArray(arr, x, y);
            return arr;
        }

        static int[,] swapRow(int[,] arr, int x, int y)
        {
            Console.WriteLine("Zadej poradi prvniho radku, ktery chces prohodit.");
            string axInput = Console.ReadLine();
            int ax = convertTesting(axInput, x, 0);;

            Console.WriteLine("Zadej poradi druheho radku, ktery chces prohodit.");
            string bxInput = Console.ReadLine();
            int bx = convertTesting(bxInput, x, 0);

            for (int i = 0; i < y; i++)
            {
                int temp = arr[ax, i];
                arr[ax, i] = arr[bx, i];
                arr[bx, i] = temp;
            }

            Console.WriteLine("Vysledek: ");
            writeArray(arr, x, y);
            return arr;
        }

        static int[,] swapColumn(int[,] arr, int x, int y)
        {
            Console.WriteLine("Zadej poradi prvniho sloupce, ktery chces prohodit.");
            string ayInput = Console.ReadLine();
            int ay = convertTesting(ayInput, y, 0);

            Console.WriteLine("Zadej poradi druheho sloupce, ktery chces prohodit.");
            string byInput = Console.ReadLine();
            int by = convertTesting(byInput, y, 0);

            for (int i = 0; i < x; i++)
            {
                int temp = arr[i, ay];
                arr[i, ay] = arr[i, by];
                arr[i, by] = temp;
            }

            Console.WriteLine("Vysledek: ");
            writeArray(arr, x, y);
            return arr;
        }

        static int[,] swapDiagonal(int[,] arr, int x, int y, bool mainD) //funkce pro prohozeni prvku na hlavni nebo vedlejsi diagonale
        {
            if(x != y)
            {
                Console.WriteLine("Matice neni ctvercova, bohuzel tedy nelze otoceni prvku na diagonale provest.");
                return arr;
            }

            for (int i = 0; i <= x / 2; i++)
            {
                int reverseI = x - i - 1;
                if (mainD)
                {
                    int temp = arr[i, i];
                    arr[i, i] = arr[reverseI, reverseI];
                    arr[reverseI, reverseI] = arr[i, i];
                } else
                {
                    int temp = arr[i, reverseI];
                    arr[i, reverseI] = arr[reverseI, i];
                    arr[reverseI, i] = arr[i, reverseI];
                }
                
                
            }

            Console.WriteLine("Vysledek: ");
            writeArray(arr, x, y);
            return arr;
        }

        static int[,] multiplyByNumber(int[,] arr, int x, int y) //funkce pro nasobeni matice cislem
        {
            Console.WriteLine("Pokud chces vynasobit cislem pouze urcity radek, zadej 1.");
            Console.WriteLine("Pokud chces vynasobit cislem pouze urcity sloupec, zadej 2.");
            Console.WriteLine("Pokud chces vynasobit cislem celou matici, zadej cokoliv jineho.");
            string optionInput = Console.ReadLine();            

            Console.WriteLine("Zadej cislo, kterym budes nasobit.");
            string multiplierInput = Console.ReadLine();
            int multiplier = convertTesting(multiplierInput, -1, 0);

            if (optionInput == "1")
            {
                Console.WriteLine("Zadej poradi radku, ktery chces vynasobit timto cislem. Cisluje se od nuly (prvni radek = 0).");
                string rowInput = Console.ReadLine();
                int row = convertTesting(rowInput, x, 0);
                for (int i = 0; i < y; i++)
                {
                    arr[row, i] *= multiplier;
                }
            }
            else if (optionInput == "2")
            {
                Console.WriteLine("Zadej poradi sloupce, ktery chces vynasobit timto cislem. Cisluje se od nuly (prvni radek = 1).");
                string columnInput = Console.ReadLine();
                int column = convertTesting(columnInput, y, 0);
                for (int i = 0; i < x; i++)
                {
                    arr[i, column] *= multiplier;
                }
            }
            else
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        arr[i, j] *= multiplier;
                    }
                }
            }


            Console.WriteLine("Vysledek: ");
            writeArray(arr, x, y);
            return arr;
        }

        static int[,] addArrays(int[,] arr, int[,] secondArr, int x, int y)
        {
            int[,] sumArr = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    sumArr[i, j] = arr[i, j] + secondArr[i, j];
                }
            }

            Console.WriteLine("Vysledek: ");
            writeArray(sumArr, x, y);
            Console.WriteLine("Zadej ANO, pokud chces dal pocitat s touto vyslednou matici. Cokoliv jineho bude znamenat, ze dale budes pocitat znovu s puvodni matici.");
            if (Console.ReadLine() == "ANO")
                return sumArr;

            return arr;
        }

        static int[,] substractArrays(int[,] arr, int[,] secondArr, int x, int y)
        {
            int[,] subArr = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    subArr[i, j] = arr[i, j] - secondArr[i, j];
                }
            }

            Console.WriteLine("Vysledek: ");
            writeArray(subArr, x, y);
            Console.WriteLine("Zadej ANO, pokud chces dal pocitat s touto vyslednou matici. Cokoliv jineho bude znamenat, ze dale budes pocitat znovu s puvodni matici.");
            if (Console.ReadLine() == "ANO")
                return subArr;

            return arr;
        }

        static int[,] multiplyArrays(int[,] arr, int[,] secondArr, int x, int y)
        {
            int[,] multArr = new int[x, y];
            int z = arr.GetLength(1); //pocet sloupcu 1. matice a zaroven pocet radku 2. matice
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for(int k = 0; k < z; k++)
                    {
                        multArr[i, j] += (arr[i, k] * secondArr[k, j]); //kazdy z prvku vysledne matice je souctem nasobeni vsech prvku i-teho radku 1. matice a j-teho sloupce 2. matice
                    }
                }
            }

            Console.WriteLine("Vysledek: ");
            writeArray(multArr, x, y);
            Console.WriteLine("Zadej ANO, pokud chces dal pocitat s touto vyslednou matici. Cokoliv jineho bude znamenat, ze dale budes pocitat znovu s puvodni matici.");
            if (Console.ReadLine() == "ANO")
                return multArr;

            return arr;
        }

        static int[,] transposition(int[,] arr, int x, int y)
        {
            int[,] transArr = new int[y, x]; //vytvorime matici, kde pocet radku a sloupcu je oproti puvodni matici prohozeny
            for(int i = 0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    transArr[i, j] = arr[j, i];
                }
            }

            Console.WriteLine("Vysledek: ");
            writeArray(transArr, y, x);
            Console.WriteLine("Zadej 0, pokud chces dal pocitat s puvodni matici. Cokoliv jineho bude znamenat, ze dale budes pocitat znovu s puvodni matici.");
            if (Console.ReadLine() == "0")
                return arr;

            return transArr;
        }

        static void sortArray(int[,] arr, int x, int y) //funkce pro serazeni a vypis prvku od nejvetsiho po nejmensi
        {
            int sortArrSize = x * y;
            int[] sortArr = new int[sortArrSize];
            Console.WriteLine(sortArrSize);
            for(int i = 0; i < x; i++)
            {
                for(int j = 0;j < y; j++)
                {
                    Console.WriteLine(i * x + j);
                    sortArr[i * y + j] = arr[i, j];
                }
            }

            Array.Sort(sortArr);
            Array.Reverse(sortArr);
            Console.WriteLine("Vysledne poradi - prvky od nejvetsiho k nejmensimu: ");
            for (int i = 0; i < x * y; i++)
            {
                Console.Write(sortArr[i] + " ");
            }
            Console.Write("\n");
        }

        static void getMaxAverage(int[,] arr, int x, int y, bool getColumn) //funkce pro hledani radku nebo sloupce s maximalnim aritm. prumerem
        //getColumn = true => hledame sloupec; getColumn = false => hledame radek
        {
            int maxSum = 0, maxSumIndex = 0;
            for(int i = 0; i < (getColumn ? y : x); i++)
            {
                int currentSum = 0;
                for(int j = 0; j < (getColumn ? x : y); j++)
                {
                    currentSum += (getColumn ? arr[j, i] : arr[i, j]); //pokud chci sloupec, getColumn je true a promenna i oznacuje sloupce, j oznacuje radky; v pripade getColumn = false hledame radek, a proto promenne i a j jsou prohozene
                }
                if(currentSum > maxSum)
                {
                    maxSum = currentSum;
                    maxSumIndex = i;
                }
            }

            double maxAverage = Convert.ToDouble(maxSum) / Convert.ToDouble(x); 
             
            if(getColumn)
            {
                Console.WriteLine("Nejvetsi aritmeticky prumer ma sloupec " + maxSumIndex + " a hodnota prumeru je zaokrouhlene " + maxAverage);
            } else
            {
                Console.WriteLine("Nejvetsi aritmeticky prumer ma radek " + maxSumIndex + " a hodnota prumeru je zaokrouhlene " + maxAverage);
            }
            return;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Zadej pocet radku v matici: ");
            string xInput = Console.ReadLine();
            int x = convertTesting(xInput, Int32.MaxValue, 1);
            Console.WriteLine("Zadej pocet sloupcu v matici: ");
            string yInput = Console.ReadLine();
            int y = convertTesting(yInput, Int32.MaxValue, 1);

            int[,] arr = new int[x, y];
            int[,] secondArr = new int[x, y]; //bude pozdeji pouzita pro scitani a odcitani jako pomocna druha matice

            Console.WriteLine("Zadej 1, pokud chces matici naplnit nahodnymi cisly.");
            Console.WriteLine("Zadej cokoliv jineho, pokud chces matici naplnit za sebou jdoucimi cisly pocinaje jednickou.");
            string fillMethod = Console.ReadLine(); //fillMethod je 1 => cisla nahodna, fillMethod neni 1 => cisla od 1 do x*y
            if (fillMethod == "1")
                fillArray(arr, x, y, 1);
            else
                fillArray(arr, x, y, 2);

            do
            {
                Console.WriteLine("Zadej cislo operace kterou chces provest.");
                Console.WriteLine("1 pro prohozeni prvku, 2 pro prohozeni radku, 3 pro prohozeni sloupcu");
                Console.WriteLine("4 pro otoceni poradi prvku po hl. diagonale, 5 po vedl. diagonale");
                Console.WriteLine("6 pro vynasobeni matice cislem, 7 pro secteni dvou matic, 8 pro odecteni dvou matic, 9 pro nasobeni dvou matic");
                Console.WriteLine("10 pro transpozici matice");
                Console.WriteLine("11 pro vypis vsech cisel od nejvetsiho po nejmensi");
                Console.WriteLine("12 pro nalezeni radku, ktery ma nejvetsi aritmeticky prumer vsech cisel v danem radku");
                Console.WriteLine("13 pro nalezeni sloupce, ktery ma nejvetsi aritmeticky prumer vsech cisel v danem sloupci");

                string operationInput = Console.ReadLine();
                int operation = convertTesting(operationInput, 14, 1); //overujeme, zda uzivatel zadal platne cislo operace

                switch (operation)
                {
                    
                    case 1:
                        arr = swapElement(arr, x, y);
                        break;
                    case 2:
                        arr = swapRow(arr, x, y);
                        break;
                    case 3:
                        arr = swapColumn(arr, x, y);
                        break;
                    case 4:
                        arr = swapDiagonal(arr, x, y, true);
                        break;
                    case 5:
                        arr = swapDiagonal(arr, x, y, false);
                        break;
                    case 6:
                        arr = multiplyByNumber(arr, x, y);
                        break;
                    case 7:
                        Console.WriteLine("Pro operaci, kterou jsi zvolil, bude potreba vytvorit druhou matici, s niz se bude puvodni matice scitat.");
                        secondArr = createSecondArray(arr, x, y, false);
                        arr = addArrays(arr, secondArr, x, y);
                        break;
                    case 8:
                        Console.WriteLine("Pro operaci, kterou jsi zvolil, bude potreba vytvorit druhou matici, ktera se odecte od puvodni matice.");
                        secondArr = createSecondArray(arr, x, y, false);
                        arr = substractArrays(arr, secondArr, x, y);
                        break;
                    case 9:
                        Console.WriteLine("Pro operaci, kterou jsi zvolil, bude potreba vytvorit druhou matici, se kterou se puvodni matice vynasobi.");
                        secondArr = createSecondArray(arr, x, y, true);
                        arr = multiplyArrays(arr, secondArr, x, secondArr.GetLength(1)); //rozmery nove matice jsou x = pocet radku prvni matice a y = pocet sloupcu druhe matice
                        break;
                    case 10:
                        arr = transposition(arr, x, y);
                        if (arr.GetLength(0) != x)
                        {
                            int xTemp = x;
                            x = y;
                            y = xTemp;
                        }
                        break;
                    case 11:
                        sortArray(arr, x, y);
                        break;
                    case 12:
                        getMaxAverage(arr, x, y, false);
                        break;
                    case 13:
                        getMaxAverage(arr, x, y, true);
                        break;

                }

                Console.WriteLine("Zadej END (a pote zmackni Enter) pro ukonceni programu, pokud zadas cokoliv jineho, budes moct pokracovat v provadeni operaci s matici.");
            }
            while (Console.ReadLine() != "END");
            Console.ReadKey();
        }
    }
}
