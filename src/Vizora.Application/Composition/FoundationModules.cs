using Vizora.Application;
using Vizora.Domain;

namespace Vizora.Application.Composition;

public static class FoundationModules
{
    public static ModuleRegistry CreateDefaultRegistry()
    {
        var registry = new ModuleRegistry();
        registry.Register(Module("Library", "LIB-", CapabilityState.Implemented));
        registry.Register(Module("Selection", "SEL-", CapabilityState.ContractOnly));
        registry.Register(Module("Representation", "REP-", CapabilityState.ContractOnly));
        registry.Register(Module("Settings", "SET-", CapabilityState.Implemented));
        registry.Register(Module("Diagnostics", "DIA-", CapabilityState.Implemented));
        registry.Register(Module("TestFeedback", "TST-", CapabilityState.ContractOnly));
        return registry;
    }

    private static FeatureModule Module(string id, string prefix, CapabilityState state) =>
        new(new ModuleRegistration(id, Array.Empty<string>(), new[] { new FeatureState(id, state, prefix) }));
}
