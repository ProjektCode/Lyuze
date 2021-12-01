Imports Newtonsoft.Json

Partial Public Class Unsplash
	<JsonProperty("id")>
	Public Property Id() As String

	<JsonProperty("created_at")>
	Public Property CreatedAt() As DateTimeOffset

	<JsonProperty("updated_at")>
	Public Property UpdatedAt() As DateTimeOffset

	<JsonProperty("width")>
	Public Property Width() As Long

	<JsonProperty("height")>
	Public Property Height() As Long

	<JsonProperty("color")>
	Public Property Color() As String

	<JsonProperty("blur_hash")>
	Public Property BlurHash() As String

	<JsonProperty("downloads")>
	Public Property Downloads() As Long

	<JsonProperty("likes")>
	Public Property Likes() As Long

	<JsonProperty("liked_by_user")>
	Public Property LikedByUser() As Boolean

	<JsonProperty("description")>
	Public Property Description() As String

	<JsonProperty("exif")>
	Public Property Exif() As Exif

	<JsonProperty("location")>
	Public Property Location() As Location

	<JsonProperty("current_user_collections")>
	Public Property CurrentUserCollections() As List(Of CurrentUserCollection)

	<JsonProperty("urls")>
	Public Property Urls() As Urls

	<JsonProperty("links")>
	Public Property Links() As UnsplashLinks

	<JsonProperty("user")>
	Public Property User() As User
End Class

Partial Public Class CurrentUserCollection
	<JsonProperty("id")>
	Public Property Id() As Long

	<JsonProperty("title")>
	Public Property Title() As String

	<JsonProperty("published_at")>
	Public Property PublishedAt() As DateTimeOffset

	<JsonProperty("last_collected_at")>
	Public Property LastCollectedAt() As DateTimeOffset

	<JsonProperty("updated_at")>
	Public Property UpdatedAt() As DateTimeOffset

	<JsonProperty("cover_photo")>
	Public Property CoverPhoto() As Object

	<JsonProperty("user")>
	Public Property User() As Object
End Class

Partial Public Class Exif
	<JsonProperty("make")>
	Public Property Make() As String

	<JsonProperty("model")>
	Public Property Model() As String

	<JsonProperty("exposure_time")>
	Public Property ExposureTime() As String

	<JsonProperty("aperture")>
	Public Property Aperture() As String

	<JsonProperty("iso")>
	Public Property Iso() As Long
End Class

Partial Public Class UnsplashLinks
	<JsonProperty("self")>
	Public Property Self() As Uri

	<JsonProperty("html")>
	Public Property Html() As Uri

	<JsonProperty("download")>
	Public Property Download() As Uri

	<JsonProperty("download_location")>
	Public Property DownloadLocation() As Uri
End Class

Partial Public Class Location
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("city")>
	Public Property City() As String

	<JsonProperty("country")>
	Public Property Country() As String

	<JsonProperty("position")>
	Public Property Position() As Position
End Class

Partial Public Class Position
	<JsonProperty("latitude")>
	Public Property Latitude() As Double

	<JsonProperty("longitude")>
	Public Property Longitude() As Double
End Class

Partial Public Class Urls
	<JsonProperty("raw")>
	Public Property Raw() As Uri

	<JsonProperty("full")>
	Public Property Full() As Uri

	<JsonProperty("regular")>
	Public Property Regular() As Uri

	<JsonProperty("small")>
	Public Property Small() As Uri

	<JsonProperty("thumb")>
	Public Property Thumb() As Uri
End Class

Partial Public Class User
	<JsonProperty("id")>
	Public Property Id() As String

	<JsonProperty("updated_at")>
	Public Property UpdatedAt() As DateTimeOffset

	<JsonProperty("username")>
	Public Property Username() As String

	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("portfolio_url")>
	Public Property PortfolioUrl() As Uri

	<JsonProperty("bio")>
	Public Property Bio() As String

	<JsonProperty("location")>
	Public Property Location() As String

	<JsonProperty("total_likes")>
	Public Property TotalLikes() As Long

	<JsonProperty("total_photos")>
	Public Property TotalPhotos() As Long

	<JsonProperty("total_collections")>
	Public Property TotalCollections() As Long

	<JsonProperty("instagram_username")>
	Public Property InstagramUsername() As String

	<JsonProperty("twitter_username")>
	Public Property TwitterUsername() As String

	<JsonProperty("links")>
	Public Property Links() As UserLinks
End Class

Partial Public Class UserLinks
	<JsonProperty("self")>
	Public Property Self() As Uri

	<JsonProperty("html")>
	Public Property Html() As Uri

	<JsonProperty("photos")>
	Public Property Photos() As Uri

	<JsonProperty("likes")>
	Public Property Likes() As Uri

	<JsonProperty("portfolio")>
	Public Property Portfolio() As Uri
End Class

Partial Public Class Unsplash
	Public Shared Function FromJson(ByVal json As String) As Unsplash
		Return JsonConvert.DeserializeObject(Of Unsplash)(json, Converter.Settings)
	End Function
End Class