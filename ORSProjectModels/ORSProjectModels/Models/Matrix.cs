using System.Collections.Generic;

namespace ORSProjectModels
{
    public class Matrix
    {

        public int rows
        {
            get;
            protected set;
        }
        public int columns
        {
            get;
            protected set;
        }

        double[,] values;

        public double determinant
        {
            get;
            protected set;
        }
        public Matrix inverse
        {
            get;
            protected set;
        }

        private Matrix(double[,] _values, bool isInverse)
        {
            rows = _values.GetLength(0);
            columns = _values.GetLength(1);
            values = _values;
            determinant = CalculateDeterminant();
        }

        public Matrix(int _dimentions)
        {
            rows = _dimentions;
            columns = _dimentions;
            values = new double[_dimentions, _dimentions];
            for (int i = 0; i < _dimentions; i++)
            {
                for (int j = 0; j < _dimentions; j++)
                {
                    if (i == j)
                        values[i, j] = 1;
                    else
                        values[i, j] = 0;
                }
            }
            determinant = CalculateDeterminant();
        }

        public Matrix(double[,] _values)
        {
            rows = _values.GetLength(0);
            columns = _values.GetLength(1);
            values = _values;
            determinant = CalculateDeterminant();
            inverse = GetInverse();
        }

        double CalculateDeterminant()// n by n matrix
        {
            double[,] detMatrix = values;
            int n = values.GetLength(0);

            double det = 0;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    det = detMatrix[j, i] / detMatrix[i, i];
                    for (int k = i; k < n; k++)
                        detMatrix[j, k] = detMatrix[j, k] - det * detMatrix[i, k];
                }
            }

            det = 1;
            for (int i = 0; i < n; i++)
                det = det * detMatrix[i, i];
            return det;
        }

        Matrix GetInverse()//only 3 by 3 matrix
        {
            //Gaussian elimination
            int dimension = values.GetLength(0);
            double[,] matrix = (double[,])values.Clone();
            double[,] inverse = new double[dimension, dimension];

            for (int i = 0; i < dimension; i++)
                inverse[i, i] = 1.0;//set 1 in diagonal



            for (int i = 0; i < dimension; i++) // for each row
            {
                int currentRow = i;

                //pivioting best row
                double biggestSoFar = 0.0;
                int biggestRow = currentRow;
                for (int x = currentRow; x < dimension; x++)
                {
                    double sizeOfThis = absolute(matrix[x, currentRow]);
                    if (sizeOfThis > biggestSoFar)
                    {
                        biggestSoFar = sizeOfThis;
                        biggestRow = x;
                    }
                }

                if (biggestRow != currentRow)
                {
                    double temp;
                    for (int j = currentRow; j < dimension; j++)
                    {
                        temp = matrix[currentRow, j];
                        matrix[currentRow, j] = matrix[biggestRow, j];
                        matrix[biggestRow, j] = temp;
                    }
                    for (int j = 0; j < dimension; j++)
                    {
                        temp = inverse[currentRow, j];
                        inverse[currentRow, j] = inverse[biggestRow, j];
                        inverse[biggestRow, j] = temp;
                    }
                }

                for (int j = currentRow + 1; j < dimension; j++)
                {
                    // Matrix might be ill-conditioned.
                    if (matrix[currentRow, currentRow] == 0)
                    {
                        matrix[currentRow, currentRow] = 0;
                    }
                    double factor = matrix[j, currentRow] / matrix[currentRow, currentRow];

                    // Check that the lower elements are not already zero
                    if (factor != 0.0)
                    {
                        for (int k = currentRow; k < dimension; k++)
                            matrix[j, k] -= factor * matrix[currentRow, k];

                        for (int k = 0; k < dimension; k++)
                            inverse[j, k] -= factor * inverse[currentRow, k];
                    }
                }
                // Row complete
            }

            for (int j = dimension - 1; j >= 0; j--)
            {
                int currentRow = j;

                if (matrix[currentRow, currentRow] == 0)
                {
                    matrix[currentRow, currentRow] = 0;
                }

                for (int k = currentRow - 1; k >= 0; k--)
                {
                    double factor = matrix[k, currentRow] / matrix[currentRow, currentRow];

                    matrix[k, currentRow] = 0.0;

                    if (factor != 0.0)
                    {
                        for (int lp = 0; lp < dimension; lp++)
                        {
                            inverse[k, lp] -= factor * inverse[currentRow, lp];
                        }
                    }
                }
                //Row complete
            }

            for (int i = 0; i < dimension; i++)
            {
                double scale = matrix[i, i];
                for (int j = 0; j < dimension; j++)
                    inverse[i, j] /= scale;
            }

            return new Matrix(inverse, true);
        }

        public static Matrix operator *(Matrix rhs, Matrix lhs) // n by n matrix
        {
            double[,] a = rhs.values, b = lhs.values;
            double[,] c = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    c[i, j] = 0;
                    for (int k = 0; k < a.GetLength(1); k++)
                        c[i, j] = c[i, j] + a[i, k] * b[k, j];
                }
            }
            return new Matrix(c);
        }

        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row > rows)
                {
                    return default(double);
                } else if (column < 0 || column > columns)
                {
                    return default(double);
                }
                return values[row, column];
            }
            set
            {
                values[row, column] = value;
            }
        }

        public override string ToString()
        {
            string formatted = "";
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    formatted += " " + values[row, column].ToString();
                }
                formatted += "\n";
            }
            return formatted;
        }

        private double absolute(double absValue)
        {
            if (absValue < 0)
                absValue *= -1;
            return absValue;
        }

        private double[,] GetMatix()
        {
            return values;
        }

    }
}