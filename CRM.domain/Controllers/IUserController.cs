using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IUserController
{
    List<UserRow> GetUsers(UserFilter filter);
}