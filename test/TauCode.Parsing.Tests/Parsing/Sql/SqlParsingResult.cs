using TauCode.Parsing.Tests.Parsing.Sql.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql;

public class SqlParsingResult : IParsingResult
{
    public List<object> Clauses { get; } = new();

    public void AddCreateClausePlaceholder()
    {
        this.Clauses.Add("CREATE");
    }

    public void ReplaceCreateClausePlaceholderWithCreateTableInfo()
    {
        var last = this.Clauses.Last();
        if ((string)last != "CREATE")
        {
            throw new Exception("Corrupted result.");
        }

        this.Clauses[^1] = new TableInfo();
    }

    public void ReplaceCreateClausePlaceholderWithCreateIndexInfo()
    {
        var last = this.Clauses.Last();
        if ((string)last != "CREATE")
        {
            throw new Exception("Corrupted result.");
        }

        this.Clauses[^1] = new IndexInfo();
    }

    public T GetLastClause<T>()
    {
        var lastClause = this.Clauses.Last();
        return (T)lastClause;
    }

    public int Version { get; private set; }

    public void IncreaseVersion()
    {
        this.Version++;
    }
}
