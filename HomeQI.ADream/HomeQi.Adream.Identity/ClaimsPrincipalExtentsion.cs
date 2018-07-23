namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtentsion
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("id");
        }
    }
}
