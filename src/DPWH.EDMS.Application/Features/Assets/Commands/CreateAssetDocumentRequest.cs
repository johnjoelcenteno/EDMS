using DPWH.EDMS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Assets.Commands;

public class CreateAssetImageRequest
{
    [Required]
    public IFormFile File { get; set; }

    [Required]
    public Guid AssetId { get; set; }
    public string Filename { get; set; }
    public string? Description { get; set; }
    //public double? Longitude { get; set; }
    //public double? Latitude { get; set; }
    #region Longitude 
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    #endregion
    #region Latitude
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
    #endregion
    public string? Uri { get; set; }
    public AssetImageView View { get; set; }
}

public class SaveAssetImageRequest
{
    public Guid Id { get; set; }
    [Required]
    public IFormFile File { get; set; }
    [Required]
    public Guid AssetId { get; set; }
    public string Filename { get; set; }
    public string? Description { get; set; }
    //public double? Longitude { get; set; }
    //public double? Latitude { get; set; }
    #region Longitude 
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    #endregion
    #region Latitude
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
    #endregion
    public string? Uri { get; set; }
    public AssetImageView View { get; set; }
}

public class SaveAssetFileRequest
{
    public Guid Id { get; set; }
    [Required]
    public IFormFile? File { get; set; }
    [Required]
    public Guid AssetId { get; set; }
    public string Filename { get; set; }
    public string DocumentType { get; set; }
    public string? DocumentTypeOthers { get; set; }
    public string? OtherRelatedDocuments { get; set; }
    public string? Description { get; set; }
    public string? DocumentNo { get; set; }
    public string? Uri { get; set; }
}

public class SaveFinancialFileRequest
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public IFormFile? File { get; set; }
    public string? YearFunded { get; set; }
    public double? Allocation { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? Uri { get; set; }
}