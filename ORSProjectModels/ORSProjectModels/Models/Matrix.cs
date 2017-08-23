using System.Collections.Generic;

namespace ORSProjectModels
{
    public class Matrix<T>
    {

        public int rows{
            get;
            protected set;
        }
        public int columns
        {
            get;
            protected set;
        }

        T[,] values;

        public double determinant
        {
            get;
            protected set;
        }
        Matrix<T> inverse;
        Matrix<T> cofactor;

        public Matrix()
        {
            values = new T[0, 0];
            rows = values.GetLength(0);
            columns = values.GetLength(1);
        }

        public Matrix(int _rows = 0,int _columns = 0)
        {
            rows = _rows;
            columns = _columns;
            values = new T[_rows, _columns];
            determinant = CalculateDeterminant();
            inverse = GetInverse();
            cofactor = GetCofactor();
        }

        public Matrix(T[,] _values)
        {
            rows = _values.GetLength(0);
            columns = _values.GetLength(1);
            values = _values;
            determinant = CalculateDeterminant();
            inverse = GetInverse();
            cofactor = GetCofactor();
        }

        double CalculateDeterminant()
        {
            return 0;
        }

        Matrix<T> GetInverse()
        {
            return null;
        }

        Matrix<T> GetCofactor()
        {
            return null;
        }

        public static Matrix<T> operator *(Matrix<T> rhs, Matrix<T> lhs)
        {
            return null;
        }

        public T this[int row,int column]{
            get
            {
                if (row < 0 || row > rows)
                {
                    return default(T);
                } else if (column < 0 || column > columns)
                {
                    return default(T);
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

    }
}