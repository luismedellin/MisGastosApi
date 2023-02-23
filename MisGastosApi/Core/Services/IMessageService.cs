using MisGastosApi.Data.Models;

namespace MisGastosApi.Core.Services;

public interface IMessageService
{
    Message GetPublicMessage();
    Message GetProtectedMessage();
    Message GetAdminMessage();
}
