namespace Cookbook.Core.Entities;
public class QRCode : EntityBase
{
    public string Code { get; set; }
    public long UserId { get; set; }
}
