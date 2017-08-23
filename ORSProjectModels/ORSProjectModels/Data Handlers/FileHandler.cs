using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ORSProjectModels
{
    public static class FileHandler
    {

        /// <summary>
        /// Reads the file into a list,one line is a separate list item
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>A list of strings containing the file's content</returns>
        public static List<string> Read(string filePath)
        {
            List<string> holder = new List<string>();
            FileStream streamFile;
            StreamReader reader;
            if (File.Exists(filePath))
            {
                streamFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            } else
            {
                throw new Exception(string.Format("File not Found. File requested = {0}", filePath));
            }
            reader = new StreamReader(streamFile);
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                holder.Add(line);
            }
            reader.Close();
            streamFile.Close();
            return holder;
        }

        /// <summary>
        /// Writes a single string to the file(The string must be preformmatted)
        /// </summary>
        /// <param name="data">String to write to the file</param>
        /// <param name="filePath">Path to the file</param>
        public static void Write(string data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            writer.Write(data);
            writer.Close();
            streamFile.Close();
        }

        /// <summary>
        /// Writes the contents of a string array to the file
        /// </summary>
        /// <param name="data">The string array</param>
        /// <param name="filePath">Path to the file</param>
        public static void Write(string[] data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            foreach (string line in data)
            {
                writer.WriteLine(line);
            }
            writer.Close();
            streamFile.Close();
        }

        /// <summary>
        /// Writes the contents of a list of strings to the file
        /// </summary>
        /// <param name="data">The list of strings</param>
        /// <param name="filePath">Path to the file</param>
        public static void Write(List<string> data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            foreach (string line in data)
            {
                writer.WriteLine(line);
            }
            writer.Close();
            streamFile.Close();
        }

        /// <summary>
        /// Appends a string to the file
        /// </summary>
        /// <param name="data">The string to append</param>
        /// <param name="filePath">Path to the file</param>
        public static void Append(string data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            writer.Write(data);
            writer.Close();
            streamFile.Close();
        }

        /// <summary>
        /// Appends the contents of a string array to the file
        /// </summary>
        /// <param name="data">The string array to append</param>
        /// <param name="filePath">Path to the file</param>
        public static void Append(string[] data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            foreach (string line in data)
            {
                writer.WriteLine(line);
            }
            writer.Close();
            streamFile.Close();
        }

        /// <summary>
        /// Appends the content of a list of strings to the file
        /// </summary>
        /// <param name="data">The list of strings to append</param>
        /// <param name="filePath">Path to the file</param>
        public static void Append(List<string> data, string filePath)
        {
            FileStream streamFile = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(streamFile);
            foreach (string line in data)
            {
                writer.WriteLine(line);
            }
            writer.Close();
            streamFile.Close();
        }

    }
}