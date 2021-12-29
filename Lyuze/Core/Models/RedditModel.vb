Imports Newtonsoft.Json

Partial Public Class Reddit
	<JsonProperty("kind")>
	Public Property Kind() As String

	<JsonProperty("data")>
	Public Property Data() As RedditData
End Class

Partial Public Class RedditData
	<JsonProperty("after")>
	Public Property After() As Object

	<JsonProperty("dist")>
	Public Property Dist() As Long?

	<JsonProperty("modhash")>
	Public Property Modhash() As String

	<JsonProperty("geo_filter")>
	Public Property GeoFilter() As String

	<JsonProperty("children")>
	Public Property Children() As List(Of PurpleChild)

	<JsonProperty("before")>
	Public Property Before() As Object
End Class

Partial Public Class PurpleChild
	<JsonProperty("kind")>
	Public Property Kind() As String

	<JsonProperty("data")>
	Public Property Data() As PurpleData
End Class

Partial Public Class PurpleData
	<JsonProperty("approved_at_utc")>
	Public Property ApprovedAtUtc() As Object

	<JsonProperty("subreddit")>
	Public Property Subreddit() As String

	<JsonProperty("selftext", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Selftext() As String

	<JsonProperty("user_reports")>
	Public Property UserReports() As List(Of Object)

	<JsonProperty("saved")>
	Public Property Saved() As Boolean

	<JsonProperty("mod_reason_title")>
	Public Property ModReasonTitle() As Object

	<JsonProperty("gilded")>
	Public Property Gilded() As Long

	<JsonProperty("clicked", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Clicked() As Boolean?

	<JsonProperty("title", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Title() As String

	<JsonProperty("link_flair_richtext", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairRichtext() As List(Of LinkFlairRichtext)

	<JsonProperty("subreddit_name_prefixed")>
	Public Property SubredditNamePrefixed() As String

	<JsonProperty("hidden", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Hidden() As Boolean?

	<JsonProperty("pwls", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Pwls() As Long?

	<JsonProperty("link_flair_css_class", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairCssClass() As String

	<JsonProperty("downs")>
	Public Property Downs() As Long

	<JsonProperty("thumbnail_height", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ThumbnailHeight() As Long?

	<JsonProperty("top_awarded_type")>
	Public Property TopAwardedType() As Object

	<JsonProperty("parent_whitelist_status", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ParentWhitelistStatus() As String

	<JsonProperty("hide_score", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property HideScore() As Boolean?

	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("quarantine", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Quarantine() As Boolean?

	<JsonProperty("link_flair_text_color", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairTextColor() As String

	<JsonProperty("upvote_ratio", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property UpvoteRatio() As Double?

	<JsonProperty("author_flair_background_color")>
	Public Property AuthorFlairBackgroundColor() As Object

	<JsonProperty("ups")>
	Public Property Ups() As Long

	<JsonProperty("domain", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Domain() As String

	<JsonProperty("media_embed", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MediaEmbed() As Gildings

	<JsonProperty("thumbnail_width", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ThumbnailWidth() As Long?

	<JsonProperty("author_flair_template_id")>
	Public Property AuthorFlairTemplateId() As Object

	<JsonProperty("is_original_content", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsOriginalContent() As Boolean?

	<JsonProperty("author_fullname")>
	Public Property AuthorFullname() As String

	<JsonProperty("secure_media")>
	Public Property SecureMedia() As Object

	<JsonProperty("is_reddit_media_domain", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsRedditMediaDomain() As Boolean?

	<JsonProperty("is_meta", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsMeta() As Boolean?

	<JsonProperty("category")>
	Public Property Category() As Object

	<JsonProperty("secure_media_embed", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property SecureMediaEmbed() As Gildings

	<JsonProperty("link_flair_text", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairText() As String

	<JsonProperty("can_mod_post")>
	Public Property CanModPost() As Boolean

	<JsonProperty("score")>
	Public Property Score() As Long

	<JsonProperty("approved_by")>
	Public Property ApprovedBy() As Object

	<JsonProperty("is_created_from_ads_ui", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsCreatedFromAdsUi() As Boolean?

	<JsonProperty("author_premium")>
	Public Property AuthorPremium() As Boolean

	<JsonProperty("thumbnail", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Thumbnail() As Uri

	<JsonProperty("edited")>
	Public Property Edited() As Boolean

	<JsonProperty("author_flair_css_class")>
	Public Property AuthorFlairCssClass() As Object

	<JsonProperty("author_flair_richtext")>
	Public Property AuthorFlairRichtext() As List(Of Object)

	<JsonProperty("gildings")>
	Public Property Gildings() As Gildings

	<JsonProperty("post_hint", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property PostHint() As String

	<JsonProperty("content_categories")>
	Public Property ContentCategories() As Object
	<JsonProperty("is_self", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsSelf() As Boolean?

	<JsonProperty("subreddit_type")>
	Public Property SubredditType() As String

	<JsonProperty("created")>
	Public Property Created() As Long

	<JsonProperty("link_flair_type", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairType() As String

	<JsonProperty("wls", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Wls() As Long?

	<JsonProperty("removed_by_category")>
	Public Property RemovedByCategory() As Object

	<JsonProperty("banned_by")>
	Public Property BannedBy() As Object

	<JsonProperty("author_flair_type")>
	Public Property AuthorFlairType() As String

	<JsonProperty("total_awards_received")>
	Public Property TotalAwardsReceived() As Long

	<JsonProperty("allow_live_comments", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property AllowLiveComments() As Boolean?

	<JsonProperty("selftext_html")>
	Public Property SelftextHtml() As Object

	<JsonProperty("likes")>
	Public Property Likes() As Object

	<JsonProperty("suggested_sort", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property SuggestedSort() As String

	<JsonProperty("banned_at_utc")>
	Public Property BannedAtUtc() As Object

	<JsonProperty("url_overridden_by_dest", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property UrlOverriddenByDest() As Uri

	<JsonProperty("view_count")>
	Public Property ViewCount() As Object

	<JsonProperty("archived")>
	Public Property Archived() As Boolean

	<JsonProperty("no_follow")>
	Public Property NoFollow() As Boolean

	<JsonProperty("is_crosspostable", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsCrosspostable() As Boolean?

	<JsonProperty("pinned", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Pinned() As Boolean?

	<JsonProperty("over_18", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Over18() As Boolean?

	<JsonProperty("preview", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Preview() As Preview

	<JsonProperty("all_awardings")>
	Public Property AllAwardings() As List(Of Object)

	<JsonProperty("awarders")>
	Public Property Awarders() As List(Of Object)

	<JsonProperty("media_only", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property MediaOnly() As Boolean?

	<JsonProperty("link_flair_template_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairTemplateId() As Guid?

	<JsonProperty("can_gild")>
	Public Property CanGild() As Boolean

	<JsonProperty("spoiler", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Spoiler() As Boolean?

	<JsonProperty("locked")>
	Public Property Locked() As Boolean

	<JsonProperty("author_flair_text")>
	Public Property AuthorFlairText() As Object

	<JsonProperty("treatment_tags")>
	Public Property TreatmentTags() As List(Of Object)

	<JsonProperty("visited", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Visited() As Boolean?

	<JsonProperty("removed_by")>
	Public Property RemovedBy() As Object

	<JsonProperty("mod_note")>
	Public Property ModNote() As Object

	<JsonProperty("distinguished")>
	Public Property Distinguished() As String

	<JsonProperty("subreddit_id")>
	Public Property SubredditId() As String

	<JsonProperty("author_is_blocked")>
	Public Property AuthorIsBlocked() As Boolean

	<JsonProperty("mod_reason_by")>
	Public Property ModReasonBy() As Object

	<JsonProperty("num_reports")>
	Public Property NumReports() As Object

	<JsonProperty("removal_reason")>
	Public Property RemovalReason() As Object

	<JsonProperty("link_flair_background_color", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkFlairBackgroundColor() As String

	<JsonProperty("id")>
	Public Property Id() As String

	<JsonProperty("is_robot_indexable", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsRobotIndexable() As Boolean?

	<JsonProperty("num_duplicates", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property NumDuplicates() As Long?

	<JsonProperty("report_reasons")>
	Public Property ReportReasons() As Object

	<JsonProperty("author")>
	Public Property Author() As String

	<JsonProperty("discussion_type")>
	Public Property DiscussionType() As Object

	<JsonProperty("num_comments", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property NumComments() As Long?

	<JsonProperty("send_replies")>
	Public Property SendReplies() As Boolean

	<JsonProperty("media")>
	Public Property Media() As Object

	<JsonProperty("contest_mode", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ContestMode() As Boolean?

	<JsonProperty("author_patreon_flair")>
	Public Property AuthorPatreonFlair() As Boolean

	<JsonProperty("author_flair_text_color")>
	Public Property AuthorFlairTextColor() As Object

	<JsonProperty("permalink")>
	Public Property Permalink() As String

	<JsonProperty("whitelist_status", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property WhitelistStatus() As String

	<JsonProperty("stickied")>
	Public Property Stickied() As Boolean

	<JsonProperty("URL", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Url() As Uri

	<JsonProperty("subreddit_subscribers", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property SubredditSubscribers() As Long?

	<JsonProperty("created_utc")>
	Public Property CreatedUtc() As Long

	<JsonProperty("num_crossposts", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property NumCrossposts() As Long?

	<JsonProperty("mod_reports")>
	Public Property ModReports() As List(Of Object)

	<JsonProperty("is_video", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsVideo() As Boolean?

	<JsonProperty("comment_type")>
	Public Property CommentType() As Object

	<JsonProperty("replies", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Replies() As RepliesUnion?

	<JsonProperty("collapsed_reason_code")>
	Public Property CollapsedReasonCode() As Object

	<JsonProperty("parent_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ParentId() As String

	<JsonProperty("collapsed", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Collapsed() As Boolean?

	<JsonProperty("body", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Body() As String

	<JsonProperty("is_submitter", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property IsSubmitter() As Boolean?

	<JsonProperty("body_html", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property BodyHtml() As String

	<JsonProperty("collapsed_reason")>
	Public Property CollapsedReason() As Object

	<JsonProperty("associated_award")>
	Public Property AssociatedAward() As Object

	<JsonProperty("unrepliable_reason")>
	Public Property UnrepliableReason() As Object

	<JsonProperty("score_hidden", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property ScoreHidden() As Boolean?

	<JsonProperty("link_id", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property LinkId() As String

	<JsonProperty("controversiality", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Controversiality() As Long?

	<JsonProperty("depth", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property Depth() As Long?

	<JsonProperty("collapsed_because_crowd_control")>
	Public Property CollapsedBecauseCrowdControl() As Object
End Class

Partial Public Class Gildings
End Class

Partial Public Class LinkFlairRichtext
	<JsonProperty("a", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property A() As String

	<JsonProperty("u", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property U() As Uri

	<JsonProperty("e")>
	Public Property E() As String

	<JsonProperty("t", NullValueHandling:=NullValueHandling.Ignore)>
	Public Property T() As String
End Class

Partial Public Class Preview
	<JsonProperty("images")>
	Public Property Images() As List(Of _Image)

	<JsonProperty("enabled")>
	Public Property Enabled() As Boolean
End Class

Partial Public Class _Image
	<JsonProperty("source")>
	Public Property Source() As Source

	<JsonProperty("resolutions")>
	Public Property Resolutions() As List(Of Source)

	<JsonProperty("variants")>
	Public Property Variants() As Gildings

	<JsonProperty("id")>
	Public Property Id() As String
End Class


Partial Public Class Source
	<JsonProperty("URL")>
	Public Property Url() As Uri

	<JsonProperty("width")>
	Public Property Width() As Long

	<JsonProperty("height")>
	Public Property Height() As Long
End Class

Partial Public Class RepliesClass
	<JsonProperty("kind")>
	Public Property Kind() As String

	<JsonProperty("data")>
	Public Property Data() As RepliesData
End Class

Partial Public Class RepliesData
	<JsonProperty("after")>
	Public Property After() As Object

	<JsonProperty("dist")>
	Public Property Dist() As Object

	<JsonProperty("modhash")>
	Public Property Modhash() As String

	<JsonProperty("geo_filter")>
	Public Property GeoFilter() As String

	<JsonProperty("children")>
	Public Property Children() As List(Of FluffyChild)

	<JsonProperty("before")>
	Public Property Before() As Object
End Class

Partial Public Class FluffyChild
	<JsonProperty("kind")>
	Public Property Kind() As String

	<JsonProperty("data")>
	Public Property Data() As FluffyData
End Class


Partial Public Class FluffyData
	<JsonProperty("subreddit_id")>
	Public Property SubredditId() As String

	<JsonProperty("approved_at_utc")>
	Public Property ApprovedAtUtc() As Object

	<JsonProperty("author_is_blocked")>
	Public Property AuthorIsBlocked() As Boolean

	<JsonProperty("comment_type")>
	Public Property CommentType() As Object

	<JsonProperty("awarders")>
	Public Property Awarders() As List(Of Object)

	<JsonProperty("mod_reason_by")>
	Public Property ModReasonBy() As Object

	<JsonProperty("banned_by")>
	Public Property BannedBy() As Object

	<JsonProperty("author_flair_type")>
	Public Property AuthorFlairType() As String

	<JsonProperty("total_awards_received")>
	Public Property TotalAwardsReceived() As Long

	<JsonProperty("subreddit")>
	Public Property Subreddit() As String

	<JsonProperty("author_flair_template_id")>
	Public Property AuthorFlairTemplateId() As Object

	<JsonProperty("likes")>
	Public Property Likes() As Object

	<JsonProperty("replies")>
	Public Property Replies() As String

	<JsonProperty("user_reports")>
	Public Property UserReports() As List(Of Object)

	<JsonProperty("saved")>
	Public Property Saved() As Boolean

	<JsonProperty("id")>
	Public Property Id() As String

	<JsonProperty("banned_at_utc")>
	Public Property BannedAtUtc() As Object

	<JsonProperty("mod_reason_title")>
	Public Property ModReasonTitle() As Object

	<JsonProperty("gilded")>
	Public Property Gilded() As Long

	<JsonProperty("archived")>
	Public Property Archived() As Boolean

	<JsonProperty("collapsed_reason_code")>
	Public Property CollapsedReasonCode() As Object

	<JsonProperty("no_follow")>
	Public Property NoFollow() As Boolean

	<JsonProperty("author")>
	Public Property Author() As String

	<JsonProperty("can_mod_post")>
	Public Property CanModPost() As Boolean

	<JsonProperty("created_utc")>
	Public Property CreatedUtc() As Long

	<JsonProperty("send_replies")>
	Public Property SendReplies() As Boolean

	<JsonProperty("parent_id")>
	Public Property ParentId() As String

	<JsonProperty("score")>
	Public Property Score() As Long

	<JsonProperty("author_fullname")>
	Public Property AuthorFullname() As String

	<JsonProperty("removal_reason")>
	Public Property RemovalReason() As Object

	<JsonProperty("approved_by")>
	Public Property ApprovedBy() As Object

	<JsonProperty("mod_note")>
	Public Property ModNote() As Object

	<JsonProperty("all_awardings")>
	Public Property AllAwardings() As List(Of Object)

	<JsonProperty("body")>
	Public Property Body() As String

	<JsonProperty("edited")>
	Public Property Edited() As Boolean

	<JsonProperty("top_awarded_type")>
	Public Property TopAwardedType() As Object

	<JsonProperty("author_flair_css_class")>
	Public Property AuthorFlairCssClass() As Object

	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("is_submitter")>
	Public Property IsSubmitter() As Boolean

	<JsonProperty("downs")>
	Public Property Downs() As Long

	<JsonProperty("author_flair_richtext")>
	Public Property AuthorFlairRichtext() As List(Of Object)

	<JsonProperty("author_patreon_flair")>
	Public Property AuthorPatreonFlair() As Boolean

	<JsonProperty("body_html")>
	Public Property BodyHtml() As String

	<JsonProperty("gildings")>
	Public Property Gildings() As Gildings

	<JsonProperty("collapsed_reason")>
	Public Property CollapsedReason() As Object

	<JsonProperty("distinguished")>
	Public Property Distinguished() As Object

	<JsonProperty("associated_award")>
	Public Property AssociatedAward() As Object

	<JsonProperty("stickied")>
	Public Property Stickied() As Boolean

	<JsonProperty("author_premium")>
	Public Property AuthorPremium() As Boolean

	<JsonProperty("can_gild")>
	Public Property CanGild() As Boolean

	<JsonProperty("link_id")>
	Public Property LinkId() As String

	<JsonProperty("unrepliable_reason")>
	Public Property UnrepliableReason() As Object

	<JsonProperty("author_flair_text_color")>
	Public Property AuthorFlairTextColor() As Object

	<JsonProperty("score_hidden")>
	Public Property ScoreHidden() As Boolean

	<JsonProperty("permalink")>
	Public Property Permalink() As String

	<JsonProperty("subreddit_type")>
	Public Property SubredditType() As String

	<JsonProperty("locked")>
	Public Property Locked() As Boolean

	<JsonProperty("report_reasons")>
	Public Property ReportReasons() As Object

	<JsonProperty("created")>
	Public Property Created() As Long

	<JsonProperty("author_flair_text")>
	Public Property AuthorFlairText() As Object

	<JsonProperty("treatment_tags")>
	Public Property TreatmentTags() As List(Of Object)

	<JsonProperty("collapsed")>
	Public Property Collapsed() As Boolean

	<JsonProperty("subreddit_name_prefixed")>
	Public Property SubredditNamePrefixed() As String

	<JsonProperty("controversiality")>
	Public Property Controversiality() As Long

	<JsonProperty("depth")>
	Public Property Depth() As Long

	<JsonProperty("author_flair_background_color")>
	Public Property AuthorFlairBackgroundColor() As Object

	<JsonProperty("collapsed_because_crowd_control")>
	Public Property CollapsedBecauseCrowdControl() As Object

	<JsonProperty("mod_reports")>
	Public Property ModReports() As List(Of Object)

	<JsonProperty("num_reports")>
	Public Property NumReports() As Object

	<JsonProperty("ups")>
	Public Property Ups() As Long

End Class

Partial Public Structure RepliesUnion
	Public RepliesClass As RepliesClass
	Public _string As String

	Public Shared Widening Operator CType(ByVal RepliesClass As RepliesClass) As RepliesUnion
		Return New RepliesUnion With {.RepliesClass = RepliesClass}
	End Operator
	Public Shared Widening Operator CType(ByVal _string As String) As RepliesUnion
		Return New RepliesUnion With {._string = _string}
	End Operator
End Structure

Partial Public Class Reddit
	Public Shared Function FromJson(ByVal json As String) As List(Of Reddit)
		Return JsonConvert.DeserializeObject(Of List(Of Reddit))(json, Converter.Settings)
	End Function
End Class