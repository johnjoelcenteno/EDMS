using DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteAllFeatures;
using DPWH.EDMS.Application.Features.ArcGis.Queries.FeatureServiceLayer;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using static DPWH.EDMS.Application.Features.ArcGis.Commands.UpdateFeatures.UpdateFeaturesCommand;
using static DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures.UploadFeaturesCommand;
using DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.ArcGis.Commands.UpdateFeatures;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.BatchCreateArcgis;

public record BatchCreateArcgisCommand(bool EnableCleanUp, string ServiceName, int LayerId, string regionId) : IRequest;



internal sealed class BatchCreateArgisHandler : IRequestHandler<BatchCreateArcgisCommand>
{
    private readonly IReadRepository _readRepository;
    private readonly IWriteRepository _writeRepository;
    private readonly ClaimsPrincipal _principal;
    private readonly IMediator _mediator;
    private readonly string Arcgis = "ArcGis";
    public BatchCreateArgisHandler(IReadRepository readRepository, IMediator mediator, IWriteRepository writeRepository, ClaimsPrincipal claimsPrincipal)
    {
        _readRepository = readRepository;
        _mediator = mediator;
        _writeRepository = writeRepository;
        _principal = claimsPrincipal;
    }
    public async Task Handle(BatchCreateArcgisCommand request, CancellationToken cancellationToken)
    {
        var syncResults = "Success";
        var syncDescription = "";
        var syncedDataCount = 0;

        try
        {
            var assetsDb = await UploadUpdateCoordinateSync(_readRepository.AssetsView, request.regionId);
            // true then delete
            if (request.EnableCleanUp)
            {
                // delete all 
                await _mediator.Send(new DeleteAllFeaturesCommand(request.ServiceName, request.LayerId, await CleanData(request.regionId)));
                var result = new UploadFeaturesCommand { ServiceName = request.ServiceName, LayerId = request.LayerId, Features = null };

                var resFeatures = assetsDb.Select(asset => new ArcGisUploadFeatureServiceLayer
                {
                    Attributes = new Dictionary<string, object>
                    {
                        { "OBJECTID", asset.BuildingId != null ? asset.BuildingId : "" },
                        { "BuildingId", asset.BuildingId != null ? asset.BuildingId : "" },
                        { "BuildingName", asset.Name != null ? asset.Name : "" },
                        { "Province", asset.Province != null ? asset.Province : "" },
                        { "Region", asset.Region != null ? asset.Region : "" },
                        { "RegionId", asset.RegionId != null ? asset.RegionId : "" },
                        { "ImplementingOffice", asset.ImplementingOffice != null ? asset.ImplementingOffice : "" },
                        { "Municipality", asset.CityOrMunicipality != null ? asset.CityOrMunicipality : "" },
                        { "PropertyCondition", asset.PropertyStatus != null ? asset.PropertyStatus : "" },
                        { "Group_", asset.Group != null ? asset.Group : "" },
                    },
                    Geometry = new ArcGisUploadGeometry
                    {
                        X = DmsToLongLat.Convert(new DmsPoint
                        {
                            Degrees = asset.LongDegrees,
                            Minutes = asset.LongMinutes,
                            Seconds = asset.LongSeconds,
                            Type = PointType.Lon,
                            Direction = asset.LongDirection
                        }),
                        Y = DmsToLongLat.Convert(new DmsPoint
                        {
                            Degrees = asset.LatDegrees,
                            Minutes = asset.LatMinutes,
                            Seconds = asset.LatSeconds,
                            Type = PointType.Lat,
                            Direction = asset.LatDirection
                        }),

                        SpatialReference = new ArcGisUploadSpatialReference
                        {
                            Wkid = 4326
                        }
                    }
                }).ToArray();
                result.Features = resFeatures;

                foreach (var feature in result.Features)
                {
                    syncedDataCount++;
                }
                syncDescription = syncedDataCount == 0
                    ? "No data updated. Already up to date."
                    : $"Successfully synced {syncedDataCount} row{(syncedDataCount > 1 ? "s" : "")} of data.";

                //upload
                await _mediator.Send(result, cancellationToken);
            }

            // if you want to update only 
            if (!request.EnableCleanUp)
            {
                var result = new UpdateFeaturesCommand { ServiceName = request.ServiceName, LayerId = request.LayerId, Features = null };
                var resFeatures = assetsDb.Select(asset => new ArcGisUpdateFeatureServiceLayer
                {
                    Attributes = new Dictionary<string, object>
                    {
                        { "BuildingId", asset.BuildingId != null ? asset.BuildingId : "" },
                        { "BuildingName", asset.Name != null ? asset.Name : "" },
                        { "Province", asset.Province != null ? asset.Province : "" },
                        { "Region", asset.Region != null ? asset.Region : "" },
                        { "RegionId", asset.RegionId != null ? asset.RegionId : "" },
                        { "ImplementingOffice", asset.ImplementingOffice != null ? asset.ImplementingOffice : "" },
                        { "Municipality", asset.CityOrMunicipality != null ? asset.CityOrMunicipality : "" },
                        { "PropertyCondition", asset.PropertyStatus != null ? asset.PropertyStatus : "" },
                        { "Group_", asset.Group != null ? asset.Group : "" }
                    },
                    ArcGisGeometry = new ArcGisUpdateGeometry
                    {
                        X = DmsToLongLat.Convert(new DmsPoint
                        {
                            Degrees = asset.LongDegrees,
                            Minutes = asset.LongMinutes,
                            Seconds = asset.LongSeconds,
                            Type = PointType.Lon,
                            Direction = asset.LongDirection
                        }),
                        Y = DmsToLongLat.Convert(new DmsPoint
                        {
                            Degrees = asset.LatDegrees,
                            Minutes = asset.LatMinutes,
                            Seconds = asset.LatSeconds,
                            Type = PointType.Lat,
                            Direction = asset.LatDirection
                        }),

                        ArcGisSpatialReference = new ArcGisUpdateSpatialReference
                        {
                            Wkid = 4326
                        }
                    }
                }).ToArray();
                result.Features = resFeatures;

                var entitiesToUpdate = await UpdateIfExist(result, request, cancellationToken);

                foreach (var arcgis in entitiesToUpdate)
                {
                    syncedDataCount++;
                }

                syncDescription = syncedDataCount == 0
                    ? "No data updated. Already up to date."
                    : $"Successfully synced {syncedDataCount} row{(syncedDataCount > 1 ? "s" : "")} of data.";

                if (entitiesToUpdate != null && entitiesToUpdate.Count > 0)
                {
                    UpdateFeaturesResult? resultUpdate = await _mediator.Send(new UpdateFeaturesCommand
                    {
                        LayerId = request.LayerId,
                        ServiceName = request.ServiceName,
                        Features = entitiesToUpdate.ToArray()
                    }, cancellationToken);
                }

            }

        }
        catch (AppException e)
        {
            syncResults = "Error";
            syncDescription = e.Message;
        }
        finally
        {
            var dataSync = DataSyncLog.Create(Arcgis, syncResults, syncDescription, _principal.GetUserName());
            await _writeRepository.DataSyncLogs.AddAsync(dataSync, cancellationToken);
        }
        await _writeRepository.SaveChangesAsync(cancellationToken);


    }

