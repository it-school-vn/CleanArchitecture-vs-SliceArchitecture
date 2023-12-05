using System.Text;

namespace CleanArchitecture.Application.Core.Helper;
public static class Utilities
{
    public static string GenerateUniqueID()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int length = 11;
        StringBuilder uniqueID = new(length);
        Random random = new();

        for (int i = 0; i < length; i++)
        {
            uniqueID.Append(chars[random.Next(chars.Length)]);
        }

        return uniqueID.ToString();
    }
}