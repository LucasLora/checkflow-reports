using System.Collections.Generic;

namespace CheckFlow.Reports.Domain.Models;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Photo> Photos { get; set; } = [];
}