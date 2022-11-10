namespace OrderingService.AppCore.Helpers
{
    public static class EmailHelper
    {
        public static string EncryptEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length < 7)
            {
                return email;
            }
            return $"{email[..2]}***{email[^2..]}";
        }
    }
}
