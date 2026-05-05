namespace Zephyrus.SharedKernel.Common.Helpers;

public static class CodeGenerator
{
    public static string GenerateCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return string.Concat(Enumerable.Range(0, length)
            .Select(_ => chars[Random.Shared.Next(chars.Length)]));
    }
}