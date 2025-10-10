namespace InterportCargo.Web.Models
{
    public class RateSchedule
    {
        public int Id { get; set; }
        public string ContainerType { get; set; } = "20GP";
        public decimal BaseCharge { get; set; }
        public decimal DepotCharge { get; set; }
        public decimal LclDeliveryCharge { get; set; }
    }
}
