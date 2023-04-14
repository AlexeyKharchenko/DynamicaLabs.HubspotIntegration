using AutoMapper;
using DynamicaLabs.HubspotIntegration.Interfaces;
using DynamicaLabs.HubspotIntegration.Models;
using Microsoft.Extensions.Logging;

namespace DynamicaLabs.HubspotIntegration
{
    /// <summary>
    /// 3. Develop a module to integrate Hubspot contacts into the system's contacts, merges it with an existing one (if any), and stores it in the database. (4h)
    /// </summary>
    internal class ContactIntegrationService : IContactIntegrationService
    {
        private readonly ILogger<ContactIntegrationService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<ContactEntity> _repository;

        public ContactIntegrationService(ILogger<ContactIntegrationService> logger,
                                         IMapper mapper,
                                         IRepository<ContactEntity> repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<OperationResult> CreateOrUpdateAsync(IContact contact)
        {
            try
            {
                var contactEntity = _mapper.Map<ContactEntity>(contact);

                var existingContact = (await _repository.FindAsync(x => x.Email == contact.Email)).FirstOrDefault();
                if (existingContact == null)
                {
                    _repository.Add(contactEntity);
                    return OperationResult.Success();
                }
                else
                {
                    _mapper.Map(contactEntity, existingContact);
                    existingContact.IsDeleted = false;
                    _repository.Update(existingContact);
                    return OperationResult.Success();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create or update contact.");
                return OperationResult.Failure("Failed to create or update contact.");
            }
        }

        public async Task<OperationResult> DeleteByHubspotIdAsync(string hubspotId)
        {
            try
            {
                var existingContact = (await _repository.FindAsync(x => x.HubspotId == hubspotId)).FirstOrDefault();
                if (existingContact == null)
                {
                    _logger.LogError("Contact does not exist in the repository.");
                    return OperationResult.Failure("Contact does not exist in the repository.");
                }
                else
                {
                    //soft delete because the Hubspot support the restoring contacts
                    //and I not sure about relations contacts with other entities
                    existingContact.IsDeleted = true;
                    _repository.Update(existingContact);
                    await _repository.SaveAsync();
                    return OperationResult.Success();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete contact.");
                return OperationResult.Failure("Failed to delete contact.");
            }
        }

        public async Task<OperationResult> MergeContactsAsync(IEnumerable<IContact> contacts)
        {
            try
            {
                var existingEntities = (await _repository.GetAllAsync()).ToList();

                //Add or update existing contacts
                foreach (var contact in contacts)
                {
                    var existingEntity = existingEntities.FirstOrDefault(e => e.Email == contact.Email);

                    if (existingEntity != null)
                    {
                        _mapper.Map(contact, existingEntity);
                        existingEntity.IsDeleted = false;
                        _repository.Update(existingEntity);
                    }
                    else
                    {
                        var newEntity = _mapper.Map<ContactEntity>(contact);
                        _repository.Add(newEntity);
                    }
                }

                //deleting contacts that were in the hubspot but were deleted
                foreach (var entity in existingEntities)
                {
                    //not delete contacts that were not synchronized
                    if (!string.IsNullOrEmpty(entity.HubspotId) && contacts.All(x => x.HubspotId != entity.HubspotId))
                    {
                        entity.IsDeleted = true;
                        _repository.Update(entity);
                    }
                }

                await _repository.SaveAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to merge contacts.");
                return OperationResult.Failure("Failed to merge contacts.");
            }
        }
    }
}
