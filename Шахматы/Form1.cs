﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Шахматы
{
    public partial class Form1 : Form
    {

        private int X, Y;

        private int[,] step = new int[8, 2] { { 1, -2 }, { 2, -1 }, { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 } };

        static int N = 1000;

        private int[,] board1 = new int[N, N];
        private int[,] board2 = new int[N, N];
        //private int[,] room1 = new int[N, N];


        private int[,] room1 = {


            { +1, +0, +0, +0, +0, +0, +0, +0 },
            { +0, +0, +0, -1, +0, -1, +0, +0 },
            { +0, +0, +0, +0, -1, +0, -1, +0 },
            { -1, -1, -1, +0, +0, +0, +0, +0 },
            { +0, +0, +0, +0, -1, +0, +0, -1 },
            { +0, -1, -1, -1, +0, -1, +0, +0 },
            { +0, +0, +0, +0, +0, -1, -1, +0 },
            { +0, +0, -1, +0, -1, -1, +0, +0 },

        };



        public Form1()
        {
            InitializeComponent();
        }



        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Visible = false;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Visible = true;
        }


        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            //try { textBoxY.Text = textBoxX.Text; }
            //catch { }
        }
        private void textBoxY_TextChanged(object sender, EventArgs e)
        {
            //try { textBoxX.Text = textBoxY.Text; }
            //catch { }
        }


        private void input()
        {
            try
            {
                X = int.Parse(textBoxX.Text);
                Y = int.Parse(textBoxY.Text);                               

                dataGridView1.RowCount = Y;
                dataGridView1.ColumnCount = X;

                for (int y = 0; y < Y; y++)                 //покраска доски
                    for (int x = 0; x < X; x++)
                    {
                        if ((x + y) % 2 == 0)
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(225, 190, 145);// Yellow
                        }
                        else
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(187, 128, 51);// Brown
                        }

                        dataGridView1.Rows[y].Cells[x].Value = null;
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверные входные данные \r\n" + ex);
            }
        }

        private void outputQueen()
        {
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    if (board1[y, x] == -1)
                    {
                        dataGridView1.Rows[y].Cells[x].Value = "X";
                        dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Gray;
                    }
        }

        private void setQueen(int i, int j)
        {
            for (int x = 0; x < X; x++)
            {
                board1[x, j] += 1;
                board1[i, x] += 1;

                if (0 <= i - j + x && i - j + x < X)
                    board1[i - j + x, x] += 1;
                if (0 <= i + j - x && i + j - x < X)
                    board1[i + j - x, x] += 1;

                board1[i, j] = -1;
            }
        }

        private void removeQueen(int i, int j)
        {
            for (int x = 0; x < X; x++)
            {
                board1[x, j] -= 1;
                board1[i, x] -= 1;

                if (0 <= i - j + x && i - j + x < X)
                    board1[i - j + x, x] -= 1;
                if (0 <= i + j - x && i + j - x < X)
                    board1[i + j - x, x] -= 1;

                board1[i, j] = 0;
            }
        }

        private void solve(int i)
        {
            for (int j = 0; j < X; j++)
                if (board1[i, j] == 0)
                {
                    setQueen(i, j);

                    if (i == X - 1)
                    {
                        outputQueen();
                        break;
                    }
                    else
                        solve(i + 1);

                    removeQueen(i, j);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ферзи          +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            input();

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    board1[y, x] = 0;

            solve(0);
        }

        private void outputKnight()
        {
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    dataGridView1.Rows[y].Cells[x].Value = board2[y, x];
        }

        private bool setKnight(int x, int y, int n)
        {            
            if ((x < 0) || (x >= X) || (y < 0) || (y >= X))
                return false;

            if (board2[y, x] != 0)
                return false;
                        
            n++;
            board2[y, x] = n;

            if (n == X * X)
                return true;

            for (int i = 0; i < 8; i++)
                if (setKnight(x + step[i, 0], y + step[i, 1], n))
                    return true;
            n--;
            board2[y, x] = 0;

            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Кони                                             +++++++++++++++++++++++++++++++++++++++++++++++++  
            int n = 0;            

            input();

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    board2[y, x] = 0;

            try { setKnight(0, 0, n); }
            catch (Exception ex) { MessageBox.Show("Ход не возможен \r\n" + ex); }

            outputKnight();

        }

        private void mazeGen()
        {
            X = 8;
            Y = 8;

            

        }

        private void outputMaze()
        {
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (room1[y, x]!=0 && room1[y, x] != -1)
                        dataGridView1.Rows[y].Cells[x].Value = room1[y, x];

                    dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Gray;

                    if (room1[y, x] == -1)
                        dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Brown;
                }

            dataGridView1.Rows[0].Cells[0].Style.BackColor = Color.Gold;
            dataGridView1.Rows[Y-1].Cells[X-1].Style.BackColor = Color.Green;
        }

        private void mazeRuner()
        {
            int step = 1;
            while (step < X * Y)
            {
                for (int y = 0; y < Y; y++)
                    for (int x = 0; x < X; x++)
                    {
                        if (room1[y, x] == step)
                        {
                            if (y != Y - 1 && room1[y + 1, x] == 0)
                                room1[y + 1, x] = step + 1;
                            if (x != X - 1 && room1[y, x + 1] == 0)
                                room1[y, x + 1] = step + 1;
                            if (y != 0 && room1[y - 1, x] == 0)
                                room1[y - 1, x] = step + 1;
                            if (x != 0 && room1[y, x - 1] == 0)
                                room1[y, x - 1] = step + 1;

                            if (room1[Y - 1, X - 1] != 0)
                                return;
                        }

                    }
                step++;
            }

            MessageBox.Show("Тупиковый лабиринт!");
                
        }
        // Maze   +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void button3_Click(object sender, EventArgs e)
        {
            input();
            mazeGen();
            mazeRuner();
            outputMaze();
        }


        //матрицы            +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private const int count_matrix = 4;

        private void button4_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 16f);

            textBox1.Text = "";

            int[,] matrix = new int [count_matrix, 2] { { 10, 20}, { 20, 50}, { 50, 40}, { 40, 20} };
            int[,] table = new int [count_matrix - 1, count_matrix - 1];
            for (int i = 0; i < count_matrix - 1; i++)
            {
                for (int j = 0; j < count_matrix - 1 - i; j++)
                {
                    if (i == 0)
                    {
                        table[0, j] = matrix[j,0] * matrix[j,1] * matrix[j + 1,1];
                    }
                    else
                    {
                        if (table[i - 1,j] <= table[i - 1,j + 1])
                        {
                            table[i,j] = table[i - 1,j] + (matrix[j,0] * matrix[i + 1 + j,0] * matrix[i + 1 + j,1]);
                        }
                        else
                        {
                            table[i,j] = table[i - 1,j + 1] + (matrix[j,0] * matrix[j,1] * matrix[i + 1 + j,1]);
                        }
                    }
                }
            }
            for (int i = 0; i < count_matrix - 1; i++)
            {
                for (int j = 0; j < count_matrix - 1; j++)
                {
                    textBox1.Text += table[i,j].ToString() + " ";
                }
                textBox1.Text += "\r\n";
            }
            textBox1.Text += "Return: " + table[count_matrix - 2, 0].ToString();

        }  
        //подстрока               +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void button5_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 16f);

            textBox1.Text = "";

            string s1 = "nhfycajhvf";
            string s2 = "qktrnhjcnfywbz";

            char[] str1 = s1.ToCharArray();
            char[] str2 = s2.ToCharArray();
            int[,] table = new int [str1.Length, str2.Length];

            if (str1[0] == str2[0])
            {
                table[0,0] = 1;
            }
            else
            {
                table[0,0] = 0;
            }
            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (i == 0 && j > 0)
                    {
                        if (str1[0] == str2[j])
                        {
                            if (str1[0] == str2[j - 1])
                            {
                                table[0,j] = table[0,j - 1] + 1;
                            }
                            else if (table[0,j - 1] > 1)
                            {
                                table[0,j] = table[0,j - 1];
                            }
                            else
                            {
                                table[0,j] = 1;
                            }
                        }
                        else
                        {
                            table[0,j] = table[0,j - 1];
                        }
                    }
                    else
                    {
                        if (i != 0 && j == 0 && str1[i] != str2[0])
                        {
                            table[i, 0] = table[i - 1, 0];
                        }
                        else if (i != 0 && j != 0 && str1[i] != str2[j])
                        {
                            if (table[i,j - 1] > table[i - 1,j])
                            {
                                table[i,j] = table[i,j - 1];
                            }
                            else
                            {
                                table[i,j] = table[i - 1,j];
                            }
                        }
                        else if(i != 0 && j != 0)
                        {
                            if (table[i,j - 1] > table[i - 1,j])
                            {
                                table[i,j] = table[i,j - 1] + 1;
                            }
                            else
                            {
                                table[i,j] = table[i - 1,j] + 1;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    textBox1.Text += table[i, j].ToString() + " ";
                }
                textBox1.Text += "\r\n";
            }
             
            int a = table[str1.Length - 1, str2.Length - 1];

            textBox1.Text += "Result: " + a;

        }

        //паркет                  ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private bool good(int mask)
        {
            int pos = NP - 1;
            while (pos >= 0)
                if ((~mask & pow2[pos]) != 0)
                    if (pos == 0)
                        return false;
                    else if ((mask & pow2[pos - 1]) != 0)
                        return false;
                    else pos -= 2;
                else pos--;
            return true;
        }

        private long[,] Res = new long[21, 256];
        private int[] goodMask = new int[256];
        private int[] pow2 = new int[8];

        private int NP = 8, MP = 20;        

        private void button6_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 16f);

            textBox1.Text = "";


            for (int i = 0; i < NP; i++)
                pow2[i] = 1 << i;
            int border = (1 << NP);
            for (int i = 0; i < border; i++)
            {
                if (good(i))
                    goodMask[i] = 1;
                else
                    goodMask[i] = 0;
            }
            Res[0,0] = 1;
            for (int pos = 0; pos < MP; pos++)
                for (int i = 0; i < border; i++)
                    for (int j = 0; j < border; j++)
                        if ((i & j) == 0)
                            if (goodMask[i | j] != 0)
                                Res[pos + 1,j] += Res[pos,i];

            for (int i = 0; i <= MP; i++)
                textBox1.Text += Res[i, 0] + " ";

            long a = Res[MP, 0];

            textBox1.Text += "\r\nResult: " + a;



        }

        //станции                          +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void button7_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 16f);

            textBox1.Text = "";

            Random rand = new Random();

            int costMax = 300;
            int costMin = 100;

            int gap = 100;

            int[] stations = new int[rand.Next(6, 10)];            

            int[] selectedTransfer = new int[stations.Length];

            int price = 0;

            double s = 0;

            for (int i = 0; i < stations.Length; i++)
                stations[i] = rand.Next(costMin, costMax);

            int[] stationsTotalCost = stations.Clone() as int[];

            for (int i = 1; i < stations.Length; i++)
            {
                stationsTotalCost[i] += stationsTotalCost[i - 1];
            }

            for (int i = 0; i < stations.Length; i++)
            {
                s += stations[i];

                if (Math.Round(s / gap) * gap < s || i == stations.Length - 1)
                {
                    price += Convert.ToInt32(Math.Round(s / gap) * gap);
                    selectedTransfer[i] = Convert.ToInt32(Math.Round(s / gap) * gap);
                    s = 0;
                }

            }

            textBox1.Text += "Стоимость между станциями: ";

            for (int i = 0; i < stations.Length; i++)
            {
                textBox1.Text += " " + stations[i];
            }

            textBox1.Text += "\r\nСтоимость между станциями, если платить за каждую, без округления: ";

            for (int i = 0; i < stations.Length; i++)
            {
                textBox1.Text += " " + stationsTotalCost[i];
            }

            textBox1.Text += "\r\nВыбранные нами станции, и их стоимость: ";

            for (int i = 0; i < stations.Length; i++)
            {
                textBox1.Text += " " + selectedTransfer[i];
            }

            textBox1.Text += "\r\nОбщая стоимость выбранных нами станций: ";

            textBox1.Text += " " + price;
        }
        //отрезки                   ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        bool cross(double ax1, double ay1, double ax2, double ay2,
           double bx1, double by1, double bx2, double by2)
        {
            double v1 = (bx2 - bx1) * (ay1 - by1) - (by2 - by1) * (ax1 - bx1);
            double v2 = (bx2 - bx1) * (ay2 - by1) - (by2 - by1) * (ax2 - bx1);
            double v3 = (ax2 - ax1) * (by1 - ay1) - (ay2 - ay1) * (bx1 - ax1);
            double v4 = (ax2 - ax1) * (by2 - ay1) - (ay2 - ay1) * (bx2 - ax1);
            return (v1 * v2 < 0 && v3 * v4 < 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 32f);

            textBox1.Text = "";

            double x1 = double.Parse(textBoxX1.Text);
            double y1 = double.Parse(textBoxY1.Text);
            double x2 = double.Parse(textBoxX2.Text);
            double y2 = double.Parse(textBoxY2.Text);

            double x3 = double.Parse(textBoxX3.Text);
            double y3 = double.Parse(textBoxY3.Text);
            double x4 = double.Parse(textBoxX4.Text);
            double y4 = double.Parse(textBoxY4.Text);


            textBox1.Text = (cross(x1, y1, x2, y2, x3, y3, x4, y4) ? "True" : "False");
        }

        

        //Треугольник                   ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.Font = new Font("Microsoft Sans Serif", 20f);

            textBox1.Text = "";

            double x1 = double.Parse(textBoxX1.Text);
            double y1 = double.Parse(textBoxY1.Text);
            double x2 = double.Parse(textBoxX2.Text);
            double y2 = double.Parse(textBoxY2.Text);

            double x3 = double.Parse(textBoxX3.Text);
            double y3 = double.Parse(textBoxY3.Text);
            double x = double.Parse(textBoxX4.Text);
            double y = double.Parse(textBoxY4.Text);

            if ((x1 == x2 && x2 == x3) || (y1 == y2 && y2 == y3))
            {
                textBox1.Text += "Неверные координаты треугольника!";
            }
            else
            {
                double s = Math.Abs((x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1)) / 2;
                double s1 = Math.Abs((x2 - x1) * (y - y1) - (x - x1) * (y2 - y1)) / 2;
                double s2 = Math.Abs((x - x1) * (y3 - y1) - (x3 - x1) * (y - y1)) / 2;
                double s3 = Math.Abs((x2 - x) * (y3 - y) - (x3 - x) * (y2 - y)) / 2; 

                if (s == s1 + s2 + s3)
                    textBox1.Text += "Точка в треугольнике";
                else
                    textBox1.Text += "Точка вне треугольника";
            }

        }
        


        //Окружность        +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void button10_Click(object sender, EventArgs e)
        {

            textBox1.Font = new Font("Microsoft Sans Serif", 20f);

            textBox1.Text = "";

            int a = int.Parse(textBoxX1.Text);
            int b = int.Parse(textBoxY1.Text);

            int R = int.Parse(textBoxX2.Text);

            int x = int.Parse(textBoxX4.Text);
            int y = int.Parse(textBoxY4.Text);


            double S = Math.Sqrt(Math.Pow((x - a), 2) + Math.Pow((y - b), 2));            

            if (S < R)
               textBox1.Text += "Точка внутри окружности.";
            else if (S == R)
                textBox1.Text += "Точка прямо на окружности.";
            else
                textBox1.Text += "Точка снаружи окружности.";

        }

        

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = "";

                int n = int.Parse(textBoxX.Text);
                n++;
                int[] mass = new int[n];
                
                for (int i = 2; i < n; i++)
                {
                    mass[i] = mass[i - 1] + 1;
                    if ((i % 2 == 0) && (mass[i / 2] + 1 < mass[i]))
                    {
                        mass[i] = mass[i / 2] + 1;
                    }
                }
                for (int i = 0; i < n; i++)
                    textBox2.Text += mass[i] + " ";
                textBox2.Text += "\r\n";
                for (int i = 0; i < n; i++)
                    textBox2.Text += i + " ";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверные входные данные \r\n" + ex);
            }            
        }

        

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = "";
                int m = int.Parse(textBoxX.Text);
                int n = int.Parse(textBoxY.Text); ;

                int[,] matrix = new int[n, m];
                
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                    {
                        if (i == 0 || j == 0)
                        {
                            matrix[i, j] = 1;
                        }
                        else
                        {
                            matrix[i, j] = 0;
                        }
                    }
                for (int i = 1; i < n; i++)
                {
                    for (int j = 1; j < m; j++)
                    {
                        matrix[i, j] = matrix[i - 1, j] + matrix[i, j - 1];
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        textBox2.Text += matrix[i, j] + " ";
                    }
                    textBox2.Text += "\r\n";
                }
                textBox2.Text += matrix[n - 1, m - 1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверные входные данные \r\n" + ex);
            }            
        }



        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                X = int.Parse(textBoxX.Text);
                Y = int.Parse(textBoxY.Text);

                dataGridView1.RowCount = Y;
                dataGridView1.ColumnCount = X;

                for (int y = 0; y < Y; y++)                 //покраска доски
                    for (int x = 0; x < X; x++)
                    {
                        if ((x + y) % 2 == 0)
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(225, 190, 145);// Yellow
                        }
                        else
                        {
                            dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(187, 128, 51);// Brown
                        }

                        //dataGridView1.Rows[y].Cells[x].Value = null;
                    }

                int[,] matrix0 = { { 5, 2, 3, -2, -2 }, { -1, 4, 1, -3, 10 }, { 6, -2, 4, -5, 0 }, { 12, -8, -5, 3, 6 } };

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 5; j++)
                        dataGridView1.Rows[i].Cells[j].Value = matrix0[i, j];

                //int n = 4, m = 5;
                //textBox2.Text = "";

                int m = X;
                int n = Y;

                int[,] matrix = new int[n, m];


                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        matrix[i, j] = Convert.ToInt32(dataGridView1.Rows[i].Cells[j].Value);

                for (int i = 1; i < m; i++)
                    matrix[0, i] += matrix[0, i - 1];
                for (int i = 1; i < n; i++)
                    matrix[i, 0] += matrix[i - 1, 0];
                for (int i = 1; i < n; i++)
                {
                    for (int j = 1; j < m; j++)
                    {
                        matrix[i, j] += (matrix[i - 1, j] > matrix[i, j - 1]) ? matrix[i - 1, j] : matrix[i, j - 1];
                    }
                }
                int a = n - 1, b = m - 1;
                while (true)
                {
                    dataGridView1.Rows[a].Cells[b].Style.BackColor = Color.Green;

                    //textBox2.Text += (a + 1) + " " + (b + 1) + "\r\n";

                    if (a == 0 && b == 0)
                        break;
                    if (a == 0)
                    {
                        b--;
                        continue;
                    }
                    if (b == 0)
                    {
                        a--;
                        continue;
                    }
                    if (matrix[a - 1, b] > matrix[a, b - 1])
                        a--;
                    else
                        b--;
                }

                //for (int i = 0; i < n; i++)
                //    for (int j = 0; j < m; j++)
                //        dataGridView1.Rows[i].Cells[j].Value = matrix[i, j];

            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверные входные данные \r\n" + ex);
            }
        }
    }
}
