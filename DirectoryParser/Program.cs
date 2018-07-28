using CsvHelper;
using DirectoryParser.Models.FileModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryParser
{
    class Program
    {
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();

        static void RecursiveMain(string[] args)
        {
            // Start with drives if you have to search the entire computer.
            string[] drives = System.Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(dr);

                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    Console.WriteLine("The drive {0} could not be read", di.Name);
                    continue;
                }
                System.IO.DirectoryInfo rootDir = di.RootDirectory;
                RecursiveFileSearch.WalkDirectoryTree(rootDir, log);
            }

            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");
            foreach (string s in log)
            {
                Console.WriteLine(s);
            }
        }

        static void StackBasedMain(string[] args)
        {
            // Specify the starting folder on the command line, or in 
            // Visual Studio in the Project > Properties > Debug pane.
            List<FileDescription> listFileDescriptions = StackBasedFileSearch.TraverseTree(args[0]);
            PrintResultsToCsv(listFileDescriptions);
        }

        static void PrintResultsToCsv (IEnumerable<Object> results)
        {
            String directoryForScanResults = ConfigurationManager.AppSettings["DirectoryToSaveResults"].ToString();
            DateTime now = System.DateTime.Now;
            using (StreamWriter textWriter = File.CreateText(directoryForScanResults + "\\ScanResults_" + now.ToString("yyyyMMdd-HHmmss") + ".csv"))
            {
                var csv = new CsvWriter(textWriter);
                csv.WriteRecords(results);
            }
        }

        static void Main(string[] args)
        {
            String directoryToScan = "";
            try
            {
                directoryToScan = ConfigurationManager.AppSettings["DirectoryToScan"].ToString();
            }
            catch (Exception e)
            {

            }
            if (directoryToScan != "")
            {
                Console.WriteLine("Scanning directory: " + directoryToScan);
                List<String> directoriesToProcess = new List<string> { directoryToScan };
                // Kick off either recursive or stack-based directory navigation
                //RecursiveMain(directoriesToProcess.ToArray());
                StackBasedMain(directoriesToProcess.ToArray());
            }
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
