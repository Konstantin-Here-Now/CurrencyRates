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

    public OneCursStruct(string Vname, string Vnom, string Vcurs, string Vcode, string VchCode, string VunitRate)
    {
        this.Vname = Vname;
        this.Vnom = Vnom;
        this.Vcurs = Vcurs;
        this.Vcode = Vcode;
        this.VchCode = VchCode;
        this.VunitRate = VunitRate;
    }

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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>Структура, содержащая информацию о курсах валют на определенную дату.</summary>
public struct CursOnDateStruct
{
    public string cursDate { get; set; }
    public List<OneCursStruct> cursData { get; set; }

    public CursOnDateStruct(string cursDate, List<OneCursStruct> cursData)
    {
        this.cursDate = cursDate;
        this.cursData = cursData;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CursOnDateStruct objectType)
        {
            return objectType.cursDate == this.cursDate && Enumerable.SequenceEqual(objectType.cursData, this.cursData);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

}