using System;
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

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            try { textBoxY.Text = textBoxX.Text; }
            catch { }
        }
        private void textBoxY_TextChanged(object sender, EventArgs e)
        {
            try { textBoxX.Text = textBoxY.Text; }
            catch { }
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
                MessageBox.Show("Неверные входные данные \n\n" + ex);
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
            //Ферзи

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
            //Кони           
            int n = 0;            

            input();

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    board2[y, x] = 0;

            try { setKnight(0, 0, n); }
            catch (Exception ex) { MessageBox.Show("Ход не возможен \n\n" + ex); }

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

        private void button3_Click(object sender, EventArgs e)
        {
            input();
            mazeGen();
            mazeRuner();
            outputMaze();
        }


        //матрицы
        private const int count_matrix = 4;

        private void button4_Click(object sender, EventArgs e)
        {
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
        //подстрока
        private void button5_Click(object sender, EventArgs e)
        {
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

        //паркет

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

        private void button7_Click(object sender, EventArgs e)
        {
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

        private void button8_Click(object sender, EventArgs e)
        {

        }
        private void button9_Click(object sender, EventArgs e)
        {

        }

    }
}
