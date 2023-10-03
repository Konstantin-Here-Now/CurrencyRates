namespace CursStructs;

/// <summary>Структура, содержащая информацию о курсе валюты.</summary>
public struct OneCursStruct
{
    /// <summary>Название валюты.</summary>
    public string Vname { get; set; }
    /// <summary>Номинал.</summary>
    public string Vnom { get; set; }
    /// <summary>Курс.</summary>
    public string Vcurs { get; set; }
    /// <summary>ISO Цифровой код валюты.</summary>
    public string Vcode { get; set; }
    /// <summary>ISO Символьный код валюты.</summary>
    public string VchCode { get; set; }
    /// <summary>Курс за 1 единицу валюты.</summary>
    public string VunitRate { get; set; }

    /// <summary>
    /// Конструктор структуры OneCursStruct.
    /// </summary>
    /// <param name="Vname">Название валюты.</param>
    /// <param name="Vnom">Номинал.</param>
    /// <param name="Vcurs">Курс.</param>
    /// <param name="Vcode">ISO Цифровой код валюты.</param>
    /// <param name="VchCode">ISO Символьный код валюты.</param>
    /// <param name="VunitRate">Курс за 1 единицу валюты.</param>
    public OneCursStruct(string Vname, string Vnom, string Vcurs, string Vcode, string VchCode, string VunitRate)
    {
        this.Vname = Vname;
        this.Vnom = Vnom;
        this.Vcurs = Vcurs;
        this.Vcode = Vcode;
        this.VchCode = VchCode;
        this.VunitRate = VunitRate;
    }

    /// <summary>
    /// Метод, переопределяющий сравнение для структуры OneCursStruct.
    /// </summary>
    /// <param name="obj">Объект для сравнения со структурой OneCursStruct.</param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is OneCursStruct objectType)
        {
            return objectType.Vname == this.Vname
            && objectType.Vnom == this.Vnom
            && objectType.Vcurs == this.Vcurs
            && objectType.Vcode == this.Vcode
            && objectType.VchCode == this.VchCode
            && objectType.VunitRate == this.VunitRate;
        }

        return false;
    }

    /// <summary>
    /// Метод, переопределяющий получение хэш-кода для структуры OneCursStruct.
    /// </summary>
    /// <returns>Хэш-код экземпляра структуры OneCursStruct.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>Структура, содержащая информацию о курсах валют на определенную дату.</summary>
public struct CursOnDateStruct
{
    /// <summary>
    /// Дата, на которую актуальные курсы валют.
    /// </summary>
    public string cursDate { get; set; }
    /// <summary>
    /// Информация о курсах валют.
    /// </summary>
    public List<OneCursStruct> cursData { get; set; }

    /// <summary>
    /// Конструктор структуры CursOnDateStruct.
    /// </summary>
    /// <param name="cursDate">Дата, на которую актуальные курсы валют.</param>
    /// <param name="cursData">Информация о курсах валют.</param>
    public CursOnDateStruct(string cursDate, List<OneCursStruct> cursData)
    {
        this.cursDate = cursDate;
        this.cursData = cursData;
    }

    /// <summary>
    /// Метод, переопределяющий сравнение для структуры CursOnDateStruct.
    /// </summary>
    /// <param name="obj">Объект для сравнения со структурой CursOnDateStruct.</param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is CursOnDateStruct objectType)
        {
            return objectType.cursDate == this.cursDate && Enumerable.SequenceEqual(objectType.cursData, this.cursData);
        }

        return false;
    }

    /// <summary>
    /// Метод, переопределяющий получение хэш-кода для структуры CursOnDateStruct.
    /// </summary>
    /// <returns>Хэш-код экземпляра структуры CursOnDateStruct.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

}