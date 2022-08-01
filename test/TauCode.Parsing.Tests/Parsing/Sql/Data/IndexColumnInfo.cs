namespace TauCode.Parsing.Tests.Parsing.Sql.Data;

public class IndexColumnInfo
{
    public string ColumnName { get; set; } = default!; // set by app
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}
