using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Tests.Parsing.Sql.Data;

public class ForeignKeyInfo
{
    public string Name { get; set; }
    public string ReferencedTableName { get; set; }
    public List<string> ColumnNames { get; set; } = new List<string>();
    public List<string> ReferencedColumnNames { get; set; } = new List<string>();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"    CONSTRAINT [{this.Name}] FOREIGN KEY(");

        for (var i = 0; i < this.ColumnNames.Count; i++)
        {
            var columnName = this.ColumnNames[i];
            sb.Append($"[{columnName}]");

            if (i < this.ColumnNames.Count - 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append(") REFERENCES ");
        sb.Append(this.ReferencedTableName);
        sb.Append(" (");

        for (var i = 0; i < this.ReferencedColumnNames.Count; i++)
        {
            var referencedColumnName = this.ReferencedColumnNames[i];
            sb.Append($"[{referencedColumnName}]");

            if (i < this.ReferencedColumnNames.Count - 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append(")");
        return sb.ToString();
    }
}