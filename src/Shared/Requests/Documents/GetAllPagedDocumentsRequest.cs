namespace CleanArchTemplate.Shared.Requests.Documents
{
    public class GetAllPagedDocumentsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}