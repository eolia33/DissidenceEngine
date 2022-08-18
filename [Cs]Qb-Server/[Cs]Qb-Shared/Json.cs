using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace QbBridge
{
    public partial class PlayerData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("job")]
        public Job Job { get; set; }

        [JsonProperty("money")]
        public Money Money { get; set; }

        [JsonProperty("cid")]
        public long Cid { get; set; }

        [JsonProperty("last_updated")]
        public object[] LastUpdated { get; set; }

        [JsonProperty("optin")]
        public bool Optin { get; set; }

        [JsonProperty("source")]
        public long Source { get; set; }

        [JsonProperty("charinfo")]
        public Charinfo Charinfo { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("tracker")]
        public string Tracker { get; set; }

        [JsonProperty("inventory")]
        public string Inventory { get; set; }

        [JsonProperty("gang")]
        public Gang Gang { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("citizenid")]
        public string Citizenid { get; set; }
    }

    public partial class Charinfo
    {
        [JsonProperty("gender")]
        public long Gender { get; set; }

        [JsonProperty("backstory")]
        public string Backstory { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("birthdate")]
        public DateTimeOffset Birthdate { get; set; }

        [JsonProperty("cid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Cid { get; set; }

        [JsonProperty("phone")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Phone { get; set; }
    }

    public partial class Gang
    {
        [JsonProperty("isboss")]
        public bool Isboss { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("grade")]
        public Grade Grade { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class Grade
    {
        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("slot")]
        public long Slot { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("unique")]
        public bool Unique { get; set; }

        [JsonProperty("shouldClose")]
        public bool ShouldClose { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("useable")]
        public bool Useable { get; set; }

        [JsonProperty("info")]
        public InfoUnion Info { get; set; }
    }

    public partial class InfoClass
    {
        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("birthdate")]
        public DateTimeOffset Birthdate { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public long? Gender { get; set; }

        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }

        [JsonProperty("citizenid", NullValueHandling = NullValueHandling.Ignore)]
        public string Citizenid { get; set; }
    }

    public partial class Job
    {
        [JsonProperty("isboss")]
        public bool Isboss { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("grade")]
        public Grade Grade { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("onduty")]
        public bool Onduty { get; set; }

        [JsonProperty("payment")]
        public long Payment { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("commandbinds")]
        public object[] Commandbinds { get; set; }

        [JsonProperty("injail")]
        public long Injail { get; set; }

        [JsonProperty("bloodtype")]
        public string Bloodtype { get; set; }

        [JsonProperty("status")]
        public object[] Status { get; set; }

        [JsonProperty("inside")]
        public Inside Inside { get; set; }

        [JsonProperty("attachmentcraftingrep")]
        public long Attachmentcraftingrep { get; set; }

        [JsonProperty("ishandcuffed")]
        public bool Ishandcuffed { get; set; }

        [JsonProperty("stress")]
        public long Stress { get; set; }

        [JsonProperty("jobrep")]
        public Jobrep Jobrep { get; set; }

        [JsonProperty("phone")]
        public object[] Phone { get; set; }

        [JsonProperty("inlaststand")]
        public bool Inlaststand { get; set; }

        [JsonProperty("craftingrep")]
        public long Craftingrep { get; set; }

        [JsonProperty("walletid")]
        public string Walletid { get; set; }

        [JsonProperty("jailitems")]
        public object[] Jailitems { get; set; }

        [JsonProperty("tracker")]
        public bool Tracker { get; set; }

        [JsonProperty("licences")]
        public Licences Licences { get; set; }

        [JsonProperty("criminalrecord")]
        public Criminalrecord Criminalrecord { get; set; }

        [JsonProperty("phonedata")]
        public Phonedata Phonedata { get; set; }

        [JsonProperty("armor")]
        public long Armor { get; set; }

        [JsonProperty("hunger")]
        public long Hunger { get; set; }

        [JsonProperty("thirst")]
        public long Thirst { get; set; }

        [JsonProperty("callsign")]
        public string Callsign { get; set; }

        [JsonProperty("dealerrep")]
        public long Dealerrep { get; set; }

        [JsonProperty("isdead")]
        public bool Isdead { get; set; }

        [JsonProperty("fitbit")]
        public object[] Fitbit { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }
    }

    public partial class Criminalrecord
    {
        [JsonProperty("hasRecord")]
        public bool HasRecord { get; set; }
    }

    public partial class Inside
    {
        [JsonProperty("apartment")]
        public object[] Apartment { get; set; }
    }

    public partial class Jobrep
    {
        [JsonProperty("taxi")]
        public long Taxi { get; set; }

        [JsonProperty("tow")]
        public long Tow { get; set; }

        [JsonProperty("trucker")]
        public long Trucker { get; set; }

        [JsonProperty("hotdog")]
        public long Hotdog { get; set; }
    }

    public partial class Licences
    {
        [JsonProperty("business")]
        public bool Business { get; set; }

        [JsonProperty("weapon")]
        public bool Weapon { get; set; }

        [JsonProperty("driver")]
        public bool Driver { get; set; }
    }

    public partial class Phonedata
    {
        [JsonProperty("InstalledApps")]
        public object[] InstalledApps { get; set; }

        [JsonProperty("SerialNumber")]
        public long SerialNumber { get; set; }
    }

    public partial class Money
    {
        [JsonProperty("bank")]
        public long Bank { get; set; }

        [JsonProperty("crypto")]
        public long Crypto { get; set; }

        [JsonProperty("cash")]
        public long Cash { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }
    }

    public enum TypeEnum { Item };

    public partial struct InfoUnion
    {
        public object[] AnythingArray;
        public InfoClass InfoClass;

        public static implicit operator InfoUnion(object[] AnythingArray) => new InfoUnion { AnythingArray = AnythingArray };
        public static implicit operator InfoUnion(InfoClass InfoClass) => new InfoUnion { InfoClass = InfoClass };
    }

    public partial class PlayerData
    {
        public static PlayerData FromJson(string json) => JsonConvert.DeserializeObject<PlayerData>(json, QbBridge.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PlayerData self) => JsonConvert.SerializeObject(self, QbBridge.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                InfoUnionConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class InfoUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(InfoUnion) || t == typeof(InfoUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<InfoClass>(reader);
                    return new InfoUnion { InfoClass = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<object[]>(reader);
                    return new InfoUnion { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type InfoUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (InfoUnion)untypedValue;
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.InfoClass != null)
            {
                serializer.Serialize(writer, value.InfoClass);
                return;
            }
            throw new Exception("Cannot marshal type InfoUnion");
        }

        public static readonly InfoUnionConverter Singleton = new InfoUnionConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "item")
            {
                return TypeEnum.Item;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Item)
            {
                serializer.Serialize(writer, "item");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
