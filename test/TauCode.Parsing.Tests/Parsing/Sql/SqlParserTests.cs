using NUnit.Framework;
using Serilog;
using System;
using System.IO;
using System.Linq;
using TauCode.Data.Text;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Sql.Data;
using TauCode.Parsing.Tests.Parsing.Sql.Producers;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TokenProducers;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql;

[TestFixture]
public class SqlParserTests
{
    private ILexer _tinyLispLexer;
    private ILexer _sqlLexer;
    private ILogger _logger;
    private StringWriter _writer;

    [SetUp]
    public void SetUp()
    {
        _tinyLispLexer = new TinyLispLexer();
        _sqlLexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new Int32Producer(IsAcceptableIntegerTerminator),
                new WordProducer(WordTerminator),
                new SqlPunctuationProducer(),
                new SqlIdentifierProducer(SqlHelper.IsReservedWord, WordTerminator)
                {
                    Delimiter =
                        SqlIdentifierDelimiter.None |
                        SqlIdentifierDelimiter.Brackets |
                        0,
                },
            },
        };

        _writer = new StringWriter();
        _logger = new LoggerConfiguration()
            .MinimumLevel
            .Verbose()
            .WriteTo
            .TextWriter(_writer)
            .CreateLogger();
    }

    [Test]
    public void SqlParser_ValidInput_Parses()
    {
        // Arrange
        var sqlGrammar = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
        IGraphScriptReader scriptReader = new SqlGraphScriptReader();
        var graphMold = scriptReader.ReadScript(sqlGrammar.AsMemory());
        var vertexFactory = new SqlVertexFactory();
        IGraphBuilder graphBuilder = new GraphBuilder(vertexFactory);

        var graph = graphBuilder.Build(graphMold);

        IParser parser = new Parser
        {
            Root = (IdleNode)graph.Single(x => x.Name == "root-node"),
            Logger = _logger,
            AllowsMultipleExecutions = true,
        };

        #region assign job to nodes

        // table
        var createNode = (ActionNode)graph.Single(x => x.Name == "create");
        createNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.AddCreateClausePlaceholder();

            parsingResult.IncreaseVersion();
        };

        var createTableNode = (ActionNode)graph.Single(x => x.Name == "do-create-table");
        createTableNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateTableInfo();

            parsingResult.IncreaseVersion();
        };

        var tableNameNode = (ActionNode)graph.Single(x => x.Name == "table-name");
        tableNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();

            tableInfo.Name = GetSqlNameFromToken(token);

            parsingResult.IncreaseVersion();
        };

        var tableOpeningNode = (ActionNode)graph.Single(x => x.Name == "table-opening");
        tableOpeningNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            parsingResult.IncreaseVersion();
        };

        var columnNameNode = (ActionNode)graph.Single(x => x.Name == "column-name");
        columnNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = new ColumnInfo
            {
                Name = GetSqlNameFromToken(token),
            };
            tableInfo.Columns.Add(columnInfo);

            parsingResult.IncreaseVersion();
        };

        var typeNameNode = (ActionNode)graph.Single(x => x.Name == "type-name");
        typeNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.TypeName = GetSqlNameFromToken(token);

            parsingResult.IncreaseVersion();
        };

        var precisionNode = (ActionNode)graph.Single(x => x.Name == "precision"); ;
        precisionNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.Precision = ((Int32Token)token).Value.ToString().ToInt32();

            parsingResult.IncreaseVersion();
        };

        var scaleNode = (ActionNode)graph.Single(x => x.Name == "scale");
        scaleNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.Scale = ((Int32Token)token).Value.ToString().ToInt32();

            parsingResult.IncreaseVersion();
        };

        var nullTokenNode = (ActionNode)graph.Single(x => x.Name == "null");
        nullTokenNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.IsNullable = true;

            parsingResult.IncreaseVersion();
        };

        var notNullTokenNode = (ActionNode)graph.Single(x => x.Name == "not-null"); ;
        notNullTokenNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.IsNullable = false;

            parsingResult.IncreaseVersion();
        };

        var constraintNameNode = (ActionNode)graph.Single(x => x.Name == "constraint-name");
        constraintNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            tableInfo.LastConstraintName = GetSqlNameFromToken(token);

            parsingResult.IncreaseVersion();
        };

        var pkNode = (ActionNode)graph.Single(x => x.Name == "do-primary-key");
        pkNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            tableInfo.PrimaryKey = new PrimaryKeyInfo
            {
                Name = tableInfo.LastConstraintName,
            };

            parsingResult.IncreaseVersion();
        };

        var pkColumnNameNode = (ActionNode)graph.Single(x => x.Name == "pk-column-name");
        pkColumnNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var primaryKey = tableInfo.PrimaryKey;
            var indexColumn = new IndexColumnInfo
            {
                ColumnName = GetSqlNameFromToken(token),
            };
            primaryKey.Columns.Add(indexColumn);

            parsingResult.IncreaseVersion();
        };

        var pkColumnAscOrDescNode = (ActionNode)graph.Single(x => x.Name == "pk-asc-or-desc");
        pkColumnAscOrDescNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var primaryKey = tableInfo.PrimaryKey;
            var indexColumn = primaryKey.Columns.Last();

            indexColumn.SortDirection = Enum.Parse<SortDirection>(
                ((TextTokenBase)token).Text.ToLowerInvariant(),
                true);

            parsingResult.IncreaseVersion();
        };

        var fkNode = (ActionNode)graph.Single(x => x.Name == "do-foreign-key"); ;
        fkNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = new ForeignKeyInfo
            {
                Name = tableInfo.LastConstraintName,
            };
            tableInfo.ForeignKeys.Add(foreignKey);

            parsingResult.IncreaseVersion();
        };

        var fkTableNameNode = (ActionNode)graph.Single(x => x.Name == "fk-referenced-table-name");
        fkTableNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyTableName = GetSqlNameFromToken(token);
            foreignKey.ReferencedTableName = foreignKeyTableName;

            parsingResult.IncreaseVersion();
        };

        var fkColumnNameNode = (ActionNode)graph.Single(x => x.Name == "fk-column-name");
        fkColumnNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyColumnName = GetSqlNameFromToken(token);
            foreignKey.ColumnNames.Add(foreignKeyColumnName);

            parsingResult.IncreaseVersion();
        };

        var fkReferencedColumnNameNode = (ActionNode)graph.Single(x => x.Name == "fk-referenced-column-name");
        fkReferencedColumnNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyReferencedColumnName = GetSqlNameFromToken(token);
            foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);

            parsingResult.IncreaseVersion();
        };

        // index
        var createUniqueIndexNode = (ActionNode)graph.Single(x => x.Name == "do-create-unique-index");
        createUniqueIndexNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateIndexInfo();
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.IsUnique = true;

            sqlParsingResult.IncreaseVersion();
        };

        var createIndexNode = (ActionNode)graph.Single(x => x.Name == "do-create-non-unique-index");
        createIndexNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateIndexInfo();
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.IsUnique = false;

            sqlParsingResult.IncreaseVersion();
        };

        var indexNameNode = (ActionNode)graph.Single(x => x.Name == "index-name"); ;
        indexNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.Name = GetSqlNameFromToken(token);

            sqlParsingResult.IncreaseVersion();
        };

        var indexTableNameNode = (ActionNode)graph.Single(x => x.Name == "index-table-name");
        indexTableNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.TableName = GetSqlNameFromToken(token);

            sqlParsingResult.IncreaseVersion();
        };

        var indexColumnNameNode = (ActionNode)graph.Single(x => x.Name == "index-column-name");
        indexColumnNameNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            var columnInfo = new IndexColumnInfo
            {
                ColumnName = GetSqlNameFromToken(token),
            };
            index.Columns.Add(columnInfo);

            sqlParsingResult.IncreaseVersion();
        };

        var indexColumnAscOrDescNode = (ActionNode)graph.Single(x => x.Name == "index-column-asc-or-desc");
        indexColumnAscOrDescNode.Action = (node, parsingContext) =>
        {
            var parsingResult = parsingContext.ParsingResult;
            var token = parsingContext.GetCurrentToken();

            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            var columnInfo = index.Columns.Last();
            columnInfo.SortDirection = Enum.Parse<SortDirection>(
                ((TextTokenBase)token).Text.ToLowerInvariant(),
                true);

            sqlParsingResult.IncreaseVersion();
        };

        #endregion

        var sql =
            @"
            CREATE Table my_tab(
                id int NOT NULL,
                name varchar(30) NOT NULL,
                Salary decimal(12, 3) NULL,
                CONSTRAINT [my_tab_pk] PRIMARY KEY(id Desc, [NAME] ASC, salary),
                CONSTRAINT [fk_other] FOREIGN KEY([id]) references other_table(otherId),
                CONSTRAINT fk_cool FOREIGN KEY([id], name) references [other_table](otherId, [birthday])
            )

            CREATE TABLE [other_table](
                [otherId] nvarchar(10),
                [birthday] [datetime],
                CONSTRAINT pk_otherTable PRIMARY KEY([otherId])
            )

            CREATE UNIQUE INDEX [UX_name] ON my_tab(id Desc, name, Salary asc)

            CREATE INDEX IX_id ON [my_tab](id)

            CREATE INDEX [IX_Salary] ON my_tab([salary])

            ";

        var sqlTokens = _sqlLexer.Tokenize(sql.AsMemory());

        // Act
        var result = new SqlParsingResult();

        try
        {
            parser.Parse(sqlTokens, result);
        }
        catch (Exception e)
        {
            // todo
            var log = _writer.ToString();
            throw;
        }

        var sqlResults = result.Clauses;

        // Assert
        Assert.That(sqlResults, Has.Count.EqualTo(5));

        // create table: my_tab
        var createTableResult = (TableInfo)sqlResults[0];
        Assert.That(createTableResult.Name, Is.EqualTo("my_tab"));
        var tableColumns = createTableResult.Columns;
        Assert.That(tableColumns, Has.Count.EqualTo(3));

        var column = tableColumns[0];
        Assert.That(column.Name, Is.EqualTo("id"));
        Assert.That(column.TypeName, Is.EqualTo("int"));
        Assert.That(column.Precision, Is.Null);
        Assert.That(column.Scale, Is.Null);
        Assert.That(column.IsNullable, Is.False);

        column = tableColumns[1];
        Assert.That(column.Name, Is.EqualTo("name"));
        Assert.That(column.TypeName, Is.EqualTo("varchar"));
        Assert.That(column.Precision, Is.EqualTo(30));
        Assert.That(column.Scale, Is.Null);
        Assert.That(column.IsNullable, Is.False);

        column = tableColumns[2];
        Assert.That(column.Name, Is.EqualTo("Salary"));
        Assert.That(column.TypeName, Is.EqualTo("decimal"));
        Assert.That(column.Precision, Is.EqualTo(12));
        Assert.That(column.Scale, Is.EqualTo(3));
        Assert.That(column.IsNullable, Is.True);

        var tablePrimaryKey = createTableResult.PrimaryKey;
        Assert.That(tablePrimaryKey.Name, Is.EqualTo("my_tab_pk"));
        var pkColumns = tablePrimaryKey.Columns;
        Assert.That(pkColumns, Has.Count.EqualTo(3));

        var pkIndexColumn = pkColumns[0];
        Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("id"));
        Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Desc));

        pkIndexColumn = pkColumns[1];
        Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("NAME"));
        Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

        pkIndexColumn = pkColumns[2];
        Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("salary"));
        Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

        var foreignKeys = createTableResult.ForeignKeys;
        Assert.That(foreignKeys, Has.Count.EqualTo(2));

        var tableForeignKey = foreignKeys[0];
        Assert.That(tableForeignKey.Name, Is.EqualTo("fk_other"));
        Assert.That(tableForeignKey.ReferencedTableName, Is.EqualTo("other_table"));
        CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] { "id" });
        CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] { "otherId" });

        tableForeignKey = foreignKeys[1];
        Assert.That(tableForeignKey.Name, Is.EqualTo("fk_cool"));
        Assert.That(tableForeignKey.ReferencedTableName, Is.EqualTo("other_table"));
        CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] { "id", "name" });
        CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] { "otherId", "birthday" });

        // create table: other_table
        createTableResult = (TableInfo)sqlResults[1];
        Assert.That(createTableResult.Name, Is.EqualTo("other_table"));
        tableColumns = createTableResult.Columns;
        Assert.That(tableColumns, Has.Count.EqualTo(2));

        column = tableColumns[0];
        Assert.That(column.Name, Is.EqualTo("otherId"));
        Assert.That(column.TypeName, Is.EqualTo("nvarchar"));
        Assert.That(column.Precision, Is.EqualTo(10));
        Assert.That(column.Scale, Is.Null);
        Assert.That(column.IsNullable, Is.True);

        column = tableColumns[1];
        Assert.That(column.Name, Is.EqualTo("birthday"));
        Assert.That(column.TypeName, Is.EqualTo("datetime"));
        Assert.That(column.Precision, Is.Null);
        Assert.That(column.Scale, Is.Null);
        Assert.That(column.IsNullable, Is.True);

        tablePrimaryKey = createTableResult.PrimaryKey;
        Assert.That(tablePrimaryKey.Name, Is.EqualTo("pk_otherTable"));
        pkColumns = tablePrimaryKey.Columns;
        Assert.That(pkColumns, Has.Count.EqualTo(1));

        pkIndexColumn = pkColumns[0];
        Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("otherId"));
        Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

        foreignKeys = createTableResult.ForeignKeys;
        Assert.That(foreignKeys, Is.Empty);

        // create index: UX_name
        var createIndexResult = (IndexInfo)sqlResults[2];
        Assert.That(createIndexResult.Name, Is.EqualTo("UX_name"));
        Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
        Assert.That(createIndexResult.IsUnique, Is.True);

        var indexColumns = createIndexResult.Columns;
        Assert.That(indexColumns, Has.Count.EqualTo(3));

        var indexColumnInfo = indexColumns[0];
        Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("id"));
        Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Desc));

        indexColumnInfo = indexColumns[1];
        Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("name"));
        Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

        indexColumnInfo = indexColumns[2];
        Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("Salary"));
        Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

        // create index: IX_id
        createIndexResult = (IndexInfo)sqlResults[3];
        Assert.That(createIndexResult.Name, Is.EqualTo("IX_id"));
        Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
        Assert.That(createIndexResult.IsUnique, Is.False);

        indexColumns = createIndexResult.Columns;
        Assert.That(indexColumns, Has.Count.EqualTo(1));

        indexColumnInfo = indexColumns[0];
        Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("id"));
        Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

        // create index: UX_name
        createIndexResult = (IndexInfo)sqlResults[4];
        Assert.That(createIndexResult.Name, Is.EqualTo("IX_Salary"));
        Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
        Assert.That(createIndexResult.IsUnique, Is.False);

        indexColumns = createIndexResult.Columns;
        Assert.That(indexColumns, Has.Count.EqualTo(1));

        indexColumnInfo = indexColumns[0];
        Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("salary"));
        Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));
    }

    private bool IsAcceptableIntegerTerminator(ReadOnlySpan<char> span, int index)
    {
        var c = span[index];

        if (c.IsInlineWhiteSpaceOrCaretControl())
        {
            return true;
        }

        if (c.IsIn('(', ')', ','))
        {
            return true;
        }

        return false;
    }

    private bool WordTerminator(ReadOnlySpan<char> input, int pos)
    {
        var c = input[pos];

        var result =
            c.IsInlineWhiteSpaceOrCaretControl() ||
            c.IsIn('(', ')', ',') ||
            false;

        return result;
    }

    // todo: wordToken cannot start with digit

    private static string GetSqlNameFromToken(ILexicalToken token)
    {
        if (token is WordToken wordToken)
        {
            return wordToken.Text;
        }
        else if (token is SqlIdentifierToken sqlIdentifierToken)
        {
            return sqlIdentifierToken.Value.Value;
        }

        throw new Exception("Not an SQL name token");
    }

    //if (token is WordToken wordToken)
    //{
    //    tableName = wordToken.Text;
    //}
    //else if (token is SqlIdentifierToken sqlIdentifierToken)
    //{
    //    tableName = sqlIdentifierToken.Value.Value;
    //}
    //else
    //{
    //    throw new Exception();
    //}


}
