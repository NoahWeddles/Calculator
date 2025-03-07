using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        string line = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OperatorClick(object sender, RoutedEventArgs e)
        {
            line += (sender as Button).Content.ToString();
            txtDisplay.Text = line;
        }

        private void NumberClick(object sender, RoutedEventArgs e)
        {
            if (line == "0")
            {
                line = (sender as Button).Content.ToString();
                txtDisplay.Text = line;
                return;
            }
            line += (sender as Button).Content.ToString();
            txtDisplay.Text = line;
        }

        private void DotClick(object sender, RoutedEventArgs e)
        {
            line += (sender as Button).Content.ToString();
            txtDisplay.Text = line;
        }

        private void BackspaceClick(object sender, RoutedEventArgs e)
        {
            if (txtDisplay.Text.Length > 1)
            {
                line = line.Remove(line.Length - 1);
                txtDisplay.Text = line;
            }
            else
            {
                line = "0";
                txtDisplay.Text = line;
            }
        }

        private void EqualClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string processedLine = ProcessExpression(line);
                line = new DataTable().Compute(processedLine, null).ToString();
            }
            catch
            {
                line = "0";
            }
            txtDisplay.Text = line;
        }

        private string ProcessExpression(string expression)
        {
            // Handle power (^) using Math.Pow(a, b)
            expression = Regex.Replace(expression, @"(\d+(\.\d+)?)\s*\^\s*(\d+(\.\d+)?)", m =>
            {
                double baseNum = double.Parse(m.Groups[1].Value);
                double exponent = double.Parse(m.Groups[3].Value);
                return Math.Pow(baseNum, exponent).ToString();
            });

            // Handle modulus (%) since Compute does not support it
            expression = Regex.Replace(expression, @"(\d+)\s*%\s*(\d+)", m =>
            {
                int left = int.Parse(m.Groups[1].Value);
                int right = int.Parse(m.Groups[2].Value);
                return (left % right).ToString();
            });

            return expression;
        }
    }
}
