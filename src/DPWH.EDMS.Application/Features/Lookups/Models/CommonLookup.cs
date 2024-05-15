using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Lookups.Models;

public record CommonLookup(IEnumerable<Lookup> Lookups, IEnumerable<AddressLookup> AddressLookups);

public record Lookup(string Key, IEnumerable<SimpleKeyValue>? Data);

public record AddressLookup(string Key, IEnumerable<SimpleKeyValueAddress> AddressData);

public record SimpleKeyValueAddress(string Id, string Name, string? UniqueId);