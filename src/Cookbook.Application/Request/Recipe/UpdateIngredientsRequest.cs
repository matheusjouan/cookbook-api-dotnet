namespace Cookbook.Application.Request.Recipe;
public class UpdateIngredientsRequest
{
    public long Id { get; set; }
    public string Product { get; set; }
    public string Amount { get; set; }
}
