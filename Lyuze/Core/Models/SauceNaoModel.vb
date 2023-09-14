Imports Newtonsoft.Json

Partial Public Class SauceNaoModel
    <JsonProperty("results")>
    Public Property Results() As List(Of Result)
End Class

Partial Public Class Result
    <JsonProperty("header")>
    Public Property Header() As Header

    <JsonProperty("data")>
    Public Property Data() As Data
End Class

Partial Public Class Data
    <JsonProperty("ext_urls", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property ExtUrls() As List(Of Uri)

    <JsonProperty("title", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Title() As String

    <JsonProperty("pixiv_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property PixivId() As Long?

    <JsonProperty("member_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MemberName() As String

    <JsonProperty("member_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MemberId() As Long?

    <JsonProperty("bcy_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property BcyId() As Long?

    <JsonProperty("member_link_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MemberLinkId() As Long?

    <JsonProperty("bcy_type", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property BcyType() As String

    <JsonProperty("danbooru_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property DanbooruId() As Long?

    <JsonProperty("yandere_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property YandereId() As Long?

    <JsonProperty("gelbooru_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property GelbooruId() As Long?

    <JsonProperty("konachan_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property KonachanId() As Long?

    <JsonProperty("material", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Material() As String

    <JsonProperty("characters", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Characters() As String

    <JsonProperty("source", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Source() As String

    <JsonProperty("da_id", NullValueHandling:=NullValueHandling.Ignore)>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property DaId() As Long?

    <JsonProperty("author_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property AuthorName() As String

    <JsonProperty("author_url", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property AuthorUrl() As Uri

    <JsonProperty("md_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MdId() As Guid?

    <JsonProperty("mu_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MuId() As Long?

    <JsonProperty("mal_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property MalId() As Long?

    <JsonProperty("part", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Part() As String

    <JsonProperty("artist", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Artist() As String

    <JsonProperty("author", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Author() As String

    <JsonProperty("anidb_aid", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property AnidbAid() As Long?

    <JsonProperty("anilist_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property AnilistId() As Long?

    <JsonProperty("year", NullValueHandling:=NullValueHandling.Ignore)>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property Year() As Long?

    <JsonProperty("est_time", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property EstTime() As String

    <JsonProperty("drawr_id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property DrawrId() As Long?

    <JsonProperty("eng_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property EngName() As String

    <JsonProperty("jp_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property JpName() As String

    <JsonProperty("as_project", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property AsProject() As String

    <JsonProperty("published", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Published() As DateTimeOffset?

    <JsonProperty("service", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Service() As String

    <JsonProperty("service_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property ServiceName() As String

    <JsonProperty("id", NullValueHandling:=NullValueHandling.Ignore, PropertyName:="LongID")>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property Id() As Long?

    <JsonProperty("user_id", NullValueHandling:=NullValueHandling.Ignore)>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property UserId() As Long?

    <JsonProperty("user_name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property UserName() As String
End Class


Partial Public Class Header
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