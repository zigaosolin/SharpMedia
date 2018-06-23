// This file constitutes a part of the SharpMedia project, (c) 2007 by the SharpMedia team
// and is licensed for your use under the conditions of the NDA or other legally binding contract
// that you or a legal entity you represent has signed with the SharpMedia team.
// In an event that you have received or obtained this file without such legally binding contract
// in place, you MUST destroy all files and other content to which this lincese applies and
// contact the SharpMedia team for further instructions at the internet mail address:
//
//    legal@sharpmedia.com
//
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LicenceInserter
{
    class Program
    {
        static bool ContainsLicence(string filename)
        {
            StreamReader reader = new StreamReader(File.OpenRead(filename));
            try
            {
                string line = reader.ReadLine();
                if (line.Contains("//")) return true;
                return false;
            }
            finally
            {
                reader.Dispose();
            }
        }

        static void DoBackup(string filename)
        {
            File.Copy(filename, filename + ".backup");
        }

        static void RemoveBackup(string filename)
        {
            File.Delete(filename + ".backup");
        }

        static void InsertLicence(string filename, string licence)
        {
            bool containsLicence = ContainsLicence(filename);

            StreamReader streamReader = new StreamReader(File.OpenRead(filename));
            string contents = streamReader.ReadToEnd();
            streamReader.Dispose();

            StreamWriter streamWriter = new StreamWriter(File.Open(filename, FileMode.Truncate));
            streamWriter.Write(licence);
            streamWriter.Write(Environment.NewLine);

            if (containsLicence)
            {
                // We skip through licence.
                int start = 0, newStart;
                while (true)
                {
                    newStart = contents.IndexOf(Environment.NewLine, start);
                    if (newStart == -1)
                    {
                        newStart = 0;
                        break;
                    }
                    if (!contents.Substring(start, newStart - start).Contains("//")) break;
                    start = newStart + Environment.NewLine.Length;
                }

                contents = contents.Substring(start);

            }
            streamWriter.Write(contents);
            
            streamWriter.Dispose();
        }

        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Usage: licence-file [ignore paths])");
                return;
            }

            Console.WriteLine("Welcome to licence inserter, please select licence file: ");

            // We read licence.
            StreamReader licenceReader = new StreamReader(File.OpenRead(args[0]));
            string licence = licenceReader.ReadToEnd();
            licenceReader.Dispose();



            string[] allFiles = Directory.GetFiles("./", "*.cs", SearchOption.AllDirectories);
            foreach(string filename in allFiles)
            {
                bool br = false;
                for (int i = 1; i < args.Length; i++)
                {
                    if (filename.Contains(args[i]))
                    {
                        br = true;
                        break;
                    }
                }

                if (br) continue;

                // Check if licence is already in it.
                Console.WriteLine("Processing {0} ...", filename);
                DoBackup(filename);
                InsertLicence(filename, licence);
                RemoveBackup(filename);
                


            }

            Console.WriteLine("End processing ...");

            Console.Read();
        }
    }
}
