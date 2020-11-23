using System;

namespace XmlToTxt
{
    class Program
    {     
        static void Main(string[] args)
        {
            //Stores the given file name
            string _fileName = null; 

            // Check whether the user provided a file name, if yes, assign it to the private variable
            if (args.Length > 0) _fileName = args[0];
            // else display a proper message in a console
            else Console.WriteLine("Any arguments weren't given (a file name is needed).");

            //If _filename variable is not null or empty load and parse xml, the result write in .txt file
            if(!string.IsNullOrEmpty(_fileName))
                Utilities.WriteToTxtFile(Utilities.XmlParser(_fileName));

            Console.ReadLine();
        }
    }
}
