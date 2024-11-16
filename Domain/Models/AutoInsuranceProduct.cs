using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class AutoInsuranceProduct : EntityBase, IEntityState
{
    /// <summary>
    /// Состояние страхового продукта
    /// </summary>
    public State State { get; set; }

    /// <summary>
    /// Возраст автомобиля
    /// </summary>
    public required int AgeAuto { get; set; }

    /// <summary>
    /// Марка автомобиля
    /// </summary>
    public required BrandAuto BrandAuto { get; set; }

    /// <summary>
    /// Стаж водительского опыта
    /// </summary>
    public required int AgeDrivingExpirience { get; set; }

    /// <summary>
    /// Возраст водителя
    /// </summary>
    public required int AgeDriver { get; set; }

    /// <summary>
    /// Доступные бонусы и скидки
    /// </summary>
    public required IList<AvailableBonusesAndDiscounts> BonusesAndDiscounts { get; set; } = [];

    /// <summary>
    /// Правила эксплуатации
    /// </summary>
    public required string UsageRules { get; set; }

    /// <summary>
    /// Географические ограничения использования
    /// </summary>
    public required string GeographicalLimitations { get; set; }

    /// <summary>
    /// Тип использования автомобиля
    /// </summary>
    public required VehicleUsageType VehicleUsageType { get; set; }

    /// <summary>
    /// Список покрываемых рисков
    /// </summary>
    public required IList<CoveredRisk> CoveredRisk { get; set; } = [];

    /// <summary>
    /// Тип страховки
    /// </summary>
    public required InsuranceType InsuranceType { get; set; }

    /// <summary>
    /// Класс автомобиля
    /// </summary>
    public required VehicleInsuranceClass VehicleInsuranceClass { get; set; }
}
