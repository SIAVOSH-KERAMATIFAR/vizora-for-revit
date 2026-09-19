using Vizora.Domain;

namespace Vizora.Application;

public sealed record DiagnosticContext(
    string OperationId,
    string FeatureId,
    string ErrorCode,
    string CorrelationId,
    string DocumentVersion,
    string RevitVersion,
    IReadOnlyList<string> AssetIds,
    IReadOnlyList<long> ElementIds);

public sealed record DiagnosticError(DiagnosticContext Context, string UserMessage, string TechnicalMessage);
public sealed record OperationResult<T>(T? Value, IReadOnlyList<DiagnosticError> Errors)
{
    public bool Succeeded => Errors.Count == 0;
    public static OperationResult<T> Success(T value) => new(value, Array.Empty<DiagnosticError>());
    public static OperationResult<T> Failure(DiagnosticError error) => new(default, new[] { error });
}

public interface IDiagnosticRouter
{
    void Report(DiagnosticError error);
    IReadOnlyList<DiagnosticError> ForCorrelation(string correlationId);
}

public sealed class DiagnosticRouter : IDiagnosticRouter
{
    private readonly List<DiagnosticError> _errors = new();
    public void Report(DiagnosticError error) => _errors.Add(error);
    public IReadOnlyList<DiagnosticError> ForCorrelation(string correlationId) => _errors.Where(x => x.Context.CorrelationId == correlationId).ToArray();
}

public interface IFeatureModule
{
    string Id { get; }
    IReadOnlyCollection<string> Dependencies { get; }
    IReadOnlyCollection<FeatureState> States { get; }
}

public sealed record ModuleRegistration(string Id, IReadOnlyCollection<string> Dependencies, IReadOnlyCollection<FeatureState> States);

public interface IModuleRegistry
{
    IReadOnlyCollection<IFeatureModule> Modules { get; }
    ValidationResult Validate();
}

public sealed class FeatureModule : IFeatureModule
{
    public FeatureModule(ModuleRegistration registration)
    {
        Id = registration.Id;
        Dependencies = registration.Dependencies;
        States = registration.States;
    }
    public string Id { get; }
    public IReadOnlyCollection<string> Dependencies { get; }
    public IReadOnlyCollection<FeatureState> States { get; }
}

public sealed class ModuleRegistry : IModuleRegistry
{
    private readonly Dictionary<string, IFeatureModule> _modules = new(StringComparer.Ordinal);
    public IReadOnlyCollection<IFeatureModule> Modules => _modules.Values;
    public void Register(IFeatureModule module) => _modules.Add(module.Id, module);

    public ValidationResult Validate()
    {
        var result = ValidationResult.Valid();
        foreach (var module in _modules.Values)
            foreach (var dependency in module.Dependencies)
                if (!_modules.ContainsKey(dependency)) result.Add($"DIA-MODULE-MISSING: '{module.Id}' depends on unregistered module '{dependency}'.");
        foreach (var module in _modules.Values)
            if (DependsOnItself(module.Id, module.Id, new HashSet<string>(StringComparer.Ordinal))) result.Add($"DIA-MODULE-CYCLE: Module '{module.Id}' participates in a dependency cycle.");
        return result;
    }

    private bool DependsOnItself(string start, string current, HashSet<string> visited)
    {
        if (!visited.Add(current)) return current == start;
        return _modules.TryGetValue(current, out var module) && module.Dependencies.Any(x => x == start || DependsOnItself(start, x, visited));
    }
}

public interface IManifestCatalog
{
    IReadOnlyList<AssetManifest> ActiveAssets { get; }
    ValidationResult Add(AssetManifest manifest);
}

public sealed class ManifestCatalog : IManifestCatalog
{
    private readonly List<AssetManifest> _assets = new();
    public IReadOnlyList<AssetManifest> ActiveAssets => _assets;
    public ValidationResult Add(AssetManifest manifest)
    {
        var result = AssetManifestValidator.Validate(manifest);
        if (_assets.Any(x => x.AssetId == manifest.AssetId)) result.Add($"LIB-ASSET-DUPLICATE: AssetId '{manifest.AssetId}' already exists.");
        if (result.IsValid) _assets.Add(manifest);
        return result;
    }
}
