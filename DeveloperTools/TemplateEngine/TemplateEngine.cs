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

namespace TemplateEngine
{
    /// <summary>
    /// A templating engine.
    /// </summary>
    class TemplateEngine
    {

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Expected one argument, the source of XML project to process.");
                return;
            }

            try
            {
                int idx = args[0].LastIndexOf('/');
                string dir, projfile;

                if(idx >= 0)
                {
                    dir = args[0].Substring(0, idx);
                    projfile = args[0].Substring(idx + 1);
                } else {
                    dir = ".";
                    projfile = args[0];
                }
                

                Directory.SetCurrentDirectory(dir);
                TemplateProject project = TemplateProject.Parse(projfile);

                project.Process();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: {0}", ex);
            }

            Console.ReadKey();

        }
    }
}
