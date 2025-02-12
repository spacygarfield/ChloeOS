using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChloeOS.DataAccess;

public class UlidToStringConverter : ValueConverter<Ulid, string> {

    private static readonly ConverterMappingHints _defaultHints = new (16);

    public UlidToStringConverter() : this(_defaultHints) { }

    public UlidToStringConverter(ConverterMappingHints? mappingHints)
        : base (
            convertToProviderExpression: u => u.ToString(),
            convertFromProviderExpression: s => Ulid.Parse(s),
            mappingHints: _defaultHints.With(mappingHints)
        ) { }

}