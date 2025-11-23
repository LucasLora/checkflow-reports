using System;
using System.Collections.Generic;

namespace CheckFlow.Reports.Domain.Models;

public class Checklist
{
    public int ChecklistId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<Item> Items { get; set; } = [];
}