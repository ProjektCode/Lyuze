NotInheritable Class YouTube
	Public Shared Function GetThumbnail(url As String) As String

		Dim thumbnail As String = String.Empty
		If url.Equals(String.Empty) Then
			Return thumbnail
		End If

		Dim vidID = url.Replace("https://www.youtube.com/watch?v=", String.Empty)
		thumbnail = "https://img.youtube.com/vi/" & vidID & "/hqdefault.jpg"

		Return thumbnail
	End Function

End Class

