using FluentMigrator;

namespace Cookbook.Infra.Persistence.Migrations;

[Migration((long)MigrationVersionEnum.CreateRecipe, "Create Recipe Table")]

public class CookbookMigrations_v2 : Migration
{
    public override void Up()
    {
        var recipeTable = Create.Table("Recipes");

        MigrationBase.InsertStandardColumn(recipeTable);

        recipeTable
            .WithColumn("Title").AsString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("PreparationMode").AsString(5000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
                .ForeignKey("FK_Recipes_User_Id", "Users", "Id");

        var ingredientsTable = Create.Table("Ingredients");

        MigrationBase.InsertStandardColumn(ingredientsTable);

        ingredientsTable
            .WithColumn("Product").AsString(100).NotNullable()
            .WithColumn("Amount").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable()
                .ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id");
    }
    
    public override void Down()
    {
    }


}
