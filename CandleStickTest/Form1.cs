using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CandleStick;

namespace CandleStickTest
{
    public partial class Form1 : Form
    {
        public class Bar
        {
            public DateTime Time { get; }
            public double Open { get; }
            public double High { get; }
            public double Low { get; }
            public double Close { get; }
            public Bar(DateTime time, double high, double low, double open, double close)
            {
                Time = time;
                Open = open;
                Low = low;
                High = high;
                Close = close;
            }
        }
        public class ChartPoint
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
            public Color Color { get; set; }
            public int Size { get; set; }
            public ChartPoint(DateTime time, double value, Color pointColor, int size)
            {
                Time = time;
                Value = value;
                Color = pointColor;
                Size = size;
            }
        }
        public class PointSymbol
        {
            public int Symbol { get; set; }
            public DateTime Time { get; set; }
            public double Value { get; set; }
            public Color Color { get; set; }
            public int Size { get; set; }
            public PointSymbol(int symbol, DateTime time, double value, Color color, int size)
            {
                Symbol = symbol;
                Time = time;
                Value = value;
                Color = color;
                Size = size;
            }
        }
        public class PointText
        {
            public string Text { get; set; }
            public DateTime Time { get; set; }
            public double Value { get; set; }
            public Color Color { get; set; }
            public int Size { get; set; }
            public PointText(string text, DateTime time, double value, Color color, int size)
            {
                Text = text;
                Time = time;
                Value = value;
                Color = color;
                Size = size;
            }
        }
        double open, close;
        List<Bar> barData;
        List<ChartPoint> HighLine;
        List<ChartPoint> LowLine;
        List<PointSymbol> SymbolList;
        List<PointText> TextList;
        Chart<Bar, ChartPoint, PointSymbol, PointText> chart;

        int timeCounter = 0;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Form1 Start");
            ChartDefaults.DefaultColorSetting(Color.White, Color.Black, Color.Green, Color.Red, Color.Red, Color.Green);
            chart = new Chart<Bar, ChartPoint, PointSymbol, PointText>(panel1);
            barData = new List<Bar>();
            HighLine = new List<ChartPoint>();
            LowLine = new List<ChartPoint>();
            TextList = new List<PointText>();
            SymbolList = new List<PointSymbol>();
            Random rand = new Random();
            open = rand.NextDouble() * 9.0 + 0.5;
            close = open + rand.NextDouble() / 50;
            Bar datum = new Bar(DateTime.Now, close + rand.NextDouble() / 50, open - rand.NextDouble() / 50, open, close);
            barData.Add(datum);
            ChartPoint HighPoint = new ChartPoint(datum.Time, datum.High, Color.Red, 3);
            HighLine.Add(HighPoint);
            ChartPoint LowPoint = new ChartPoint(datum.Time, datum.Low, Color.Green, 3);
            LowLine.Add(LowPoint);
            
            DateTime startTime = DateTime.Now.AddSeconds(-100);
            for (int i = 0; i < 100; i++)
            {
                bool isUpGoing = (rand.Next(0, 2) > 0);
                open = close;
                if (isUpGoing)
                {
                    close = open + rand.NextDouble() / 50;
                    datum = new Bar(startTime.AddSeconds(i), close + rand.NextDouble() / 50, open - rand.NextDouble() / 50, open, close);
                    barData.Add(datum);
                }
                else
                {
                    close = open - rand.NextDouble() / 50;
                    datum = new Bar(startTime.AddSeconds(i), open + rand.NextDouble() / 50, close - rand.NextDouble() / 50, open, close);
                    barData.Add(datum);
                }
                Color color = (isUpGoing) ? Color.Green : Color.Red;
                HighPoint = new ChartPoint(datum.Time, datum.High, color, 3);
                HighLine.Add(HighPoint);
                LowPoint = new ChartPoint(datum.Time, datum.Low, color, 3);
                LowLine.Add(LowPoint);

                if (i % 30 == 10)
                {
                    TextList.Add(new PointText(datum.High.ToString("n5"), startTime.AddSeconds(i), datum.High, Color.White, 10));
                }
                if (i % 30 == 0)
                {
                    SymbolList.Add(new PointSymbol((int)SymbolType.SmileFace, startTime.AddSeconds(i), datum.High, Color.Yellow, 20));
                }
            }
            
