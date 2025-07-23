namespace HotelBookingSystem.Domain.Enums;

public enum BookingStatus
{
    AwaitingUserConfirmation,
    AwaitingManagerConfirmation,
    ConfirmedByManager,
    Cancelled,
    Rejected
}