using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHandlers;

namespace Targets
{
    public interface ITargetFileReader
    {
        List<Target> read();
        string FileName { get; set; }
    }

    public class INITargetFileReader : ITargetFileReader
    {
        private string m_filename;
        // Exceptions
        public class InvalidHeader : Exception { }
        public class InvalidName : Exception { }
        public class InvalidTarget : Exception { }
        string ITargetFileReader.FileName
        {
            get
            {
                return m_filename;
            }
            set
            {
                m_filename = value;
            }
        }
        private FileReader Reader;
        /*
         * ValidateName() check to see if the name contains a space.
         * If it does, throw InvalidName(), otherwise return the name.
         */
        private string ValidateName(string name)
        {
            if (name.Contains(' '))
            {
                throw new InvalidName();
            }
            else
                return name;
        }
        /*
         * ValidateCoord() attempt to convert a string to a double.
         * For coordinates X, Y, and Z
         * Throws FormatException if it fails.
         */
        private double ValidateCoord(string coord)
        {
            return (Convert.ToDouble(coord));
        }
        /*
         * ValidateInteger() attemp to convert a string to an integer.
         * For Points, Friend, and FlashRate.
         * Throws FormatException if it fails.
         */
        private int ValidateInteger(string value)
        {
            return (Convert.ToInt32(value));
        }
        /*
         * ValidateFriend() Attempt to convert a string to a boolean.
         * Throws InvalidTarget if the string is not either "true" or "false"
         */
        private bool ValidateFriend(string friend)
        {
            friend = friend.ToLower();
            friend.Replace("  ", "");
            bool friendVal;
            if (friend == "true")
                friendVal = true;
            else if (friend == "false")
                friendVal = false;
            else
                throw new InvalidTarget();
            return friendVal;
        }

        /*
         * INITargetFileReader() constructor
         */
        public INITargetFileReader(string fileName)
        {
            m_filename = fileName;
            Reader = new INIReader(fileName);
        }

        /*
         * Read target file
         */
        List<Target> ITargetFileReader.read()
        {
            List<Target> targetList = new List<Target>();
            var nodes = Reader.getNodes("target");

            foreach (var node in nodes)
            {
                try
                {
                    // Attempt to get all the information for a target and convert
                    // to the correct file types
                    string name = ValidateName(node.Contents["name"]);
                    double x = ValidateCoord(node.Contents["x"]);
                    double y = ValidateCoord(node.Contents["y"]);
                    double z = ValidateCoord(node.Contents["z"]);
                    int points = ValidateInteger(node.Contents["points"]);
                    int flashRate = ValidateInteger(node.Contents["flashrate"]);
                    bool friend = ValidateFriend(node.Contents["friend"]);
                    targetList.Add(new Target(name, x, y, z, friend, points, flashRate));
                }
                catch (KeyNotFoundException)
                {
                    // the target was missing a field.
                    throw new InvalidTarget();
                }
            }
            return targetList;
        }
    }

    public class TargetFileReaderFactory
    {
        public static ITargetFileReader Create(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            ITargetFileReader reader = null;
            switch(extension)
            {
                // create the correct TargetFileReader given the file type
                case ".ini":
                    reader = new INITargetFileReader(fileName);
                    break;
            }
            return reader;
        }
    }
}
