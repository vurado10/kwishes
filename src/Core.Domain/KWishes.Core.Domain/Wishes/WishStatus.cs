namespace KWishes.Core.Domain.Wishes;

public enum WishStatus
{
    /// <summary>
    /// Пожелание на рассмотрении
    /// </summary>
    Moderating,
    
    /// <summary>
    /// Пожелание в работе
    /// </summary>
    Processing,

    /// <summary>
    /// Пожелание отклонено
    /// </summary>
    Rejected,
    
    /// <summary>
    /// Пожелание завершено
    /// </summary>
    Completed
}