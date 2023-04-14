namespace DynamicaLabs.HubspotIntegration.Models
{
    internal class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult Success()
        {
            return new OperationResult { IsSuccess = true };
        }

        public static OperationResult Failure(string error)
        {
            return new OperationResult { IsSuccess = false, ErrorMessage = error };
        }
    }
}
