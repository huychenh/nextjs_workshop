namespace OrderingService.AppCore.Helpers
{
    public static class EmailHelper
    {
        public static string EncryptEmail(string email)
        {
            return $"{email[..3]}***{email.Substring(email.Length - 3, 3)}";
        }
    }
}