            chart.SetBarParams("Time", "Open", "Close", "High", "Low");
            chart.SetData(barData);
            chart.SetLinePointParams("Time", "Value", "Color", "Size");
            chart.SetSymbolPointParams("Symbol", "Time", "Value", "Color", "Size");
            chart.SetTextPointParams("Text", "Time", "Value", "Color", "Size");
            chart.AddLine(LowLine);
            chart.AddLine(HighLine);
            chart.SetText(TextList);
            chart.SetSymbol(SymbolList);
            //chart.SymbolPattern = new string[] { "U", "X" };
            chart.SetYAxisWidth(70);
            chart.SetInfoLabel(lblCurTime, lblCurHigh, lblCurLow, lblCurOpen, lblCurClose);
            chart.SetTitle("New Chart");
            chart.SetAxiSFormart("dd/MM/yyyy HH:mm:ss");
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart.IsXSpacing = checkBox1.Checked;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            timeCounter++;
            if(timeCounter % 2 == 0)
            {
                Button1_Click(null, null);
            }
            else
            {
                Bar bar = barData[barData.Count - 1];
                double newClose = bar.Close + rand.NextDouble() / 100 - 0.005;
                Bar newBar = new Bar(bar.Time, Math.Max(bar.High, newClose), Math.Min(bar.Low, newClose), bar.Open, newClose);
                barData[barData.Count - 1] = newBar;
            }

        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            chart.Zoomin();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            chart.ZoomOut();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            chart.IsCrossView = checkBox2.Checked;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            btnTLine.BackColor = Color.White;
            btnVLine.BackColor = Color.White;
            btnHLine.BackColor = Color.White;
            if(chart.DrawingType == LineType.vertical)
            {
                chart.DrawingType = LineType.none;
            }
            else
            {
                chart.SetLineType(1);//vertical
                btnVLine.BackColor = Color.Red;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            btnTLine.BackColor = Color.White;
            btnVLine.BackColor = Color.White;
            btnHLine.BackColor = Color.White;
            if (chart.DrawingType == LineType.horizontal)
            {
                chart.DrawingType = LineType.none;
            }
            else
            {
                chart.SetLineType(2);//horizontal
                btnHLine.BackColor = Color.Red;
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            chart.IsDash = checkBox3.Checked;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            btnTLine.BackColor = Color.White;
            btnVLine.BackColor = Color.White;
            btnHLine.BackColor = Color.White;
            if (chart.DrawingType == LineType.trend)
            {
                chart.DrawingType = LineType.none;
            }
            else
            {
                chart.SetLineType(3);//trend
                btnTLine.BackColor = Color.Red;
            }
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            chart.SelectedLineDelete();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            bool isUpGoing = (rand.Next(0, 2) > 0);
            Bar datum;
            open = close;
            if (isUpGoing)
            {
                close = open + rand.NextDouble() / 50;
                datum = new Bar(DateTime.Now, close + rand.NextDouble() / 50, open - rand.NextDouble() / 50, open, close);
            }
            else
            {
                close = open - rand.NextDouble() / 50;
                datum = new Bar(DateTime.Now, open + rand.NextDouble() / 50, close - rand.NextDouble() / 50, open, close);
            }
            ChartPoint HighPoint = new ChartPoint(datum.Time, datum.High, Color.Red, 3);
            HighLine.Add(HighPoint);
            ChartPoint LowPoint = new ChartPoint(datum.Time, datum.Low, Color.Green, 3);
            LowLine.Add(LowPoint);
            barData.Add(datum);
            chart.Redraw(true);
        }
    }
}
