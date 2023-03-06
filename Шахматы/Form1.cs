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

        private void outputQueen(int[,] board)
        {
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    if (board[x, y] == -1)
                    {
                        dataGridView1.Rows[y].Cells[x].Value = "X";
                        dataGridView1.Rows[y].Cells[x].Style.BackColor = Color.Gray;
                    }
        }

        private void setQueen(int i, int j, int[,] board)
        {
            for (int x = 0; x < X; x++)
            {
                board[x, j] += 1;
                board[i, x] += 1;

                if (0 <= i - j + x && i - j + x < X)
                    board[i - j + x, x] += 1;
                if (0 <= i + j - x && i + j - x < X)
                    board[i + j - x, x] += 1;

                board[i, j] = -1;
            }
        }

        private void removeQueen(int i, int j, int[,] board)
        {
            for (int x = 0; x < X; x++)
            {
                board[x, j] -= 1;
                board[i, x] -= 1;

                if (0 <= i - j + x && i - j + x < X)
                    board[i - j + x, x] -= 1;
                if (0 <= i + j - x && i + j - x < X)
                    board[i + j - x, x] -= 1;

                board[i, j] = 0;
            }
        }

        private void solve(int i, int[,] board)
        {
            for (int j = 0; j < X; j++)
                if (board[i, j] == 0)
                {
                    setQueen(i, j, board);

                    if (i == X - 1)
                    {
                        outputQueen(board);
                        break;
                    }
                    else
                        solve(i + 1, board);

                    removeQueen(i, j, board);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Ферзи

            input();

            int[,] board = new int[X, Y];

            solve(0, board);
        }

        private void outputKnight(int[,] board)
        {
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    dataGridView1.Rows[y].Cells[x].Value = board[x, y];
        }

        private void setKnight(int i, int j, int[,] board)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Кони           

            int[,] step = new int[8, 2] { { 1, -2 }, { 2, -1 }, { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 } };

            input();

            int[,] board = new int[X, Y];

            

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
