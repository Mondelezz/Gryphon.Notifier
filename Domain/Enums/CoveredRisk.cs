namespace Domain.Enums;

public enum CoveredRisk
{
    /// <summary>
    /// Угон автомобиля
    /// </summary>
    Theft,

    /// <summary>
    /// Ущерб в результате ДТП
    /// </summary>
    CarAccident,

    /// <summary>
    /// Повреждение от стихийных бедствий (например, наводнение, ураган)
    /// </summary>
    NaturalDisasters,

    /// <summary>
    /// Пожар
    /// </summary>
    Fire,

    /// <summary>
    /// Вандализм
    /// </summary>
    Vandalism,

    /// <summary>
    /// ДТП с участием других лиц
    /// </summary>
    ThirdPartyLiability,

    /// <summary>
    /// Ущерб от животного
    /// </summary>
    AnimalDamage,

    /// <summary>
    /// Ущерб при столкновении с объектом
    /// </summary>
    CollisionWithObject,

    /// <summary>
    /// Иные риски (например, разбитое стекло)
    /// </summary>
    Other
}
