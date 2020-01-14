﻿using NUnit.Framework;
using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class PerformanceTests
    {

        [Test]
        [Ignore("Performance test")]
        public void PerformanceTestForTinyLispLexer()
        {
            ILexer tinyLispLexer = new /*Tiny-LispLexer()*/OmicronTinyLispLexer();
            var grammar = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);

            var start = DateTime.UtcNow;

            //var num = 10 * 1000;
            var num = 10 * 1000;
            for (var i = 0; i < num; i++)
            {
                tinyLispLexer.Lexize(grammar);
            }

            var end = DateTime.UtcNow;
            var seconds = (end - start).TotalSeconds;

            var perSecond = num / seconds;
            var msPerCall = seconds / num * 1000;

            //var k = 3;
        }

        public static void Run()
        {
            var tests = new PerformanceTests();
            tests.PerformanceTestForTinyLispLexer();
        }
    }
}
