using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IComplaintController
{
    /// <summary>
    /// Returns the full filtered complaint list, ordered by SubmittedAt descending.
    /// Pagination is the form's responsibility.
    /// </summary>
    List<ComplaintRow> GetComplaints(ComplaintFilter filter);
}