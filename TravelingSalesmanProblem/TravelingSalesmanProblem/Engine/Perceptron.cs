using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelingSalesmanProblem.Interface;

namespace TravelingSalesmanProblem.Engine
{
    class Perceptron
    {
        #region Fields
        private PerceptronForm form;
        double threshold = 1;
        double learningRate = 0.1;
        // Init weights
        double[] weights = {0.0, 0.0};
        
        // AND function Training data
        int[][][] ANDtrainingData = new int[4][][] {
            new int[][] { new int[] {0,0} , new int[] {0}},
            new int[][] { new int[] {0,1} , new int[] {0}},
            new int[][] { new int[] {1,0} , new int[] {0}},
            new int[][] { new int[] {1,1} , new int[] {1}}
        };
        #endregion


        #region Constructors
        public Perceptron(PerceptronForm f)
        {
            form = f;
        }
        #endregion

        #region Public Methods
        public void AND()
        {
            // Start training loop
            while (true)
            {
                int errorCount = 0;
                // Loop over training data
                for (int i = 0; i < ANDtrainingData.Length; i++)
                {
                    Log("Starting weights: " + PrintWeights(weights), LogType.STATUS);
                    // Calculate weighted input
                    double weightedSum = 0;
                    for (int ii = 0; ii < ANDtrainingData[i][0].Length; ii++)
                    {
                        weightedSum += ANDtrainingData[i][0][ii] * weights[ii];
                    }

                    // Calculate output
                    int output = 0;
                    if (threshold <= weightedSum)
                    {
                        output = 1;
                    }

                    Log("Target output: " + ANDtrainingData[i][1][0] + ", "
                            + "Actual Output: " + output, LogType.STATUS);

                    // Calculate error
                    int error = ANDtrainingData[i][1][0] - output;

                    // Increase error count for incorrect output
                    if (error != 0)
                    {
                        errorCount++;
                    }

                    // Update weights
                    for (int ii = 0; ii < ANDtrainingData[i][0].Length; ii++)
                    {
                        weights[ii] += learningRate * error * ANDtrainingData[i][0][ii];
                    }

                    Log("New weights: " + PrintWeights(weights) + "\n", LogType.RESULT);
                }

                // If there are no errors, stop
                if (errorCount == 0)
                {
                    Log("Final weights: " + PrintWeights(weights), LogType.RESULT);
                    Log("Learning finished !", LogType.STATUS);
                    return;
                }
            }
        }
        #endregion

        #region Private Methods
        private void Log(string text, LogType type)
        {
            // Display a text in the Output log box. //
            System.Drawing.Color color = System.Drawing.Color.Black;

            switch (type)
            {

                case LogType.ERROR:
                    text = "[ERROR] " + text;
                    color = System.Drawing.Color.Red;
                    break;
                case LogType.RESULT:
                    text = "[RESULT] " + text;
                    color = System.Drawing.Color.Green;
                    break;
                case LogType.STATUS:
                    text = "[STATUS] " + text;
                    color = System.Drawing.Color.Black;
                    break;
            }


            int startIndex = form.Output.TextLength;
            form.Output.AppendText(text + "\r\n"); // add the text 
            form.Output.Select(form.Output.Text.Length, 0); // select the text that we just added
            form.Output.ScrollToCaret();           // scroll down to the end of the box
            form.Output.SelectionStart = startIndex;
            form.Output.SelectionLength = text.Length;
            form.Output.SelectionColor = color; // colorize the selected text
        }

        private string PrintWeights(double[] array)
        {
            // Convert weights array to a printable string
            string result = "";

            foreach (double d in array)
            {
                result += d.ToString() + " ";
            }
            return result;
        }
        #endregion


        #region LogType Enum
        private enum LogType
        {
            ERROR,
            STATUS,
            RESULT
        }
        #endregion
    }
}

        // OR function Training data
        int[][][] ORtrainingData = new int[4][][] {
            new int[][] { new int[] {0,0} , new int[] {0}},
            new int[][] { new int[] {0,1} , new int[] {1}},
            new int[][] { new int[] {1,0} , new int[] {1}},
            new int[][] { new int[] {1,1} , new int[] {1}}
        };
        #endregion


        #region Constructors
        public Perceptron(PerceptronForm f)
        {
            form = f;
        }
        #endregion

        #region Public Methods
        public void AND()
        {
            // Start training loop
            while (true)
            {
                int errorCount = 0;
                // Loop over training data
                for (int i = 0; i < ORtrainingData.Length; i++)
                {
                    Log("Starting weights: " + PrintWeights(weights), LogType.STATUS);
                    // Calculate weighted input
                    double weightedSum = 0;
                    for (int ii = 0; ii < ORtrainingData[i][0].Length; ii++)
                    {
                        weightedSum += ORtrainingData[i][0][ii] * weights[ii];
                    }

                    // Calculate output
                    int output = 0;
                    if (threshold <= weightedSum)
                    {
                        output = 1;
                    }

                    Log("Target output: " + ORtrainingData[i][1][0] + ", "
                            + "Actual Output: " + output, LogType.STATUS);

                    // Calculate error
                    int error = ORtrainingData[i][1][0] - output;

                    // Increase error count for incorrect output
                    if (error != 0)
                    {
                        errorCount++;
                    }

                    // Update weights
                    for (int ii = 0; ii < ORtrainingData[i][0].Length; ii++)
                    {
                        weights[ii] += learningRate * error * ORtrainingData[i][0][ii];
                    }

                    Log("New weights: " + PrintWeights(weights) + "\n", LogType.RESULT);
                }

                // If there are no errors, stop
                if (errorCount == 0)
                {
                    Log("Final weights: " + PrintWeights(weights), LogType.RESULT);
                    Log("Learning finished !", LogType.STATUS);
                    return;
                }
            }
        }
        #endregion








