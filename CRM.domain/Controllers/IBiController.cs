using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IBiController
{
    BiSnapshot GetSnapshot(BiFilter filter);
}