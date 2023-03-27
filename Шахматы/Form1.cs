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

    }
}
