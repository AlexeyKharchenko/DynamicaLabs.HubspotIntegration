namespace DynamicaLabs.HubspotIntegration.Models
{
    public class ContactEntity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string HubspotId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
