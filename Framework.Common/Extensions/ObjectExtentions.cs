using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.Common.Extensions
{
  public static class ObjectExtentions
  {
    public static string GetObjectDataForLog(object target)
    {
      try
      {
        var properties =
                        from property in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        select new
                        {
                          Name = property.Name,
                          Value = property.GetValue(target, null)
                        };

        var builder = new StringBuilder();

        foreach (var property in properties)
        {
          builder
              .Append(property.Name)
              .Append(" = ")
              .Append(property.Value)
              .AppendLine();
        }

        return builder.ToString();
      }
      catch (Exception)
      {
        return string.Empty;
      }

    }

  }
}
