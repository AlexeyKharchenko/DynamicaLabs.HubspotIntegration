namespace DynamicaLabs.HubspotIntegration.Interfaces
{
    internal interface IContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string HubspotId { get; set; }
    }
}
