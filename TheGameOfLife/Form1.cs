using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TheGameOfLife
{
    public partial class Form1 : Form
    {
        
        static int width = 50;
        static int height = 50;
        PictureBox[,] pb = new PictureBox[width, height];
        int[,] cellStatus = new int[width, height];
        int[,] aliveNeighbors = new int[width, height];
        string alivePath = "..\\..\\Images\\1.jpg";
        string deadPath = "..\\..\\Images\\0.jpg";
        int StepsCounter = 0;

        List<int> lista = new List<int>();
        List<PictureBox> pbListON = new List<PictureBox>();
        List<PictureBox> pbListOFF = new List<PictureBox>();

        List<int> cellAlive = new List<int>();
        List<int> cellDead = new List<int>();


        public Form1()
        {
            InitializeComponent();

            createGameField();
        }

        void click(object sender, MouseEventArgs e)
        {
            if (((PictureBox)sender).ImageLocation.ToString().Substring(((PictureBox)sender).ImageLocation.ToString().LastIndexOf('\\') + 1) == "0.jpg")
            {
                ((PictureBox)sender).ImageLocation = alivePath;
            }
            else
            {
                ((PictureBox)sender).ImageLocation = deadPath;
            }
        }

        private void createGameField()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    pb[x, y] = new PictureBox();
                    pb[x, y].ImageLocation = deadPath;
                    pb[x, y].Name = x.ToString() + ',' + y.ToString();
                    cellStatus[x, y] = 0;
                    aliveNeighbors[x, y] = 0;
                    pb[x, y].MouseClick += new MouseEventHandler(click);
                    gameField.Controls.Add(pb[x, y], x, y);
                }
            }
        }

        private void isCellAliveNextTurn2(int x, int y)
        {
            if (cellStatus[x, y] == 0)
            {
                if (aliveNeighbors[x, y] == 3)
                {
                    cellAlive.Add(cellStatus[x, y]);
                    pbListON.Add(pb[x,y]);
                    //cellON(x, y);
                }
            }
            if (cellStatus[x, y] == 1)
            {
                if (aliveNeighbors[x, y] == 2 || aliveNeighbors[x, y] == 3)
                {
                    cellStatus[x, y] = 1;
                }
                else
                {
                    cellDead.Add(cellStatus[x, y]);
                    pbListOFF.Add(pb[x, y]);
                    //cellOFF(x, y);
                }
            }
        }

        private void StartStop()
        {       
            labelStepsCounter.Text = "Steps: " + (StepsCounter += 1).ToString();          
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    checkAliveNeighbors(x, y);
                    isCellAliveNextTurn2(x, y);
                    aliveNeighbors[x, y] = 0;
                }
            }

            for (int i = 0; i < pbListOFF.Count; i++)
            {
                cellDead[i] = 0;
                pbListOFF[i].ImageLocation = deadPath;
            }
            for (int i = 0; i < pbListON.Count; i++)
            {
                cellAlive[i] = 1;               
                pbListON[i].ImageLocation = alivePath;
            }

            cellDead.Clear();
            cellAlive.Clear();

            pbListON.Clear();
            pbListOFF.Clear();

            //MessageBox.Show(pbListON.Count.ToString());
            //MessageBox.Show(pbListOFF.Count.ToString());


            /*
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    isCellAliveNextTurn(x, y);
                    aliveNeighbors[x, y] = 0;
                }
            }
            */
        }

        private void isCellAliveNextTurn(int x, int y)
        {
            if (cellStatus[x, y] == 0)
            {
                if (aliveNeighbors[x, y] == 3)
                {
                    cellStatus[x, y] = 1;
                    cellON(x, y);
                }
            }
            if (cellStatus[x, y] == 1)
            {
                if (aliveNeighbors[x, y] == 2 || aliveNeighbors[x, y] == 3)
                {
                    cellStatus[x, y] = 1;
                }
                else
                {
                    cellStatus[x, y] = 0;
                    cellOFF(x, y);
                }
            }
        }

        private void checkCellStatus(int x, int y)
        {         
            if (pb[x, y].ImageLocation.ToString().Substring(pb[x, y].ImageLocation.ToString().LastIndexOf('\\') + 1) == "0.jpg")
            {
                cellStatus[x, y] = 0;
            }
            else
                cellStatus[x, y] = 1;              
        }

        private void checkAliveNeighbors(int x, int y)
        {
            if (x > 0)
            {
                if (y > 0)
                {
                    if (cellStatus[x - 1, y - 1] == 1) aliveNeighbors[x, y] += 1;
                }
                if (y < height-1)
                {
                    if (cellStatus[x - 1, y + 1] == 1) aliveNeighbors[x, y] += 1;
                }
                if (cellStatus[x - 1, y] == 1) aliveNeighbors[x, y] += 1;
            }
            if (x < width-1)
            {
                if (y > 0)
                {
                    if (cellStatus[x + 1, y - 1] == 1) aliveNeighbors[x, y] += 1;
                }
                if (y < height - 1)
                {
                    if (cellStatus[x + 1, y + 1] == 1) aliveNeighbors[x, y] += 1;
                }
                if (cellStatus[x + 1, y] == 1) aliveNeighbors[x, y] += 1;
            }
            if (y > 0)
            {
                if (cellStatus[x, y - 1] == 1) aliveNeighbors[x, y] += 1;
            }
            if (y < height - 1)
            {
                if (cellStatus[x, y + 1] == 1) aliveNeighbors[x, y] += 1;
            }
        }

        private void cellON(int x, int y)
        {
            pb[x, y].ImageLocation = alivePath;
        }

        private void cellOFF(int x, int y)
        {
            pb[x, y].ImageLocation = deadPath;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartStop();
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            checkGameField();
            if (timer1.Enabled == true)
            {
                timer1.Stop();
                buttonStartStop.Text = "Start";
                buttonsSwitch(true);
            }
            else
            {
                timer1.Start();
                buttonStartStop.Text = "Stop";
                buttonsSwitch(false);
            }
        }

        private void buttonGGLoad_Click(object sender, EventArgs e)
        {
            buttonsSwitch(false);
            buttonStartStop.Enabled = false;

            string[] lines = File.ReadLines("..\\..\\GG.txt").ToArray();

            for(int y = 0; y < width; y++)
            {
                string[] splittedLine = lines[y].Split(',');
                for (int x = 0; x < width; x++)
                {
                    cellStatus[x, y] = Int32.Parse(splittedLine[x]);
                    if (cellStatus[x, y] == 1) cellON(x, y);
                    if (cellStatus[x, y] == 0) cellOFF(x, y);
                }
            }
            MessageBox.Show("Wczytano Glider Gun!");
            buttonsSwitch(true);
            buttonStartStop.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            buttonsSwitch(false);
            buttonStartStop.Enabled = false;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "txt files (*.txt)|*.txt";
            sf.ShowDialog();
            if(sf.FileName != "")
            {
                string[] lines = new string[50];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        checkCellStatus(x, y);
                        lines[y] += cellStatus[x, y].ToString() + ",";
                    }
                }
                File.WriteAllLines(sf.FileName, lines);
                MessageBox.Show("Zapisano do pliku!");
            }
            buttonsSwitch(true);
            buttonStartStop.Enabled = true;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            buttonsSwitch(false);
            buttonStartStop.Enabled = false;
            StepsCounter = 0;
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    cellOFF(x, y);
                }
            }
            labelStepsCounter.Text = "Steps: " + StepsCounter.ToString();
            MessageBox.Show("Zresetowano plansze!");
            buttonsSwitch(true);
            buttonStartStop.Enabled = true;
        }

        private void buttonsSwitch(bool status)
        {
            buttonGGLoad.Enabled = status;
            buttonSave.Enabled = status;
            buttonReset.Enabled = status;
            buttonLoad.Enabled = status;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            buttonsSwitch(false);
            buttonStartStop.Enabled = false;

            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "txt files (*.txt)|*.txt";
            of.ShowDialog();
            if(of.FileName != "")
            {
                try
                {
                    string[] lines = File.ReadLines(of.FileName).ToArray();

                    for (int y = 0; y < width; y++)
                    {
                        string[] splittedLine = lines[y].Split(',');
                        for (int x = 0; x < width; x++)
                        {
                            cellStatus[x, y] = Int32.Parse(splittedLine[x]);
                            if (cellStatus[x, y] == 1) cellON(x, y);
                            if (cellStatus[x, y] == 0) cellOFF(x, y);
                        }
                    }
                    MessageBox.Show("Wczytano zapis!");
                }
                catch
                {
                    MessageBox.Show("Błąd odczytu");
                }
            }

            buttonsSwitch(true);
            buttonStartStop.Enabled = true;
        }

        private void checkGameField()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    checkCellStatus(x, y);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cellDead[0].ToString());
        }
    }
}
