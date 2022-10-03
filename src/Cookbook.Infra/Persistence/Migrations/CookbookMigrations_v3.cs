using FluentMigrator;

namespace Cookbook.Infra.Persistence.Migrations;

[Migration((long)MigrationVersionEnum.AlterRecipeTable, "Include PreparationTime column in Recipe Table")]
public class CookbookMigrations_v3 : Migration
{
    public override void Up()
    {
        Alter.Table("recipes")
            .AddColumn("PreparationTime")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0); // Para os registro que já existem, coloca "0"
    }
    public override void Down()
    {
        throw new NotImplementedException();
    }
}
