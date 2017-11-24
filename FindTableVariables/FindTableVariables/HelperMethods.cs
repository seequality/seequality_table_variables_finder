using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindTableVariables
{
    internal class FindTableVariables
    {
        private static List<FileWithTableVariable> FindTableVariableInDirectory(string directoryPath)
        {
            List<string> allFiles = new List<string>();
            List<FileWithTableVariable> allFilesWithTableVariables = new List<FileWithTableVariable>();

            foreach (string file in Directory.EnumerateFiles(directoryPath, "*.sql", SearchOption.AllDirectories))
            {
                allFiles.Add(file);
            }

            foreach (var file in allFiles)
            {
                var txt = File.ReadAllText(file);
                var _txt = Regex.Replace(txt, @"\s+", " ")
                                        .ToLower()
                                        .Replace(System.Environment.NewLine, " ")
                                        .Replace("\t", " ")
                                        .Replace("\n", " ")
                                        .Replace("\r", " ")
                                        .Replace("  ", " ");

                var words = _txt.Split(' ');
                int n = words.Count();

                for (int i = 0; i < n; i++)
                {

                    if (words[i] == "declare" && words[i + 1].StartsWith("@") && words[i + 2] == "table")
                    {
                        allFilesWithTableVariables.Add(new FileWithTableVariable() { FileName = file, VariableName = words[i + 1] });
                    }
                }
            }

            return allFilesWithTableVariables;

        }
        
        /// <summary>
        /// Return table variables from all *.sql files in given directory
        /// Will return one file path and one variable in one line
        /// If given script/file has more than one table variable it will return multiple lines per file
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        internal static string ReturnTableVariableFromDirectory(string directoryPath, string delimeter)
        {
            return string.Join(Environment.NewLine, FindTableVariableInDirectory(directoryPath).Select(x=> x.FileName + delimeter + x.VariableName).ToList());
        }

        /// <summary>
        /// Return table variables from all *.sql files in given directory
        /// Will return one file path and all variables in one line
        /// If given script/file has more than one table variable it will return single line per file
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="singleLineMethod"></param>
        /// <param name="delimeter"></param>
        internal static string ReturnTableVariableFromDirectory(string directoryPath, string delimeter, bool singleLineMethod)
        {
            List<FileWithTableVariable> allFilesWithTableVariables = new List<FileWithTableVariable>();
            List<string> allFilesWithTableVariablesOutput = new List<string>();

            if (singleLineMethod)
            {
                allFilesWithTableVariables = FindTableVariableInDirectory(directoryPath);

                foreach (var stat in allFilesWithTableVariables)
                {
                    if (allFilesWithTableVariablesOutput.Any(x => x.StartsWith(stat.FileName)))
                    {
                        var indexPosition = allFilesWithTableVariablesOutput
                            .IndexOf(allFilesWithTableVariablesOutput
                            .Where(x => x.StartsWith(stat.FileName))
                            .First());

                        allFilesWithTableVariablesOutput[indexPosition] = allFilesWithTableVariablesOutput
                            .Where(x => x.StartsWith(stat.FileName))
                            .First() + ", " + stat.VariableName;
                    }
                    else
                    {
                        allFilesWithTableVariablesOutput.Add(stat.FileName + delimeter + stat.VariableName);
                    }
                }
                return string.Join(Environment.NewLine, allFilesWithTableVariablesOutput).ToString();
            }
            else
            {
                return string.Join(Environment.NewLine, FindTableVariableInDirectory(directoryPath).Select(x => x.FileName + delimeter + x.VariableName).ToList());
            }
        }

    }
}
