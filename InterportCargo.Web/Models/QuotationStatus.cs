namespace InterportCargo.Web.Models
{
    /// <summary>
    /// Represents the different stages a quotation can progress through
    /// during the quotation lifecycle between the customer and the quotation officer.
    /// </summary>
    public enum QuotationStatus
    {
        /// <summary>
        /// The quotation request has been submitted by the customer
        /// but has not yet been reviewed.
        /// </summary>
        Pending,

        /// <summary>
        /// The quotation officer is currently preparing the quotation.
        /// </summary>
        Preparing,

        /// <summary>
        /// The quotation has been prepared and sent to the customer
        /// for review and approval or rejection.
        /// </summary>
        Sent,

        /// <summary>
        /// The customer has reviewed and accepted the quotation.
        /// </summary>
        Accepted,

        /// <summary>
        /// The quotation has been rejected by either the customer or officer.
        /// </summary>
        Rejected
    }
}
