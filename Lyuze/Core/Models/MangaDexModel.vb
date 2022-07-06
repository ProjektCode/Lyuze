Imports Newtonsoft.Json

Partial Public Class MangaDex
	<JsonProperty("result")>
	Public Property Result() As String

	<JsonProperty("response")>
	Public Property Response() As String

	<JsonProperty("data")>
	Public Property Data() As Data

End Class

Partial Public Class Data
    <JsonProperty("id")>
    Public Property GuId() As Guid

    <JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("attributes")>
	Public Property Attributes() As DataAttributes

	<JsonProperty("relationships")>
	Public Property Relationships() As Relationship()
End Class

Partial Public Class DataAttributes
	<JsonProperty("title")>
	Public Property Title() As Description

	<JsonProperty("altTitles")>
	Public Property AltTitles() As Description()

	<JsonProperty("description")>
	Public Property Description() As Description

	<JsonProperty("isLocked")>
	Public Property IsLocked() As Boolean

	<JsonProperty("links")>
	Public Property Links() As Links

	<JsonProperty("originalLanguage")>
	Public Property OriginalLanguage() As String

	<JsonProperty("lastVolume")>
	Public Property LastVolume() As String

	<JsonProperty("lastChapter")>
	Public Property LastChapter() As String

	<JsonProperty("publicationDemographic")>
	Public Property PublicationDemographic() As String

	<JsonProperty("status")>
	Public Property Status() As String

	<JsonProperty("year")>
	Public Property Year() As Long

	<JsonProperty("contentRating")>
	Public Property ContentRating() As String

	<JsonProperty("tags")>
	Public Property Tags() As Tag()

	<JsonProperty("state")>
	Public Property State() As String

	<JsonProperty("version")>
	Public Property Version() As Long

	<JsonProperty("createdAt")>
	Public Property CreatedAt() As String

	<JsonProperty("updatedAt")>
	Public Property UpdatedAt() As String

End Class

Partial Public Class Description
	<JsonProperty("en")>
	Public Property En() As String

	<JsonProperty("fr")>
	Public Property Fr() As String
End Class

Partial Public Class Links
	<JsonProperty("property1")>
	Public Property Property1() As String

	<JsonProperty("property2")>
	Public Property Property2() As String
End Class

Partial Public Class Tag
	<JsonProperty("id")>
	Public Property Id() As Guid

	<JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("attributes")>
	Public Property Attributes() As TagAttributes

	<JsonProperty("relationships")>
	Public Property Relationships() As Relationship()
End Class

Partial Public Class TagAttributes
	<JsonProperty("name")>
	Public Property Name() As Description

	<JsonProperty("description")>
	Public Property Description() As Description()

	<JsonProperty("group")>
	Public Property Group() As String

	<JsonProperty("version")>
	Public Property Version() As Long
End Class

Partial Public Class Relationship
	<JsonProperty("id")>
	Public Property Id() As Guid

	<JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("related")>
	Public Property Related() As String

	<JsonProperty("attributes")>
	Public Property Attributes() As RelationshipAttributes
End Class

Partial Public Class RelationshipAttributes
End Class

Partial Public Class MangaDex
	Public Shared Function FromJson(ByVal json As String) As MangaDex
		Return JsonConvert.DeserializeObject(Of MangaDex)(json, Converter.Settings)
	End Function
End Class
