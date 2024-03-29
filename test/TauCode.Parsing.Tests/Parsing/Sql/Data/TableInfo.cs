﻿using System.Text;

namespace TauCode.Parsing.Tests.Parsing.Sql.Data;

public class TableInfo
{
    public string Name { get; set; } = default!; // set by app
    public List<ColumnInfo> Columns { get; } = new();
    public PrimaryKeyInfo? PrimaryKey { get; set; }
    public List<ForeignKeyInfo> ForeignKeys { get; set; } = new();
    public string LastConstraintName { get; set; } = default!; // set by app

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE [{this.Name}](");
        for (var i = 0; i < this.Columns.Count; i++)
        {
            var column = this.Columns[i];
            sb.Append(column);
            if (i < this.Columns.Count - 1)
            {
                sb.AppendLine(",");
            }
        }

        if (this.PrimaryKey != null)
        {
            sb.AppendLine(",");
            sb.Append(this.PrimaryKey);
        }

        if (this.ForeignKeys.Count > 0)
        {
            sb.AppendLine(",");
            for (var i = 0; i < this.ForeignKeys.Count; i++)
            {
                var fk = this.ForeignKeys[i];
                sb.Append(fk);
                if (i < this.Columns.Count - 1)
                {
                    sb.AppendLine(",");
                }
            }
        }

        sb.Append(")");
        return sb.ToString();
    }
}