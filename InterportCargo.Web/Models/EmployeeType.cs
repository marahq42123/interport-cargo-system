using System;

namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Defines the different types of employees in the Interport Cargo system.
    /// </summary>
    /// <remarks>
    /// These roles determine the level of access and system functions available
    /// to each employee. Used when registering a new employee (User Story T3).
    /// </remarks>
    public enum EmployeeType
    {
        /// <summary>
        /// System administrator — has the highest-level access and manages users, system settings, etc.
        /// </summary>
        Admin,

        /// <summary>
        /// Employee who manages quotation requests and prepares quotations for customers.
        /// </summary>
        QuotationOfficer,

        /// <summary>
        /// Handles booking of shipments, containers, and transport scheduling.
        /// </summary>
        BookingOfficer,

        /// <summary>
        /// Manages warehouse operations, cargo handling and storage activities.
        /// </summary>
        WarehouseOfficer,

        /// <summary>
        /// Oversees team operations, workflow approvals and staff coordination.
        /// </summary>
        Manager,

        /// <summary>
        /// Chief Information Officer — oversees the IT infrastructure and enterprise system strategy.
        /// </summary>
        CIO
    }
}
