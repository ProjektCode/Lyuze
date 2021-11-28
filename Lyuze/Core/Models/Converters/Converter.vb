Imports System.Globalization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Friend Module Converter
	Public ReadOnly Settings As New JsonSerializerSettings With {
			.MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			.DateParseHandling = DateParseHandling.None,
			.NullValueHandling = NullValueHandling.Ignore,
			.Formatting = Formatting.Indented,
			.Converters = {
				New IsoDateTimeConverter With {.DateTimeStyles = DateTimeStyles.AssumeUniversal}
			}
		}
End Module