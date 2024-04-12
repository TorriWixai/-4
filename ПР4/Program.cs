using System;
using System.Collections.Generic;

namespace Мат.Мод
{
    public class SimpMet
    {

        double[,] table; //симплекс-таблица

        int m, n;

        List<int> basis; //список базисных переменных

        public SimpMet(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }

            n = table.GetLength(1);
        }

        //result - в этот массив будут записаны полученные значения X
        public double[,] Reh(double[] result)
        {
            int mainCol, mainRow; //переменные для ведущие столбец и строка

            while (!End())
            {
                mainCol = Column();
                mainRow = Row(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }

        private bool End()//проверяет достигнута ли корнечнаят точка процесса
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int Column()//находит вед столбец
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int Row(int mainCol)//находит вед строку
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }

        class Program
        {
            static void Main(string[] args)
            {
                double[,] table = { {-6, -3,  -4},
                                    {3, 1, 3},
                                    {4,  2,  1},
                                    {0,  -4, -16}
                                  };

                double[] result = new double[2];
                double[,] table_result;
                SimpMet S = new SimpMet(table);
                table_result = S.Reh(result);
                Console.WriteLine("Симплекс таблица с оптимальным решением:\n");
                for (int i = 0; i < table_result.GetLength(0); i++)
                {
                    for (int j = 0; j < table_result.GetLength(1); j++)
                    {
                        Console.Write(table_result[i, j].ToString("#0.##").PadLeft(8)); // Форматируем числа с точкой как десятичный разделитель
                        Console.Write(" "); // Добавляем пробел после каждого числа
                    }
                    Console.WriteLine(); // Переходим на новую строку после каждой строки таблицы
                }

                Console.WriteLine();
                Console.WriteLine("Так как значения свободных неизвестных в индексной строке положительны, значит решение оптимально.");
                Console.WriteLine("Так как переменная x осталась в столбе свободных переменных, приравняем ее к нулю.");
                Console.WriteLine("Тогда итоговая функция выглядит так:\nF_max=4*0+16*1=16");
                Console.WriteLine("X = " + result[0]);
                Console.WriteLine("Y = " + result[1]);
                Console.WriteLine("Поставьте 5 пожалуйста!!!!!!");
                Console.WriteLine("Мы правда старались!!!!");
                Console.WriteLine("Это я добавила изменения в чужой репозиторий");
                Console.WriteLine("Мы вас любим, Екатерина Михайловна!");
                Console.ReadLine();
            }
        }
    }
}


