using DynamicaLabs.HubspotIntegration.Models;

namespace DynamicaLabs.HubspotIntegration.Interfaces
{
    internal interface IContactIntegrationService
    {
        /// <summary>
        /// Creates or updates a single contact in the database.
        /// </summary>
        /// <param name="contact">The Hubspot contact to create or update.</param>
        /// <returns>An OperationResult indicating success or failure of the operation.</returns>
        Task<OperationResult> CreateOrUpdateAsync(IContact contact);

        /// <summary>
        /// Deletes a single contact from the database by hubspot Id.
        /// </summary>
        /// <param name="hubspotId">Hubspot Contact Id</param>
        /// <returns>An OperationResult indicating success or failure of the operation.</returns>
        Task<OperationResult> DeleteByHubspotIdAsync(string hubspotId);

        /// <summary>
        /// Merges a list of contacts with the corresponding entities in the database.
        /// </summary>
        /// <param name="contacts">The Hubspot contacts to merge.</param>
        /// <returns>An OperationResult indicating success or failure of the operation.</returns>
        Task<OperationResult> MergeContactsAsync(IEnumerable<IContact> contacts);
    }
}
