using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    internal static class SqlHelper
    {
        private static readonly HashSet<string> ReservedWords = new HashSet<string>
        {
            "CREATE",
            "TABLE",
            "UNIQUE",
            "INDEX",
            "ASC",
            "DESC",
            "PRIMARY",
            "KEY",
            "FOREIGN",
            "CONSTRAINT",
            "NOT",
            "NULL",
            "DEFAULT",
            "REFERENCES",
            "ON",
        };

        internal static bool IsReservedWord(string text) => ReservedWords.Contains(text.ToUpperInvariant());
    }
}
