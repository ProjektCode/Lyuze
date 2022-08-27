Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Partial Public Class WouldYouRather
	<JsonProperty("id", NullValueHandling:=NullValueHandling.Ignore)>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property Id() As Long?

	<JsonProperty("data", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Data() As String
End Class

Partial Public Class WouldYouRather
	Public Shared Function FromJson(ByVal json As String) As WouldYouRather
		Return JsonConvert.DeserializeObject(Of WouldYouRather)(json, Converter.Settings)
	End Function
End Class
