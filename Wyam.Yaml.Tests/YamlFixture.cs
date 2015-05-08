﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Wyam.Core;
using Wyam.Core.Modules;
using Wyam.Extensibility;
using YamlDotNet.Dynamic;

namespace Wyam.Yaml.Tests
{
    [TestFixture]
    public class YamlFixture
    {
        [Test]
        public void SetsMetadataKey()
        {
            // Given
            IDocument document = Substitute.For<IDocument>();
            IEnumerable<KeyValuePair<string, object>> items = null;
            document
                .When(x => x.Clone(Arg.Any<IEnumerable<KeyValuePair<string, object>>>()))
                .Do(x => items = x.Arg<IEnumerable<KeyValuePair<string, object>>>());
            document.Content.Returns(@"A: 1");
            Yaml yaml = new Yaml("MyYaml");

            // When
            yaml.Execute(new [] { document }, null).ToList();  // Make sure to materialize the result list

            // Then
            document.Received().Clone(Arg.Any<IEnumerable<KeyValuePair<string, object>>>());
            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("MyYaml", items.First().Key);
        }

        [Test]
        public void GeneratesDynamicObject()
        {
            // Given
            IDocument document = Substitute.For<IDocument>();
            IEnumerable<KeyValuePair<string, object>> items = null;
            document
                .When(x => x.Clone(Arg.Any<IEnumerable<KeyValuePair<string, object>>>()))
                .Do(x => items = x.Arg<IEnumerable<KeyValuePair<string, object>>>());
            document.Content.Returns(@"
A: 1
B: true
C: Yes
");
            Yaml yaml = new Yaml("MyYaml");

            // When
            yaml.Execute(new[] { document }, null).ToList();  // Make sure to materialize the result list

            // Then
            document.Received().Clone(Arg.Any<IEnumerable<KeyValuePair<string, object>>>());
            Assert.AreEqual(1, items.Count());
            Assert.IsInstanceOf<DynamicYaml>(items.First().Value);
            Assert.AreEqual(1, (int)((dynamic)items.First().Value).A);
            Assert.AreEqual(true, (bool)((dynamic)items.First().Value).B);
            Assert.AreEqual("Yes", (string)((dynamic)items.First().Value).C);
        }
    }
}