    public async Task<List<ArcGisUpdateFeatureServiceLayer>> UpdateIfExist(UpdateFeaturesCommand result, BatchCreateArcgisCommand request, CancellationToken cancellationToken)
    {
        var toUpdate = new List<ArcGisUpdateFeatureServiceLayer>();
        var ekte = new List<List<string>>();
        var ressss = new UpdateFeaturesCommand { ServiceName = result.ServiceName, LayerId = result.LayerId, Features = null };
        // get all the 
        var resFeatures = await _mediator.Send(new FeatureServiceLayerQuery(result.ServiceName, result.LayerId, await CleanData(request.regionId)), cancellationToken);

        if (resFeatures != null && resFeatures.Features != null)
        {
            foreach (var feature in resFeatures.Features)
            {
                var entity = result.Features!.FirstOrDefault(f => string.Equals(feature.Attributes!["BuildingId"].ToString(), f.Attributes!["BuildingId"].ToString(), StringComparison.OrdinalIgnoreCase));
                if (entity is null)
                {
                    continue;
                }
                entity.Attributes!["OBJECTID"] = feature.Attributes!["OBJECTID"];
                //var keysWithEqualValues = entity.Attributes.Keys
                //.Where(key =>
                //    feature.Attributes.ContainsKey(key) && entity != null && entity.Attributes != null && entity.Attributes[key].ToString().Equals(feature.Attributes[key].ToString())).ToList();

                var keysWithEqualValues = entity?.Attributes?.Keys
                .Where(key =>
                    feature.Attributes?.ContainsKey(key) == true &&
                    entity.Attributes != null && entity.Attributes[key].ToString().Equals(feature.Attributes[key].ToString()))
                .ToList();

                if (keysWithEqualValues != null && keysWithEqualValues!.Count != entity!.Attributes.Count)
                {
                    toUpdate.Add(entity);
                }

            }
        }
        return toUpdate;
    }


    public async Task<IQueryable<Asset>> UploadUpdateCoordinateSync(IQueryable<Asset> assets, string regionId)
    {

        if (regionId != null)
        {
            assets = assets.Where(asset => asset.RegionId == regionId);
        }


        return assets;
    }

    public async Task<string> CleanData(string regionId)
    {
        string dataClean = "";


        if (regionId != null)
        {
            dataClean = $@"RegionId = '{regionId}'";
        }
        return dataClean;
    }
}


