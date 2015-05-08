﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Wyam.Core.Configuration;

namespace Wyam.Core.Tests
{
    [TestFixture]
    public class ConfiguratorFixture
    {
        [Test]
        public void GetConfigPartsReturnsBothPartsWithDelimiter()
        {
            // Given
            Engine engine = new Engine();
            Configurator configurator = new Configurator(engine);
            string configScript = @"A-
-B
---
-C
D";

            // When
            Tuple<string, string> configParts = configurator.GetConfigParts(configScript);

            // Then
            Assert.AreEqual(@"A-
-B", configParts.Item1);
            Assert.AreEqual(@"-C
D", configParts.Item2);
        }

        [Test]
        public void GetConfigPartsReturnsBothPartsWithDelimiterWithTrailingSpaces()
        {
            // Given
            Engine engine = new Engine();
            Configurator configurator = new Configurator(engine);
            string configScript = @"A-
-B
---  
-C
D";

            // When
            Tuple<string, string> configParts = configurator.GetConfigParts(configScript);

            // Then
            Assert.AreEqual(@"A-
-B", configParts.Item1);
            Assert.AreEqual(@"-C
D", configParts.Item2);
        }

        [Test]
        public void GetConfigPartsReturnsConfigWithDelimiterWithLeadingSpaces()
        {
            // Given
            Engine engine = new Engine();
            Configurator configurator = new Configurator(engine);
            string configScript = @"A-
-B
  ---
-C
D";

            // When
            Tuple<string, string> configParts = configurator.GetConfigParts(configScript);

            // Then
            Assert.IsNull(configParts.Item1);
            Assert.AreEqual(@"A-
-B
  ---
-C
D", configParts.Item2);
        }

        [Test]
        public void GetConfigPartsReturnsBothPartsWithDelimiterWithExtraLines()
        {
            // Given
            Engine engine = new Engine();
            Configurator configurator = new Configurator(engine);
            string configScript = @"A-
-B

---

-C
D";

            // When
            Tuple<string, string> configParts = configurator.GetConfigParts(configScript);

            // Then
            Assert.AreEqual(@"A-
-B", configParts.Item1);
            Assert.AreEqual(@"-C
D", configParts.Item2);
        }

        [Test]
        public void GetConfigPartsReturnsConfigWithoutDelimiter()
        {
            // Given
            Engine engine = new Engine();
            Configurator configurator = new Configurator(engine);
            string configScript = @"A-
-B
C";

            // When
            Tuple<string, string> configParts = configurator.GetConfigParts(configScript);

            // Then
            Assert.IsNull(configParts.Item1);
            Assert.AreEqual(@"A-
-B
C", configParts.Item2);
            
        }
    }
}
