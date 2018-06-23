using System;
using System.Collections.Generic;
using System.Text;
using SharpMedia.Components.TextConsole;
using SharpMedia.Components.Configuration;
using System.IO;

namespace SharpMedia.Tools.Standard
{


    /// <summary>
    /// A line counter tool, works on files (not nodes).
    /// </summary>
    public class LineCounter : ITool
    {
        #region Private Members
        string baseDirectory = ".";
        string mask = "*.cs";
        ITextConsole console;
        SearchOption option = SearchOption.AllDirectories;
        #endregion

        #region Properties

        /// <summary>
        /// The console.
        /// </summary>
        [Required]
        public ITextConsole Console
        {
            get { return console; }
            set { console = value; }
        }

        /// <summary>
        /// Sets or gets base directory.
        /// </summary>
        [UI(FriendlyName="Base directory", 
            Description="The directory where files are searched for.",
            IsOptional=true)]
        public string BaseDirectory
        {
            get { return baseDirectory; }
            set { baseDirectory = value; }
        }

        /// <summary>
        /// The pattern of files you wish to line count.
        /// </summary>
        [UI(FriendlyName="Search pattern",
            Description="The pattern against which files are matched, wildcards allowed.",
            IsOptional=true)]
        public string Pattern
        {
            get { return mask; }
            set { mask = value; }
        }

        /// <summary>
        /// The search option.
        /// </summary>
        [UI(FriendlyName="Directory search options",
            Description="Options determine whether the subdirectories will also be included in search.",
            IsOptional=true)]
        public SearchOption SearchOptions
        {
            get { return option; }
            set { option = value; }
        }

        #endregion

        #region Helpers

        void ProcessFile(byte[] data, out uint lines, out uint bytes)
        {
            lines = 0;
            bytes = (uint)data.Length;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == '\n') lines++;
            }
        }


        #endregion

        #region ITool Members

        public int Run(params string[] args)
        {
            if (args.Length > 0)
            {
                console.Error.WriteLine("Unexpected arguments, ignoring them.");
            }

            uint lineCount = 0;
            uint byteCount = 0;
            uint fileCount = 0;

            foreach (string file in Directory.GetFiles(baseDirectory, mask, option))
            {
                fileCount++;

                // We read file.
                byte[] data = File.ReadAllBytes(file);
                uint currByteCount, currLineCount;

                // And process it.
                ProcessFile(data, out currLineCount, out currByteCount);
                lineCount += currLineCount;
                byteCount += currByteCount;

                // FIXME: may process and XML output later ... per file and summed basis
            }

            console.WriteLine("Line counting results:");
            console.WriteLine("  Number of files: {0}", fileCount);
            console.WriteLine("  Number of bytes: {0}", byteCount);
            console.WriteLine("  Number of lines: {0}", lineCount);

            return 0;
        }

        #endregion
    }
}
