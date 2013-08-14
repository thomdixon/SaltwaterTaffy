using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SaltwaterTaffy.Test
{
    [TestClass]
    public class NmapOptionsUnitTests
    {
        [TestMethod]
        public void when_option_is_added_then_keys_contains_flag()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, string.Empty);

            Assert.IsTrue(no.ContainsKey(NmapFlag.A));
        }

        [TestMethod]
        public void when_option_is_added_then_values_contains_arguments()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, "All they have is bow-dow-bow-dow-bow-dow-do-dee-do-dow-dee-do");
            Assert.IsTrue(no.Values.Contains("All they have is bow-dow-bow-dow-bow-dow-do-dee-do-dow-dee-do"));
        }

        [TestMethod]
        public void when_option_is_added_then_key_retrieves_arguments()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, "Heartbreak under the streetlights");
            Assert.AreEqual("Heartbreak under the streetlights", no[NmapFlag.A]);
        }

        [TestMethod]
        public void when_existing_option_is_added_then_arguments_are_comma_separated()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, "A");
            no.Add(NmapFlag.A, "B");
            Assert.AreEqual("A,B", no[NmapFlag.A]);
        }

        [TestMethod]
        public void when_ToString_is_called_with_one_option_specified_then_output_is_formatted_correctly()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, "A");
            Assert.AreEqual("-A A", no.ToString());
        }

        [TestMethod]
        public void when_ToString_is_called_with_multiple_options_specified_then_output_is_formatted_correctly()
        {
            var no = new NmapOptions();
            no.Add(NmapFlag.A, "A");
            no.Add(NmapFlag.SourcePortG, "B");
            
            Assert.AreEqual("-A A -g B", no.ToString());
        }
    }
}
