using System;

namespace CheckFlow.Reports.Domain.Models;

public class Photo
{
    public int PhotoId { get; set; }
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    ///     Caminho relativo dentro do ZIP (ex.: "photos/1_photo.jpg").
    /// </summary>
    public string Path { get; set; } = string.Empty;

    public DateTime PhotoAttachedAt { get; set; }

    /// <summary>
    ///     Indica se a foto estava faltando no momento de exportar o checklist.
    /// </summary>
    public bool Missing { get; set; }
}