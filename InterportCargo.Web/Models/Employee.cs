using System.ComponentModel.DataAnnotations;

namespace InterportCargo.Web.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required] public string FirstName { get; set; } = "";
        [Required] public string FamilyName { get; set; } = "";
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required] public string Phone { get; set; } = "";
        [Required] public EmployeeType EmployeeType { get; set; }
        [Required] public string Address { get; set; } = "";
        [Required] public string PasswordHash { get; set; } = "";
    }
}
