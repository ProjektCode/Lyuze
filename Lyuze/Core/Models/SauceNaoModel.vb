Imports Newtonsoft.Json

Partial Public Class SauceNaoModel

    <JsonProperty("results")>
    Public Property Results() As List(Of Result)
End Class

Partial Public Class Result
	<JsonProperty("header")>
	Public Property Header() As ResultHeader

	<JsonProperty("data")>
	Public Property Data() As Data
End Class


Partial Public Class Data
	<JsonProperty("ext_urls")>
	Public Property ExtUrls() As List(Of Uri)

	<JsonProperty("danbooru_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property DanbooruId() As Long?

	<JsonProperty("gelbooru_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property GelbooruId() As Long?

	<JsonProperty("creator", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Creator() As String

	<JsonProperty("material", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Material() As String

	<JsonProperty("characters", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Characters() As String

	<JsonProperty("source", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Source() As String

	<JsonProperty("title", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Title() As String

	<JsonProperty("as_project", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property AsProject() As String

	<JsonProperty("author_name", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property AuthorName() As String

	<JsonProperty("author_url", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property AuthorUrl() As Uri

	<JsonProperty("pixiv_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property PixivId() As Long?

	<JsonProperty("member_name", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MemberName() As String

	<JsonProperty("member_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MemberId() As Long?

	<JsonProperty("md_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MdId() As Guid?

	<JsonProperty("part")>
	Public Property Part() As String

	<JsonProperty("artist", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Artist() As String

	<JsonProperty("author", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Author() As String

	<JsonProperty("mu_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MuId() As Long?

	<JsonProperty("mal_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MalId() As Long?

	<JsonProperty("created_at", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property CreatedAt() As DateTimeOffset?

	<JsonProperty("tweet_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property TweetId() As String

	<JsonProperty("twitter_user_id", NullValueHandling:=NullValueHandling.Ignore)>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property TwitterUserId() As Long?

	<JsonProperty("twitter_user_handle", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property TwitterUserHandle() As String

	<JsonProperty("imdb_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ImdbId() As String

	<JsonProperty("year", NullValueHandling:=NullValueHandling.Ignore)>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property Year() As Long?

	<JsonProperty("est_time", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property EstTime() As String
End Class

Partial Public Class ResultHeader
	<JsonProperty("similarity")>
	Public Property Similarity() As String

	<JsonProperty("thumbnail")>
	Public Property Thumbnail() As Uri

	<JsonProperty("index_id")>
	Public Property IndexId() As Long

	<JsonProperty("index_name")>
	Public Property IndexName() As String

	<JsonProperty("dupes")>
	Public Property Dupes() As Long

	<JsonProperty("hidden")>
	Public Property Hidden() As Long
End Class


Partial Public Class SauceNaoModel
	Public Shared Function FromJson(ByVal json As String) As SauceNaoModel
		Return JsonConvert.DeserializeObject(Of SauceNaoModel)(json, Converter.Settings)
	End Function
End Class