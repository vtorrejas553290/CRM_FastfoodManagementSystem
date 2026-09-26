using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IPromotionController
{
    List<PromotionRow> GetPromotions(PromotionFilter filter);
}