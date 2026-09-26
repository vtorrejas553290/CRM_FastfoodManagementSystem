using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IFeedbackController
{
    List<FeedbackRow> GetFeedback(FeedbackFilter filter);
}