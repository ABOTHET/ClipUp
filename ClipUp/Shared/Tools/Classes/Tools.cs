using ClipUp.Shared.Objects.DTOs;
using System.Reflection;

namespace ClipUp.Shared.Tools.Classes
{
    public static class Tools
    {
        // Поля изменяемого обьекта и входных данных должны иметь одинаковые имена
        public static void UpdateObject(object updateData, object mutableObject)
        {
            PropertyInfo[] propertyInfos = updateData.GetType().GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                object? propertyValue = property.GetValue(updateData);
                if (propertyValue == null) continue;
                PropertyInfo? propertyInfo = mutableObject!
                    .GetType()
                    .GetProperty(property.Name);
                if (propertyInfo == null) continue;
                propertyInfo.SetValue(mutableObject, propertyValue);
            }
        }
    }
}
