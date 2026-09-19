using System.Collections.ObjectModel;

namespace Vizora.Domain;

public enum RepresentationMode { Plan2D, Basic3D, Full3D }
public enum AssetSource { Core, Custom, Shared }
public enum CapabilityState { Planned, ContractOnly, Implemented, Verified, Blocked }

public sealed record DimensionDefinition(string Key, double Minimum, double Maximum, double Default, string Unit);
public sealed record MaterialSlotDefinition(string Key, string DisplayName, bool Required);
public sealed record RepresentationVariant(RepresentationMode Mode, string PhysicalContentId, bool Available);

public sealed record AssetManifest(
    string SchemaVersion,
    string AssetVersion,
    string AssetId,
    string DisplayName,
    string CategoryId,
    IReadOnlyList<string> Styles,
    IReadOnlyList<string> Tags,
    string RevitCategory,
    string ThumbnailPath,
    IReadOnlyList<RepresentationVariant> Representations,
    IReadOnlyList<DimensionDefinition> Dimensions,
    IReadOnlyList<MaterialSlotDefinition> MaterialSlots,
    AssetSource Source,
    string? ForwardAxis = null,
    IReadOnlyList<string>? RelationshipAnchors = null);

public sealed record CategoryDefinition(
    string Id,
    string DisplayName,
    string? ParentId,
    string IconKey,
    int SortOrder,
    string RevitCategory,
    IReadOnlyList<string> DimensionKeys,
    IReadOnlyList<string> MaterialSlots,
    IReadOnlyList<RepresentationMode> SupportedRepresentations,
    IReadOnlyList<string> Capabilities);

public sealed record FeatureState(string FeatureId, CapabilityState State, string ErrorPrefix, string? Reason = null);

public sealed class ValidationResult
{
    private readonly List<string> _errors = new();
    public IReadOnlyList<string> Errors => new ReadOnlyCollection<string>(_errors);
    public bool IsValid => _errors.Count == 0;
    public void Add(string error) => _errors.Add(error);
    public static ValidationResult Valid() => new();
}

public static class AssetManifestValidator
{
    public static ValidationResult Validate(AssetManifest manifest)
    {
        var result = ValidationResult.Valid();
        if (string.IsNullOrWhiteSpace(manifest.SchemaVersion)) result.Add("LIB-MANIFEST-SCHEMA: SchemaVersion is required.");
        if (string.IsNullOrWhiteSpace(manifest.AssetVersion)) result.Add("LIB-MANIFEST-VERSION: AssetVersion is required.");
        if (string.IsNullOrWhiteSpace(manifest.AssetId) || !manifest.AssetId.StartsWith("VZ_", StringComparison.Ordinal)) result.Add("LIB-ASSET-ID: AssetId must start with VZ_.");
        if (string.IsNullOrWhiteSpace(manifest.DisplayName)) result.Add("LIB-DISPLAY-NAME: DisplayName is required.");
        if (string.IsNullOrWhiteSpace(manifest.CategoryId)) result.Add("LIB-CATEGORY: CategoryId is required.");
        if (string.IsNullOrWhiteSpace(manifest.ThumbnailPath)) result.Add("LIB-THUMBNAIL: ThumbnailPath is required.");
        if (manifest.Representations.Count == 0) result.Add("LIB-REPRESENTATIONS: At least one representation is required.");
        if (!manifest.Representations.Any(x => x.Available && x.Mode == RepresentationMode.Plan2D)) result.Add("LIB-PLAN: An available 2D Plan representation is required.");
        if (!manifest.Representations.Any(x => x.Available && x.Mode == RepresentationMode.Basic3D)) result.Add("LIB-BASIC: An available Basic 3D representation is required.");
        var duplicateDimensions = manifest.Dimensions.GroupBy(x => x.Key).Where(x => x.Count() > 1).Select(x => x.Key);
        foreach (var key in duplicateDimensions) result.Add($"LIB-DIMENSION-DUPLICATE: Dimension '{key}' is duplicated.");
        foreach (var dimension in manifest.Dimensions.Where(x => x.Minimum > x.Maximum || x.Default < x.Minimum || x.Default > x.Maximum)) result.Add($"LIB-DIMENSION-RANGE: Dimension '{dimension.Key}' has an invalid range.");
        var duplicateSlots = manifest.MaterialSlots.GroupBy(x => x.Key).Where(x => x.Count() > 1).Select(x => x.Key);
        foreach (var key in duplicateSlots) result.Add($"LIB-MATERIAL-DUPLICATE: Material slot '{key}' is duplicated.");
        return result;
    }
}
