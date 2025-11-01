using Microsoft.AspNetCore.Http;

namespace InterportCargo.Web.Helpers
{
    /// <summary>
    /// Provides helper methods and session key constants for managing
    /// authentication and authorization in the Interport Cargo web application.
    /// </summary>
    public static class Auth
    {
        /// <summary>
        /// Session key used to store the logged-in customer's ID.
        /// </summary>
        public const string SessionCustomerId = "CustomerId";

        /// <summary>
        /// Session key used to store the logged-in customer's display name.
        /// </summary>
        public const string SessionCustomerName = "CustomerName";

        /// <summary>
        /// Session key used to store the user's role in the system,
        /// for example "Customer" or "Employee".
        /// </summary>
        public const string SessionUserRole = "UserRole";

        /// <summary>
        /// Retrieves the current logged-in customer's ID from the session.
        /// </summary>
        /// <param name="ctx">The current HTTP context.</param>
        /// <returns>
        /// The customer ID if present in session; otherwise, <c>null</c>.
        /// </returns>
        public static int? GetCustomerId(HttpContext ctx)
            => ctx.Session.GetInt32(SessionCustomerId);

        /// <summary>
        /// Retrieves the current logged-in customer's name from the session.
        /// </summary>
        /// <param name="ctx">The current HTTP context.</param>
        /// <returns>
        /// The stored customer name, or <c>null</c> if it does not exist.
        /// </returns>
        public static string? GetCustomerName(HttpContext ctx)
            => ctx.Session.GetString(SessionCustomerName);

        /// <summary>
        /// Retrieves the role of the currently logged-in user from session data.
        /// </summary>
        /// <param name="ctx">The current HTTP context.</param>
        /// <returns>
        /// The user's role as a string, for example "Customer" or "Employee",
        /// or <c>null</c> if no role is found.
        /// </returns>
        public static string? GetUserRole(HttpContext ctx)
            => ctx.Session.GetString(SessionUserRole);

        /// <summary>
        /// Determines whether the current session belongs to a logged-in customer.
        /// </summary>
        /// <param name="ctx">The current HTTP context.</param>
        /// <returns>
        /// <c>true</c> if a CustomerId exists in session and the user role is "Customer";
        /// otherwise <c>false</c>.
        /// </returns>
        public static bool IsCustomer(HttpContext ctx)
            => GetCustomerId(ctx).HasValue && GetUserRole(ctx) == "Customer";
    }
}
