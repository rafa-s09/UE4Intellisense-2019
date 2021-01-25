﻿using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UE4SpecsUnitTests
{
    [TestClass]
    public class UE4Specifiers
    {
        [TestMethod]
        public void CheckRegExSpecifiersParse()
        {
            var input_string = "meta=(www,tr, ), a, s=2, dd, d";

            var specifiersList = new List<string>();
            var metaList = new List<string>();

            var matchSpecs = Regex.Matches(input_string, @"meta=\(\s*(([\w\d=]+)\,?)*\s*\)|([\w\d=]+)\,?", RegexOptions.IgnoreCase);

            foreach (var spec in matchSpecs)
            {
                var mm = (Match)spec;

                if (mm.Groups[2].Success)
                {
                    foreach (var ms in mm.Groups[2].Captures)
                    {
                        metaList.Add(((Capture)ms).Value);
                    }
                }

                if (mm.Groups[3].Success)
                    specifiersList.Add(mm.Groups[3].Value);
            }

            Assert.IsTrue(specifiersList.SequenceEqual(new List<string> { "a", "s=2", "dd", "d" }));
            Assert.IsTrue(metaList.SequenceEqual(new List<string> { "www", "tr" }));
        }
    }
}
