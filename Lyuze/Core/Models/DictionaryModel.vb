Imports Newtonsoft.Json

Partial Public Class Dictionary
	<JsonProperty("word")>
	Public Property Word() As String

	<JsonProperty("phonetics")>
	Public Property Phonetics() As List(Of Phonetic)

	<JsonProperty("meanings")>
	Public Property Meanings() As List(Of Meaning)
End Class

Partial Public Class Meaning
	<JsonProperty("partOfSpeech")>
	Public Property PartOfSpeech() As String

	<JsonProperty("definitions")>
	Public Property Definitions() As List(Of Definition)
End Class

Partial Public Class Definition
	<JsonProperty("definition")>
	Public Property DefinitionDefinition() As String

	<JsonProperty("example")>
	Public Property Example() As String

	<JsonProperty("synonyms", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Synonyms() As List(Of String)
End Class

Partial Public Class Phonetic
	<JsonProperty("text")>
	Public Property Text() As String

	<JsonProperty("audio")>
	Public Property Audio() As Uri
End Class

Partial Public Class Dictionary
	Public Shared Function FromJson(ByVal json As String) As List(Of Dictionary)
		Return JsonConvert.DeserializeObject(Of List(Of Dictionary))(json, Converter.Settings)
	End Function
End Class
