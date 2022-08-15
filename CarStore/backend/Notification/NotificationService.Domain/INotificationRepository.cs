using CarStore.AppContracts.Dtos;

namespace NotificationService.AppCore
{
    public interface INotificationRepository
    {
        Task<bool> SendEmail(EmailDto emailDto);
    }
}