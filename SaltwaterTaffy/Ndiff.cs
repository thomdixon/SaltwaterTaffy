using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SaltwaterTaffy
{
    public static class Ndiff
    {
        /// <summary>
        ///     Run the Ndiff tool to compare output files from Nmap.
        /// </summary>
        /// <param name="file1">Original File</param>
        /// <param name="file2">Modified File</param>
        /// <param name="verbose">Also show hosts and ports that haven't changed.</param>
        /// <param name="xml">Display output in XML format.</param>
        /// <returns>Output from Ndiff application.</returns>
        public static string RunDiff(bool verbose, bool xml, string file1, string file2)
        {
            try
            {
                string output, error;
                var nmap = new NmapContext();

                var sVerbose = verbose == true ? "-v" : "";
                var sXml = xml == true ? "--xml" : "--text";

                using (var process = new Process())
                {
                    //thanks for the code T30 @ stackoverflow
                    //https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
                    process.StartInfo.FileName = nmap.GetPathToNdiff();
                    process.StartInfo.Arguments = string.Format("{0} {1} {2} {3}", sVerbose, sXml, file1, file2);
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    //* Read the output (or the error)
                    output = process.StandardOutput.ReadToEnd();
                    error = process.StandardError.ReadToEnd();

                    process.WaitForExit();
                }


                return string.IsNullOrEmpty(error) ? output : error;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to run diff. Ex: {0}", ex.ToString()));
            }
        }
    }
}
