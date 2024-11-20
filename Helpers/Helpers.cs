
using System.Text;

public static class Helpers
{

    public static string GenerateRandomCode(int codeLength)
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder randomStrBuilder = new StringBuilder(codeLength);
        Random random = new Random();
        for (int i = 0; i < codeLength; i++)
        {
            int randomIndex = random.Next(alphabet.Length);
            char randomChar = alphabet[randomIndex];

            randomStrBuilder.Append(randomChar);
        }
        return randomStrBuilder.ToString();
    }


}

