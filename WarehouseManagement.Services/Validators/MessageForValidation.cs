namespace WarehouseManagement.Services.Validators
{
    /// <summary>
    /// Статический класс для работы с сообщениями в валидаторе
    /// </summary>
    internal static class MessageForValidation
    {
        /// <summary>
        /// Сообщение для ошибок NotNull, NotEmpty
        /// </summary>
        public static string DefaultMessage = "Ошибка в поле {PropertyName}: значение {PropertyValue}";

        /// <summary>
        /// Сообщение для ошибок длинны строк
        /// </summary>
        public static string LengthMessage = "Длина поля {PropertyName} должна быть от {MinLength} до {MaxLength}. Текущая длина: {TotalLength}";

        /// <summary>
        /// Сообщение для ошибок диапазона
        /// </summary>
        public static string InclusiveBetweenMessage = "Значение поля {PropertyName} должно находится в " +
            "диапозоне от {From} до {To} включительно. Тукущее значение равно {PropertyValue}";

        public static string NotFoundGuidMessage = "Вы ввели несуществующий Guid в поле {PropertyName}";
    }
}
