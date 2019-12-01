﻿using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Data;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Parsing
{
    [TestFixture]
    public class SqlParserTestsViaLisp
    {
        [Test]
        public void SqlParser_ValidInput_Parses()
        {
            // Arrange
            //var input = this.GetType().Assembly.GetResourceText("SQLiteRealGrammar.txt", true);
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            //var aideLexer = new AideLexer();
            //IParser parser = new Parser();
            ILexer lexer2 = new TinyLispLexer();
            var tokens = lexer2.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var freshRoot = builder.Build(list);
            //root.FetchTree();

            IParser freshParser = new Parser();

            //var tokens = aideLexer.Lexize(input);
            //var aideRoot = AideHelper.BuildParserRoot();
            //var results = parser.Parse(aideRoot, tokens);

            //IBuilder builder = new Builder();

            //var sqlRoot = builder.Build("sql tree", results.Cast<BlockDefinitionResult>());


            var allSqlNodes = freshRoot.FetchTree();

            var exactWordNodes = allSqlNodes
                .Where(x => x is ExactWordNode)
                .Cast<ExactWordNode>()
                .ToList();

            foreach (var exactWordNode in exactWordNodes)
            {
                exactWordNode.IsCaseSensitive = false;
            }

            var reservedWords = exactWordNodes
                .Select(x => x.Word)
                .Distinct()
                .Select(x => x.ToUpperInvariant())
                .ToHashSet();

            //var todoWtf = allSqlNodes.Where(x => x.Name != null).ToList();

            var identifiersAsWords = allSqlNodes
                .Where(x =>
                    x is WordNode wordNode &&
                    x.Name.EndsWith("-name-word", StringComparison.InvariantCultureIgnoreCase))
                .Cast<WordNode>()
                .ToList();

            #region assign job to nodes

            // table
            var createTable = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "do-create-table", StringComparison.InvariantCultureIgnoreCase));
            createTable.Action = (token, accumulator) =>
            {
                var tableInfo = new TableInfo();
                accumulator.AddResult(tableInfo);
            };

            var tableName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "table-name-ident", StringComparison.InvariantCultureIgnoreCase));
            tableName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.Name = ((IdentifierToken) token).Identifier;
            };

            var tableNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "table-name-word", StringComparison.InvariantCultureIgnoreCase));
            tableNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.Name = ((WordToken) token).Word;
            };

            var columnName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "column-name-ident", StringComparison.InvariantCultureIgnoreCase));
            columnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = new ColumnInfo
                {
                    Name = ((IdentifierToken) token).Identifier,
                };
                tableInfo.Columns.Add(columnInfo);
            };

            var columnNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "column-name-word", StringComparison.InvariantCultureIgnoreCase));
            columnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = new ColumnInfo
                {
                    Name = ((WordToken) token).Word,
                };
                tableInfo.Columns.Add(columnInfo);
            };

            var typeName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "type-name-ident", StringComparison.InvariantCultureIgnoreCase));
            typeName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((IdentifierToken) token).Identifier;
            };

            var columnTypeWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "type-name-word", StringComparison.InvariantCultureIgnoreCase));
            columnTypeWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((WordToken) token).Word;
            };

            var precision = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "precision", StringComparison.InvariantCultureIgnoreCase));
            precision.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Precision = ((IntegerToken) token).IntegerValue.ToInt32();
            };

            var scale = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "scale", StringComparison.InvariantCultureIgnoreCase));
            scale.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Scale = ((IntegerToken) token).IntegerValue.ToInt32();
            };

            var nullToken = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "null", StringComparison.InvariantCultureIgnoreCase));
            nullToken.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = true;
            };

            var notNullToken = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "not-null", StringComparison.InvariantCultureIgnoreCase));
            notNullToken.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = false;
            };

            var constraintName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "constraint-name-ident", StringComparison.InvariantCultureIgnoreCase));
            constraintName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.LastConstraintName = ((IdentifierToken) token).Identifier;
            };

            var constraintNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "constraint-name-word", StringComparison.InvariantCultureIgnoreCase));
            constraintNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.LastConstraintName = ((WordToken) token).Word;
            };

            var pk = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "do-primary-key", StringComparison.InvariantCultureIgnoreCase));
            pk.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.PrimaryKey = new PrimaryKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
            };

            var pkColumnName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "pk-column-name-ident", StringComparison.InvariantCultureIgnoreCase));
            pkColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = new IndexColumnInfo
                {
                    ColumnName = ((IdentifierToken) token).Identifier,
                };
                primaryKey.Columns.Add(indexColumn);
            };

            var pkColumnNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "pk-column-name-word", StringComparison.InvariantCultureIgnoreCase));
            pkColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = new IndexColumnInfo
                {
                    ColumnName = ((WordToken) token).Word,
                };
                primaryKey.Columns.Add(indexColumn);
            };

            var pkColumnAsc = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "asc", StringComparison.InvariantCultureIgnoreCase));
            pkColumnAsc.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();
                indexColumn.SortDirection = SortDirection.Asc;
            };

            var pkColumnDesc = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "desc", StringComparison.InvariantCultureIgnoreCase));
            pkColumnDesc.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();
                indexColumn.SortDirection = SortDirection.Desc;
            };

            var fk = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "do-foreign-key", StringComparison.InvariantCultureIgnoreCase));
            fk.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = new ForeignKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
                tableInfo.ForeignKeys.Add(foreignKey);
            };

            var fkTableName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-referenced-table-name-ident", StringComparison.InvariantCultureIgnoreCase));
            fkTableName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyTableName = ((IdentifierToken) token).Identifier;
                foreignKey.TableName = foreignKeyTableName;
            };

            var fkTableNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-referenced-table-name-word", StringComparison.InvariantCultureIgnoreCase));
            fkTableNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyTableName = ((WordToken) token).Word;
                foreignKey.TableName = foreignKeyTableName;
            };

            var fkColumnName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-column-name-ident", StringComparison.InvariantCultureIgnoreCase));
            fkColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyColumnName = ((IdentifierToken) token).Identifier;
                foreignKey.ColumnNames.Add(foreignKeyColumnName);
            };

            var fkColumnNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-column-name-word", StringComparison.InvariantCultureIgnoreCase));
            fkColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyColumnName = ((WordToken) token).Word;
                foreignKey.ColumnNames.Add(foreignKeyColumnName);
            };

            var fkReferencedColumnName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-referenced-column-name-ident", StringComparison.InvariantCultureIgnoreCase));
            fkReferencedColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyReferencedColumnName = ((IdentifierToken) token).Identifier;
                foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);
            };

            var fkReferencedColumnNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "fk-referenced-column-name-word", StringComparison.InvariantCultureIgnoreCase));
            fkReferencedColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyReferencedColumnName = ((WordToken) token).Word;
                foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);
            };

            // index
            var createUniqueIndex = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "do-create-unique-index", StringComparison.InvariantCultureIgnoreCase));
            createUniqueIndex.Action = (token, accumulator) =>
            {
                var index = new IndexInfo
                {
                    IsUnique = true,
                };
                accumulator.AddResult(index);
            };

            var createIndex = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "do-create-index", StringComparison.InvariantCultureIgnoreCase));
            createIndex.Action = (token, accumulator) =>
            {
                bool brandNewIndex;

                if (accumulator.Count == 0)
                {
                    brandNewIndex = true;
                }
                else
                {
                    var result = accumulator.Last();
                    if (result is IndexInfo indexInfo)
                    {
                        brandNewIndex = indexInfo.IsCreationFinalized;
                    }
                    else
                    {
                        brandNewIndex = true;
                    }
                }

                if (brandNewIndex)
                {
                    var newIndex = new IndexInfo
                    {
                        IsCreationFinalized = true,
                    };

                    accumulator.AddResult(newIndex);
                }
                else
                {
                    var existingIndexInfo = accumulator.GetLastResult<IndexInfo>();
                    existingIndexInfo.IsCreationFinalized = true;
                }
            };

            var indexName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-name-ident", StringComparison.InvariantCultureIgnoreCase));
            indexName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.Name = ((IdentifierToken) token).Identifier;
            };

            var indexNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-name-word", StringComparison.InvariantCultureIgnoreCase));
            indexNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.Name = ((WordToken) token).Word;
            };

            var indexTableName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-table-name-ident", StringComparison.InvariantCultureIgnoreCase));
            indexTableName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.TableName = ((IdentifierToken) token).Identifier;
            };

            var indexTableNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-table-name-word", StringComparison.InvariantCultureIgnoreCase));
            indexTableNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.TableName = ((WordToken) token).Word;
            };

            var indexColumnName = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-column-name-ident", StringComparison.InvariantCultureIgnoreCase));
            indexColumnName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = new IndexColumnInfo
                {
                    ColumnName = ((IdentifierToken) token).Identifier,
                };
                index.Columns.Add(columnInfo);
            };

            var indexColumnNameWord = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-column-name-word", StringComparison.InvariantCultureIgnoreCase));
            indexColumnNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = new IndexColumnInfo
                {
                    ColumnName = ((WordToken) token).Word,
                };
                index.Columns.Add(columnInfo);
            };

            var indexColumnAsc = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-column-asc", StringComparison.InvariantCultureIgnoreCase));
            indexColumnAsc.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = index.Columns.Last();
                columnInfo.SortDirection = SortDirection.Asc;
            };

            var indexColumnDesc = (ActionNode) allSqlNodes.Single(x =>
                string.Equals(x.Name, "index-column-desc", StringComparison.InvariantCultureIgnoreCase));
            indexColumnDesc.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = index.Columns.Last();
                columnInfo.SortDirection = SortDirection.Desc;
            };

            #endregion

            foreach (var identifiersAsWord in identifiersAsWords)
            {
                identifiersAsWord.AdditionalChecker = (token, accumulator) =>
                {
                    var wordToken = ((WordToken) token).Word.ToUpperInvariant();
                    return !reservedWords.Contains(wordToken);
                };
            }

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
            var sqlLexer = new SqlLexer();
            var sqlTokens = sqlLexer.Lexize(sql);

            // Act
            var sqlResults = freshParser.Parse(freshRoot, sqlTokens);

            // Assert
            Assert.That(sqlResults, Has.Length.EqualTo(5));

            // create table: my_tab
            var createTableResult = (TableInfo) sqlResults[0];
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
            Assert.That(tableForeignKey.TableName, Is.EqualTo("other_table"));
            CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] {"id"});
            CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] {"otherId"});

            tableForeignKey = foreignKeys[1];
            Assert.That(tableForeignKey.Name, Is.EqualTo("fk_cool"));
            Assert.That(tableForeignKey.TableName, Is.EqualTo("other_table"));
            CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] {"id", "name"});
            CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] {"otherId", "birthday"});

            // create table: other_table
            createTableResult = (TableInfo) sqlResults[1];
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
            var createIndexResult = (IndexInfo) sqlResults[2];
            Assert.That(createIndexResult.Name, Is.EqualTo("UX_name"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.True);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

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
            createIndexResult = (IndexInfo) sqlResults[3];
            Assert.That(createIndexResult.Name, Is.EqualTo("IX_id"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.False);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

            indexColumns = createIndexResult.Columns;
            Assert.That(indexColumns, Has.Count.EqualTo(1));

            indexColumnInfo = indexColumns[0];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("id"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

            // create index: UX_name
            createIndexResult = (IndexInfo) sqlResults[4];
            Assert.That(createIndexResult.Name, Is.EqualTo("IX_Salary"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.False);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

            indexColumns = createIndexResult.Columns;
            Assert.That(indexColumns, Has.Count.EqualTo(1));

            indexColumnInfo = indexColumns[0];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("salary"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));
        }
    }
}