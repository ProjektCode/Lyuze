Imports System.IO
Imports Newtonsoft.Json

NotInheritable Class settingsHandler
    Public ReadOnly Property token As String
    Public ReadOnly Property prefix As String
    Public ReadOnly Property ownerID As String
    Public ReadOnly Property welcomeID As String
    Public ReadOnly Property tenorAPIKey As String
    Public ReadOnly Property kickID As String

    <JsonIgnore> Private Shared ReadOnly Filename As String = "settings.json"
    Private Shared ReadOnly path = AppDomain.CurrentDomain.BaseDirectory
    Private Shared ReadOnly dir = $"{path}\Resources\Settings\{Filename}"

    <JsonConstructor>
    Public Sub New(Token As String, Prefix As String, OwnerID As String, WelcomeID As String, TenorAPIKey As String, KickID As String)
        Me.token = Token
        Me.prefix = Prefix
        Me.ownerID = OwnerID
        Me.welcomeID = WelcomeID
        Me.tenorAPIKey = TenorAPIKey
        Me.kickID = KickID
    End Sub


    Public Shared Function Load() As settingsHandler
        If File.Exists(dir) Then
            Dim Json As String = File.ReadAllText(dir)
            Return JsonConvert.DeserializeObject(Of settingsHandler)(Json)

        End If
        Return Create()
    End Function

    Private Shared Function Create() As settingsHandler
        Dim RawToken As String = Nothing
        Dim RawPrefix As String = Nothing
        Dim RawOwnerID As String = Nothing
        Dim RawWelcomeID As String = Nothing
        Dim RawTenorAPIKey As String = Nothing
        Dim RawKickID As String = Nothing

        Do While String.IsNullOrEmpty(RawToken)
            Console.Write($"{vbTab}Enter Token: ")
            RawToken = Console.ReadLine
        Loop
        Do While String.IsNullOrEmpty(RawPrefix)
            Console.Write($"{vbTab}Enter Prefix: ")
            RawPrefix = Console.ReadLine
        Loop
        Do While String.IsNullOrEmpty(RawOwnerID)
            Console.Write($"{vbTab}Enter Owner ID: ")
            RawOwnerID = Console.ReadLine
        Loop
        Do While String.IsNullOrEmpty(RawWelcomeID)
            Console.Write($"{vbTab}Enter Welcome Channel ID: ")
            RawWelcomeID = Console.ReadLine
        Loop
        Do While String.IsNullOrEmpty(RawTenorAPIKey)
            Console.Write($"{vbTab}Enter your Tenor API Key: ")
            RawTenorAPIKey = Console.ReadLine
        Loop


        Dim Settings As New settingsHandler(RawToken, RawPrefix, RawOwnerID, RawWelcomeID, RawTenorAPIKey, RawKickID)
        Settings.Save()
        Return Settings
    End Function

    Private Sub Save()
        Dim Json As String = JsonConvert.SerializeObject(Me, Formatting.Indented)
        File.WriteAllText(dir, Json)
    End Sub

End Class

Public Structure botConfig
    Private privateToken As String
    <JsonProperty("token")>
    Public Property Token() As String
        Get
            Return privateToken
        End Get
        Private Set(value As String)
            privateToken = value
        End Set
    End Property
    Private privatePrefix As String
    <JsonProperty("prefix")>
    Public Property Prefix() As String
        Get
            Return privatePrefix
        End Get
        Private Set(value As String)
            privatePrefix = value
        End Set
    End Property
    Private privateOwnerID As String
    <JsonProperty("ownerid")>
    Public Property OwnerID() As String
        Get
            Return privateOwnerID
        End Get
        Private Set(value As String)
            privateOwnerID = value
        End Set
    End Property
    Private privateWelcomeID As String
    <JsonProperty("welcomeid")>
    Public Property WelcomeID() As String
        Get
            Return privateWelcomeID
        End Get
        Private Set(value As String)
            privateWelcomeID = value
        End Set
    End Property
    Private privateTenorAPIKey As String
    <JsonProperty("tenor-apikey")>
    Public Property tenorAPIKey() As String
        Get
            Return privateTenorAPIKey
        End Get
        Private Set(value As String)
            privateTenorAPIKey = value
        End Set
    End Property
    Private privateKickID As String
    <JsonProperty("kickid")>
    Public Property KickID() As String
        Get
            Return privateKickID
        End Get
        Private Set(value As String)
            privateKickID = value
        End Set
    End Property
End Structure