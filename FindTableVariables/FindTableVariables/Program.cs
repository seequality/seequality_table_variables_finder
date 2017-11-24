using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using FindTableVariables;

namespace FindTableVariables
{

    class Program
    {

 

        static void Main(string[] args)
        {

            string path = @"C:\GIT\database";
            string delimeter = " > ";

            Console.WriteLine("Start");

            var tableVariable = FindTableVariables.ReturnTableVariableFromDirectory(path, delimeter);
            var tableVariableFlattaned = FindTableVariables.ReturnTableVariableFromDirectory(path, delimeter, true);

            File.WriteAllText("tableVariable_stats.txt", tableVariable);
            File.WriteAllText("tableVariableFlattaned_stats.txt", tableVariable);

            Console.WriteLine("Done");
            Console.ReadLine();
        }




    }

}
