using System.Text;

public static class RandomStringGenerator
{
    public static string Generate(int length)
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string numbers = "0123456789";

        string allChars = letters + numbers;
        var random = new Random();

        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var index = random.Next(allChars.Length);
            result.Append(allChars[index]);
        }

        return result.ToString();
    }
}
