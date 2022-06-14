using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Serilog;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
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
                new WordProducer(),
                new SqlPunctuationProducer(),
                new IntegerProducer(IsAcceptableIntegerTerminator),
                new SqlIdentifierProducer(),
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
        var sqlGrammar = this.GetType().Assembly.GetResourceText("sql-grammar-smart.lisp", true);
        IGraphScriptReader scriptReader = new SqlGraphScriptReader();
        var graphMold = scriptReader.ReadScript(sqlGrammar.AsMemory());
        var vertexFactory = new SqlVertexFactory();
        IGraphBuilder graphBuilder = new GraphBuilder(vertexFactory);

        var graph = graphBuilder.Build(graphMold);

        IParser parser = new Parser
        {
            Root = (IdleNode)graph.Single(x => x.Name == "root-node"),
            Logger = _logger,
        };

        #region assign job to nodes

        // table
        var create = (ActionNode)graph.Single(x => x.Name == "create");
        create.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.AddCreateClausePlaceholder();

            parsingResult.IncreaseVersion();
        };

        var createTable = (ActionNode)graph.Single(x => x.Name == "do-create-table");
        createTable.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateTableInfo();

            parsingResult.IncreaseVersion();
        };

        var tableName = (ActionNode)graph.Single(x => x.Name == "table-name");
        tableName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            tableInfo.Name = ((TextToken)token).Text;

            parsingResult.IncreaseVersion();
        };

        var tableOpening = (ActionNode)graph.Single(x => x.Name == "table-opening");
        tableOpening.Action = (node, token, parsingResult) =>
        {
            parsingResult.IncreaseVersion();
        };

        var columnName = (ActionNode)graph.Single(x => x.Name == "column-name");
        columnName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = new ColumnInfo
            {
                Name = ((TextToken)token).Text,
            };
            tableInfo.Columns.Add(columnInfo);

            parsingResult.IncreaseVersion();
        };

        var typeName = (ActionNode)graph.Single(x => x.Name == "type-name");
        typeName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.TypeName = ((TextToken)token).Text;

            parsingResult.IncreaseVersion();
        };

        var precision = (ActionNode)graph.Single(x => x.Name == "precision");;
        precision.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.Precision = ((IntegerToken)token).Value.ToString().ToInt32();

            parsingResult.IncreaseVersion();
        };

        var scale = (ActionNode)graph.Single(x => x.Name == "scale");
        scale.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.Scale = ((IntegerToken)token).Value.ToString().ToInt32();

            parsingResult.IncreaseVersion();
        };

        var nullToken = (ActionNode)graph.Single(x => x.Name == "null");
        nullToken.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.IsNullable = true;

            parsingResult.IncreaseVersion();
        };

        var notNullToken = (ActionNode)graph.Single(x => x.Name == "not-null");;
        notNullToken.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var columnInfo = tableInfo.Columns.Last();
            columnInfo.IsNullable = false;

            parsingResult.IncreaseVersion();
        };

        var constraintName = (ActionNode)graph.Single(x => x.Name == "constraint-name");
        constraintName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            tableInfo.LastConstraintName = ((TextToken)token).Text;

            parsingResult.IncreaseVersion();
        };

        var pk = (ActionNode)graph.Single(x => x.Name == "do-primary-key");
        pk.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            tableInfo.PrimaryKey = new PrimaryKeyInfo
            {
                Name = tableInfo.LastConstraintName,
            };

            parsingResult.IncreaseVersion();
        };

        var pkColumnName = (ActionNode)graph.Single(x => x.Name == "pk-column-name");
        pkColumnName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var primaryKey = tableInfo.PrimaryKey;
            var indexColumn = new IndexColumnInfo
            {
                ColumnName = ((TextToken)token).Text,
            };
            primaryKey.Columns.Add(indexColumn);

            parsingResult.IncreaseVersion();
        };

        var pkColumnAscOrDesc = (ActionNode)graph.Single(x => x.Name == "pk-asc-or-desc");
        pkColumnAscOrDesc.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var primaryKey = tableInfo.PrimaryKey;
            var indexColumn = primaryKey.Columns.Last();

            indexColumn.SortDirection = Enum.Parse<SortDirection>(
                ((TextToken)token).Text.ToLowerInvariant(),
                true);

            parsingResult.IncreaseVersion();
        };

        var fk = (ActionNode)graph.Single(x => x.Name == "do-foreign-key");;
        fk.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = new ForeignKeyInfo
            {
                Name = tableInfo.LastConstraintName,
            };
            tableInfo.ForeignKeys.Add(foreignKey);

            parsingResult.IncreaseVersion();
        };

        var fkTableName = (ActionNode)graph.Single(x => x.Name == "fk-referenced-table-name");
        fkTableName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyTableName = ((TextToken)token).Text;
            foreignKey.ReferencedTableName = foreignKeyTableName;

            parsingResult.IncreaseVersion();
        };

        var fkColumnName = (ActionNode)graph.Single(x => x.Name == "fk-column-name");
        fkColumnName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyColumnName = ((TextToken)token).Text;
            foreignKey.ColumnNames.Add(foreignKeyColumnName);

            parsingResult.IncreaseVersion();
        };

        var fkReferencedColumnName = (ActionNode)graph.Single(x => x.Name == "fk-referenced-column-name");
        fkReferencedColumnName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var tableInfo = sqlParsingResult.GetLastClause<TableInfo>();
            var foreignKey = tableInfo.ForeignKeys.Last();
            var foreignKeyReferencedColumnName = ((TextToken)token).Text;
            foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);

            parsingResult.IncreaseVersion();
        };

        // index
        var createUniqueIndex = (ActionNode)graph.Single(x => x.Name == "do-create-unique-index");
        createUniqueIndex.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateIndexInfo();
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.IsUnique = true;

            sqlParsingResult.IncreaseVersion();
        };

        var createIndex = (ActionNode)graph.Single(x => x.Name == "do-create-non-unique-index");
        createIndex.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            sqlParsingResult.ReplaceCreateClausePlaceholderWithCreateIndexInfo();
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.IsUnique = false;

            sqlParsingResult.IncreaseVersion();
        };

        var indexName = (ActionNode)graph.Single(x => x.Name == "index-name");;
        indexName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.Name = ((TextToken)token).Text;

            sqlParsingResult.IncreaseVersion();
        };

        var indexTableName = (ActionNode)graph.Single(x => x.Name == "index-table-name");
        indexTableName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            index.TableName = ((TextToken)token).Text;

            sqlParsingResult.IncreaseVersion();
        };

        var indexColumnName = (ActionNode)graph.Single(x => x.Name == "index-column-name");
        indexColumnName.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            var columnInfo = new IndexColumnInfo
            {
                ColumnName = ((TextToken)token).Text,
            };
            index.Columns.Add(columnInfo);

            sqlParsingResult.IncreaseVersion();
        };

        var indexColumnAscOrDesc = (ActionNode)graph.Single(x => x.Name == "index-column-asc-or-desc");
        indexColumnAscOrDesc.Action = (node, token, parsingResult) =>
        {
            var sqlParsingResult = (SqlParsingResult)parsingResult;
            var index = sqlParsingResult.GetLastClause<IndexInfo>();
            var columnInfo = index.Columns.Last();
            columnInfo.SortDirection = Enum.Parse<SortDirection>(
                ((TextToken)token).Text.ToLowerInvariant(),
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

    private bool IsAcceptableIntegerTerminator(char c)
    {
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
}
