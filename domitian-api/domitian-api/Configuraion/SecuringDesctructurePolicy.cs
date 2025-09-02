using domitian.Infrastructure.Censure;
using Microsoft.AspNetCore.Identity;
using Serilog.Core;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace domitian_api.Configuraion
{
  public class CensuringDesctructurePolicy : IDestructuringPolicy
  {
    private const string Censured = "***CENSURED***";

    public bool TryDestructure(object value,
      ILogEventPropertyValueFactory propertyValueFactory,
      [NotNullWhen(true)] out LogEventPropertyValue? result)
    {
      result = null;

      var logEventList = CreateLogEventList(value, propertyValueFactory);

      if (logEventList is null)
        return false;

      result = new StructureValue(logEventList);

      return true;
    }

    private IEnumerable<LogEventProperty>? CreateLogEventList(
      object? propValue,
      ILogEventPropertyValueFactory propertyValueFactory,
      IList<LogEventProperty>? structure = null)
    {
      if (propValue == null || propValue is string || propValue.GetType().IsPrimitive)
        return null;

      var props = propValue
        .GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

      structure = structure ?? new List<LogEventProperty>();

      foreach (var property in props)
      {
        structure.Add(new LogEventProperty(
          property.Name,
          propertyValueFactory.CreatePropertyValue(
            property.HasAttribute<CensuredAttribute>() || property.HasAttribute<ProtectedPersonalDataAttribute>()
          ? Censured
          : property.GetValue(propValue), true)));
      }

      return structure;
    }
  }
}
