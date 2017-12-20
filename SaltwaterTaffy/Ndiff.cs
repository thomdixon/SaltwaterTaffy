using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SaltwaterTaffy
{
    public enum NdiffFlag
    {
        Text,
        Xml,
        Verbose,
        Help,
    }

    /// <summary>
    ///     Class which represents command-line options for ndiff
    /// </summary>
    public class NdiffOptions : IDictionary<NdiffFlag, string>
    {
        #region NdiffFlagToOption

        private readonly Dictionary<NdiffFlag, string> _NdiffFlagToOption = new Dictionary<NdiffFlag, string>
            {
                {NdiffFlag.Text, "--text"},
                {NdiffFlag.Xml, "--xml"},
                {NdiffFlag.Verbose, "-v"},
                {NdiffFlag.Help, "-h"}
            };

        #endregion

        #region NmapOptionToFlag

        private readonly Dictionary<string, NdiffFlag> _ndiffOptionToFlag = new Dictionary<string, NdiffFlag>
            {
                {"--text", NdiffFlag.Text},
                {"--xml", NdiffFlag.Xml},
                {"-v", NdiffFlag.Verbose},
                {"-h", NdiffFlag.Help}
            };

        #endregion

        private readonly Dictionary<string, string> _ndiffOptions;

        public NdiffOptions()
        {
            _ndiffOptions = new Dictionary<string, string>();
        }

        public void Add(KeyValuePair<NdiffFlag, string> kvp)
        {
            Add(kvp.Key, kvp.Value);
        }

        public void Clear()
        {
            _ndiffOptions.Clear();
        }

        public bool Contains(KeyValuePair<NdiffFlag, string> item)
        {
            return _ndiffOptions.Contains(new KeyValuePair<string, string>(_NdiffFlagToOption[item.Key], item.Value));
        }

        public void CopyTo(KeyValuePair<NdiffFlag, string>[] array, int arrayIndex)
        {
            _ndiffOptions.Select(x => new KeyValuePair<NdiffFlag, string>(_ndiffOptionToFlag[x.Key], x.Value))
                        .ToArray()
                        .CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<NdiffFlag, string> item)
        {
            return _ndiffOptions.Remove(_NdiffFlagToOption[item.Key]);
        }

        public int Count
        {
            get { return _ndiffOptions.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(NdiffFlag key)
        {
            return _ndiffOptions.ContainsKey(_NdiffFlagToOption[key]);
        }

        public void Add(NdiffFlag flag, string argument)
        {
            string option = _NdiffFlagToOption[flag];

            if (_ndiffOptions.ContainsKey(option))
            {
                _ndiffOptions[option] = string.Format("{0},{1}", _ndiffOptions[option], argument);
            }
            else
            {
                _ndiffOptions.Add(option, argument);
            }
        }

        public bool Remove(NdiffFlag key)
        {
            return _ndiffOptions.Remove(_NdiffFlagToOption[key]);
        }

        public bool TryGetValue(NdiffFlag key, out string value)
        {
            return _ndiffOptions.TryGetValue(_NdiffFlagToOption[key], out value);
        }

        public string this[NdiffFlag key]
        {
            get { return _ndiffOptions[_NdiffFlagToOption[key]]; }
            set { _ndiffOptions[_NdiffFlagToOption[key]] = value; }
        }

        public ICollection<NdiffFlag> Keys
        {
            get { return _ndiffOptions.Select(x => _ndiffOptionToFlag[x.Key]).ToArray(); }
        }

        public ICollection<string> Values
        {
            get { return _ndiffOptions.Values; }
        }


        public IEnumerator<KeyValuePair<NdiffFlag, string>> GetEnumerator()
        {
            return
                _ndiffOptions.Select(x => new KeyValuePair<NdiffFlag, string>(_ndiffOptionToFlag[x.Key], x.Value))
                            .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(NdiffFlag flag)
        {
            Add(flag, string.Empty);
        }

        public void AddAll(IEnumerable<NdiffFlag> flags)
        {
            foreach (NdiffFlag flag in flags)
            {
                Add(flag);
            }
        }

        public void AddAll(IEnumerable<KeyValuePair<NdiffFlag, string>> kvps)
        {
            foreach (var kvp in kvps)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        public override string ToString()
        {
            return
                _ndiffOptions.Aggregate(new StringBuilder(), (sb, kvp) => sb.AppendFormat("{0} {1} ", kvp.Key, kvp.Value),
                                       sb => sb.ToString()).Trim();
        }
    }
    
    public class NdiffException : ApplicationException
    {
        public NdiffException(string ex) : base(ex)
        {
        }
    }

    public class NdiffContext
    {
        /// <summary>
        ///     By default we try to find the path to the ndiff executable by searching the path and the ndiff options are empty.
        /// </summary>
        public NdiffContext()
        {
            //using NmapContext to get the path so as to not duplicate code for finding the executable in the PATH
            Path = new NmapContext().GetPathToNdiff();
            Options = new NdiffOptions();
        }

        /// <summary>
        ///     The path to the ndiff executable
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     The specified ndiff options
        /// </summary>
        public NdiffOptions Options { get; set; }

        /// <summary>
        ///     Original file to compare. FILE1 as per ndiff -h
        /// </summary>
        public string File1 { get; set; }

        /// <summary>
        ///     Original file to compare. FILE2 as per ndiff -h
        /// </summary>
        public string File2 { get; set; }

        /// <summary>
        ///     Run the Ndiff tool to compare output files from Nmap.
        /// </summary>
        /// <returns>The Ndiff comparison output.</returns>
        public string Run()
        {
            if (string.IsNullOrEmpty(Path) || !File.Exists(Path))
            {
                throw new ApplicationException("Path to ndiff is invalid");
            }

            if (string.IsNullOrEmpty(File1))
            {
                throw new ApplicationException("Attempted run on missing comparison File1.");
            }

            if (string.IsNullOrEmpty(File2))
            {
                throw new ApplicationException("Attempted run on missing comparison File2.");
            }

            if (Options == null)
            {
                throw new ApplicationException("Ndiff options null");
            }

            string output, error;

            using (var process = new Process())
            {
                process.StartInfo.FileName = Path;
                process.StartInfo.Arguments = string.Format("{0} {1} {2}", Options, File1, File2);
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                process.WaitForExit();

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }
            

            return string.IsNullOrEmpty(error) ? output : error;
        }
    }
}
