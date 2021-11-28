Imports Newtonsoft.Json

Friend Class ParseStringConverter
    Inherits JsonConverter

    Public Overrides Function CanConvert(ByVal t As Type) As Boolean
        Return t Is GetType(Long) OrElse t Is GetType(Long?)
    End Function

    Public Overrides Function ReadJson(ByVal reader As JsonReader, ByVal t As Type, ByVal existingValue As Object, ByVal serializer As JsonSerializer) As Object
        If reader.TokenType = JsonToken.Null Then
            Return Nothing
        End If
        Dim value = serializer.Deserialize(Of String)(reader)
        Dim l As Long = Nothing
        If Int64.TryParse(value, l) Then
            Return l
        End If
        Throw New Exception("Cannot unmarshal type long")
    End Function

    Public Overrides Sub WriteJson(ByVal writer As JsonWriter, ByVal untypedValue As Object, ByVal serializer As JsonSerializer)
        If untypedValue Is Nothing Then
            serializer.Serialize(writer, Nothing)
            Return
        End If
        Dim value = DirectCast(untypedValue, Long)
        serializer.Serialize(writer, value.ToString())
        Return
    End Sub

End Class

