using BindingFlags = System.Reflection.BindingFlags;

namespace CleanArchTemplate.Domain.Common;

public record Currency:IComparable<Currency>
{
    public string Symbol { get; }
    internal Currency(string symbol)
    {
        Symbol = symbol;
    }
    public static implicit operator Currency(string symbol) => Create(symbol);
    public static implicit operator string(Currency currency) => currency.Symbol;
    public int CompareTo(Currency? other)
        => other is null ? 1 : string.Compare(Symbol, other.Symbol, StringComparison.Ordinal);
    public static Currency Create(string symbol) 
        => string.IsNullOrWhiteSpace(symbol)  ? Empty
        :  Currencies.FirstOrDefault(x => x.Symbol == symbol) ?? new Currency(symbol)
        ?? throw new NotSupportedException($"Currency {symbol} is not supported");
    private static  IEnumerable<Currency> Currencies 
      => typeof(Currency).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
          .Select(f => (Currency)f.GetValue(default)!)
          .Where(x=>x!=default!)
          .DistinctBy(x=>x.Symbol);

    public override string ToString() => Symbol;

    #region Static Properties
    public static readonly Currency Empty = new (string.Empty);
    public static readonly Currency AlbaniaLek=new("Lek");
    public static readonly Currency AfghanistanAfghani=new("؋");
    public static readonly Currency ArgentinaPeso=new("$");
    public static readonly Currency ArubaGuilder=new("ƒ");
    public static readonly Currency AustraliaDollar=new("$");
    public static readonly Currency AzerbaijanManat=new("₼");
    public static readonly Currency BahamasDollar=new("$");
    public static readonly Currency BarbadosDollar=new("$");
    public static readonly Currency BelarusRuble=new("Br");
    public static readonly Currency BelizeDollar=new("BZ$");
    public static readonly Currency BermudaDollar=new("$");
    public static readonly Currency BoliviaBolíviano=new("$b");
    public static readonly Currency BosniaAndHerzegovinaConvertibleMark=new("KM");
    public static readonly Currency BotswanaPula=new("P");
    public static readonly Currency BulgariaLev=new("лв");
    public static readonly Currency BrazilReal=new("R$");
    public static readonly Currency BruneiDarussalamDollar=new("$");
    public static readonly Currency CambodiaRiel=new("៛");
    public static readonly Currency CanadaDollar=new("$");
    public static readonly Currency CaymanIslandsDollar=new("$");
    public static readonly Currency ChilePeso=new("$");
    public static readonly Currency ChinaYuanRenminbi=new("¥");
    public static readonly Currency ColombiaPeso=new("$");
    public static readonly Currency CostaRicaColon=new("₡");
    public static readonly Currency CroatiaKuna=new("kn");
    public static readonly Currency CubaPeso=new("₱");
    public static readonly Currency CzechRepublicKoruna=new("Kč");
    public static readonly Currency DenmarkKrone=new("kr");
    public static readonly Currency DominicanRepublicPeso=new("RD$");
    public static readonly Currency EastCaribbeanDollar=new("$");
    public static readonly Currency EgyptPound=new("£");
    public static readonly Currency ElSalvadorColon=new("$");
    public static readonly Currency EuroMemberCountries=new("€");
    public static readonly Currency FalklandIslandsMalvinasPound=new("£");
    public static readonly Currency FijiDollar=new("$");
    public static readonly Currency GhanaCedi=new("¢");
    public static readonly Currency GibraltarPound=new("£");
    public static readonly Currency GuatemalaQuetzal=new("Q");
    public static readonly Currency GuernseyPound=new("£");
    public static readonly Currency GuyanaDollar=new("$");
    public static readonly Currency HondurasLempira=new("L");
    public static readonly Currency HongKongDollar=new("$");
    public static readonly Currency HungaryForint=new("Ft");
    public static readonly Currency IcelandKrona=new("kr");
    public static readonly Currency IndiaRupee=new("₹");
    public static readonly Currency IndonesiaRupiah=new("Rp");
    public static readonly Currency IranRial=new("﷼");
    public static readonly Currency IsleOfManPound=new("£");
    public static readonly Currency IsraelShekel=new("₪");
    public static readonly Currency JamaicaDollar=new("J$");
    public static readonly Currency JapanYen=new("¥");
    public static readonly Currency JerseyPound=new("£");
    public static readonly Currency KazakhstanTenge=new("лв");
    public static readonly Currency NorthKoreaWon=new("₩");
    public static readonly Currency SouthKoreaWon=new("₩");
    public static readonly Currency KyrgyzstanSom=new("лв");
    public static readonly Currency LaosKip=new("₭");
    public static readonly Currency LebanonPound=new("£");
    public static readonly Currency LiberiaDollar=new("$");
    public static readonly Currency MacedoniaDenar=new("ден");
    public static readonly Currency MalaysiaRinggit=new("RM");
    public static readonly Currency MauritiusRupee=new("₨");
    public static readonly Currency MexicoPeso=new("$");
    public static readonly Currency MongoliaTughrik=new("₮");
    public static readonly Currency MoroccanDirham=new(" د.إ");
    public static readonly Currency MozambiqueMetical=new("MT");
    public static readonly Currency NamibiaDollar=new("$");
    public static readonly Currency NepalRupee=new("₨");
    public static readonly Currency NetherlandsAntillesGuilder=new("ƒ");
    public static readonly Currency NewZealandDollar=new("$");
    public static readonly Currency NicaraguaCordoba=new("C$");
    public static readonly Currency NigeriaNaira=new("₦");
    public static readonly Currency NorwayKrone=new("kr");
    public static readonly Currency OmanRial=new("﷼");
    public static readonly Currency PakistanRupee=new("₨");
    public static readonly Currency PanamaBalboa=new("B/.");
    public static readonly Currency ParaguayGuarani=new("Gs");
    public static readonly Currency PeruSol=new("S/.");
    public static readonly Currency PhilippinesPeso=new("₱");
    public static readonly Currency PolandZloty=new("zł");
    public static readonly Currency QatarRiyal=new("﷼");
    public static readonly Currency RomaniaLeu=new("lei");
    public static readonly Currency RussiaRuble=new("₽");
    public static readonly Currency SaintHelenaPound=new("£");
    public static readonly Currency SaudiArabiaRiyal=new("﷼");
    public static readonly Currency SerbiaDinar=new("Дин.");
    public static readonly Currency SeychellesRupee=new("₨");
    public static readonly Currency SingaporeDollar=new("$");
    public static readonly Currency SolomonIslandsDollar=new("$");
    public static readonly Currency SomaliaShilling=new("S");
    public static readonly Currency SouthKoreanWon=new("₩");
    public static readonly Currency SouthAfricaRand=new("R");
    public static readonly Currency SriLankaRupee=new("₨");
    public static readonly Currency SwedenKrona=new("kr");
    public static readonly Currency SwitzerlandFranc=new("CHF");
    public static readonly Currency SurinameDollar=new("$");
    public static readonly Currency SyriaPound=new("£");
    public static readonly Currency TaiwanNewDollar=new("NT$");
    public static readonly Currency ThailandBaht=new("฿");
    public static readonly Currency TrinidadAndTobagoDollar=new("TT$");
    public static readonly Currency TurkeyLira=new("₺");
    public static readonly Currency TuvaluDollar=new("$");
    public static readonly Currency UkraineHryvnia=new("₴");
    public static readonly Currency UAEDirham=new(" د.إ");
    public static readonly Currency UnitedKingdomPound=new("£");
    public static readonly Currency UnitedStatesDollar=new("$");
    public static readonly Currency UruguayPeso=new("$U");
    public static readonly Currency UzbekistanSom=new("лв");
    public static readonly Currency VenezuelaBolívar=new("Bs");
    public static readonly Currency VietNamDong=new("₫");
    public static readonly Currency YemenRial=new("﷼");
    public static readonly Currency ZimbabweDollar=new("Z$");
    #endregion
}