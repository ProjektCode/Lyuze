Imports Newtonsoft.Json
Imports System.IO

Partial Public Class Settings
	<JsonProperty("_Discord")>
	Public Property Discord() As _Discord

	<JsonProperty("IDs")>
	Public Property IDs() As IDs

	<JsonProperty("APIs")>
	Public Property ApIs() As ApIs
End Class

Partial Public Class ApIs
	<JsonProperty("Tenor")>
	Public Property Tenor() As String

	<JsonProperty("UnsplashAccess")>
	Public Property UnsplashAccess() As String

	<JsonProperty("UnsplashSecret")>
	Public Property UnsplashSecret() As String
End Class


Partial Public Class _Discord
	<JsonProperty("Name")>
	Public Property Name() As String

	<JsonProperty("Token")>
	Public Property Token() As String

	<JsonProperty("Prefix")>
	Public Property Prefix() As String
End Class


Partial Public Class IDs
	<JsonProperty("Owner ID")>
	Public Property OwnerId() As ULong

	<JsonProperty("Welcome ID")>
	Public Property WelcomeId() As ULong

	<JsonProperty("Report ID")>
	Public Property ReportId() As ULong

	<JsonProperty("Error ID")>
	Public Property ErrorId() As ULong

	<JsonProperty("Kick ID")>
	Public Property KickId() As ULong
End Class



Partial Public Class Settings
	Public Shared Function Data() As Settings
		Dim basePath = AppDomain.CurrentDomain.BaseDirectory
		Dim filePath = $"{basePath}Resources\Settings\settings.json"
		Dim json = File.ReadAllText(filePath)
		Return JsonConvert.DeserializeObject(Of Settings)(json, Converter.Settings)
	End Function
End Class