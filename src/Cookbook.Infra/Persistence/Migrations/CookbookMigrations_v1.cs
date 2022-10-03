using FluentMigrator;

namespace Cookbook.Infra.Persistence.Migrations;

// Indica o número da versão e uma breve descrição
[Migration((long)MigrationVersionEnum.CreateUserTable, "Create User Table")]
public class CookbookMigrations_v1 : Migration
{
    public override void Up()
    {
        var userTable = Create.Table("Users");

        // Definindo as colunas padrões que terão todas as tabelas com os formatos
        MigrationBase.InsertStandardColumn(userTable);

        userTable
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(150).NotNullable().Unique()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Phone").AsString(14).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}
