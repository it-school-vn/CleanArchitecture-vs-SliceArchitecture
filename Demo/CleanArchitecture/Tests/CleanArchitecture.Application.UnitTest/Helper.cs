namespace CleanArchitecture.Application.UnitTest
{
    public static class Helper
    {
        public static void SetPropertyValue(string propertyName, object instance, object? propertyvalue)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);

            propertyInfo?.SetValue(instance, propertyvalue, null);
        }

    }
}