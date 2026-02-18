using System;
using System.Collections.Generic;

namespace CheckFlow.Reports.Domain.Models;

/// <summary>
///     Represents a checklist exported from the CheckFlow application.
/// </summary>
public class Checklist
{
	public int ChecklistId { get; set; }
	public string Title { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public List<Item> Items { get; set; } = [];
}
