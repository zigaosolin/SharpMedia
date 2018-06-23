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

namespace LineCounter
{
    class Program
    {
        static string[] templateExt = new string[] { "*.template", "*.tcs" };
        static string[] sourceExt = new string[] { "*.cs", "*.cpp", "*.c" };
        static string[] xmlExt = new string[] { "*.appxml", "*.libxml", "*.xml", "*.ipkg" };

        


        static uint ProcessFile(string name, out uint bytes)
        {
            byte[] data = File.ReadAllBytes(name);

            bytes = (uint)data.Length;

            uint lines = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '\n') lines++;
            }

            return lines;
        }

        static void ProcessGroup(string path, string[] ext, out uint lines, out uint files, out uint bytes)
        {
            lines = 0;
            files = 0;
            bytes = 0;
            for (int i = 0; i < ext.Length; i++)
            {
                
                foreach (string file in Directory.GetFiles(path, ext[i], SearchOption.AllDirectories))
                {
                    files++;
                    uint fileBytes;
                    lines += ProcessFile(file, out fileBytes);
                    bytes += fileBytes;
                }
            }
        }

        static void Main(string[] args)
        {
            if(args.Length != 1) throw new Exception("One argument, path expected.");


            uint templateLines = 0;
            uint templateFiles = 0;
            uint templateBytes = 0;
            uint sourceLines = 0;
            uint sourceFiles = 0;
            uint sourceBytes = 0;
            uint xmlLines = 0;
            uint xmlFiles = 0;
            uint xmlBytes = 0;

            // We do it.
            ProcessGroup(args[0], templateExt, out templateLines, out templateFiles, out templateBytes);
            ProcessGroup(args[0], sourceExt, out sourceLines, out sourceFiles, out sourceBytes);
            ProcessGroup(args[0], xmlExt, out xmlLines, out xmlFiles, out xmlBytes);

            Console.WriteLine("Template Files: {0,4}   Template Lines: {1,6}   Template KB: {2,5}", templateFiles, templateLines, templateBytes / 1024);
            Console.WriteLine("Source Files:   {0,4}   Source Lines:   {1,6}   Source KB:   {2,5}", sourceFiles, sourceLines, sourceBytes / 1024);
            Console.WriteLine("Xml Files:      {0,4}   Xml Lines:      {1,6}   Xml KB:      {2,5}", xmlFiles, xmlLines, xmlBytes / 1024);
            Console.WriteLine("All Files:      {0,4}   All Lines:      {1,6}   All KB:      {2,5}", templateFiles + sourceFiles + xmlFiles, 
                templateLines + sourceLines + xmlLines, (templateBytes + sourceBytes + xmlBytes)/1024);

            Console.ReadKey();
        }
    }
}
