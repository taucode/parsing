using System.Text;

namespace TauCode.Parsing.Tests.Parsing.Sql.Data;

public class IndexInfo
{
    public string Name { get; set; } = default!; // set by app
    public bool IsUnique { get; set; }
    public string TableName { get; set; } = default!; // set by app
    public List<IndexColumnInfo> Columns { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("CREATE");
        if (this.IsUnique)
        {
            sb.Append(" UNIQUE");
        }

        sb.Append(" INDEX");
        sb.Append($" [{this.Name}]");
        sb.Append(" ON ");
        sb.Append($"[{this.TableName}](");

        for (var i = 0; i < this.Columns.Count; i++)
        {
            var indexColumn = this.Columns[i];
            sb.Append($"[{indexColumn.ColumnName}] {indexColumn.SortDirection.ToString().ToUpperInvariant()}");

            if (i < this.Columns.Count - 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append(")");

        return sb.ToString();
    }
}