﻿namespace Gu.Persist.SystemXml.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Gu.Persist.Core.Tests;
    using Gu.Persist.SystemXml;

    using NUnit.Framework;

    [Explicit("Longrunning benchmarks")]
    public class BenchmarkTests
    {
        private TypicalSetting setting1;
        private TypicalSetting setting2;

        public static ComparerData[] ComparerSource { get; } =
        {
            new ComparerData(XmlEqualsComparer<TypicalSetting>.Default, 1000)
        };

        [SetUp]
        public void SetUp()
        {
            this.setting1 = new TypicalSetting
            {
                Name = "Johan Larsson",
                Dummies = Enumerable.Range(0, 1).Select(x => new DummySerializable(x)).ToList(),
                Value1 = 1.2,
                Value2 = 2,
                Value3 = 3,
                Value4 = 4,
                Value5 = 5
            };
            this.setting2 = new TypicalSetting
            {
                Name = "Johan Larsson",
                Dummies = Enumerable.Range(0, 1).Select(x => new DummySerializable(x)).ToList(),
                Value1 = 1.2,
                Value2 = 2,
                Value3 = 3,
                Value4 = 4,
                Value5 = 5
            };
        }

        [TestCaseSource(nameof(ComparerSource))]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void XmlEquals(ComparerData data)
        {
            var comparer = XmlEqualsComparer<TypicalSetting>.Default;
            var warmup = comparer.Equals(this.setting1, this.setting2);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < data.Times; i++)
            {
                var result = comparer.Equals(this.setting1, this.setting2);
            }

            sw.Stop();
            Console.WriteLine(
                "{0}.Equals(_setting1, _setting2) {1} times took: {2} ms total ({3} ms each)",
                data.ComparerName,
                data.Times,
                sw.ElapsedMilliseconds,
                sw.Elapsed.TotalMilliseconds / data.Times);
        }

        [TestCaseSource(nameof(ComparerSource))]
        [SuppressMessage("ReSharper", "UnusedVariable")]
        public void GetHashCode(ComparerData data)
        {
            var comparer = XmlEqualsComparer<TypicalSetting>.Default;
            var warmup = comparer.GetHashCode(this.setting1);
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < data.Times; i++)
            {
                var result = comparer.GetHashCode(this.setting1);
            }

            sw.Stop();
            Console.WriteLine(
                "{0}.GetHashCode(_setting1) {1} times took: {2} ms total ({3} ms each)",
                data.ComparerName,
                data.Times,
                sw.ElapsedMilliseconds,
                sw.Elapsed.TotalMilliseconds / data.Times);
        }

        public class ComparerData
        {
            public readonly IEqualityComparer<TypicalSetting> Comparer;
            public readonly int Times;

            public ComparerData(IEqualityComparer<TypicalSetting> comparer, int times)
            {
                this.Comparer = comparer;
                this.Times = times;
            }

            public string ComparerName => $"{this.Comparer.GetType().Name.Replace("`1", string.Empty)}<{typeof(TypicalSetting).Name}>";

            public override string ToString()
            {
                return $"{this.ComparerName} {this.Times} times";
            }
        }
    }
}
