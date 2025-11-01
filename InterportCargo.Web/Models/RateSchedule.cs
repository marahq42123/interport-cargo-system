namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents a rate schedule record that defines the pricing for different container types.
    /// This model is used by the quotation officer to calculate quotation costs (User Story I4).
    /// </summary>
    public class RateSchedule
    {
        /// <summary>
        /// Primary key for the rate schedule record in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The container type that this rate applies to (e.g. 20GP, 40GP, LCL).
        /// </summary>
        public string ContainerType { get; set; } = "20GP";

        /// <summary>
        /// The base charge applied for this container type (includes general freight handling).
        /// </summary>
        public decimal BaseCharge { get; set; }

        /// <summary>
        /// Additional depot-related charges for this container type (loading, storage etc.).
        /// </summary>
        public decimal DepotCharge { get; set; }

        /// <summary>
        /// LCL (Less than Container Load) delivery charge.
        /// Only applies if the container type is 'LCL'.
        /// </summary>
        public decimal LclDeliveryCharge { get; set; }
    }
}
