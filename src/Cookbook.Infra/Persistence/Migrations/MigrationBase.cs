using FluentMigrator.Builders.Create.Table;

namespace Cookbook.Infra.Persistence.Migrations;

public static class MigrationBase
{
    public static ICreateTableWithColumnOrSchemaOrDescriptionSyntax InsertStandardColumn(ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
    {
        table
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("createdAt").AsDateTime().NotNullable();

        return table;
    }
}