Imports MongoDB.Driver
Imports Discord
Imports Discord.Commands

Public Class DatabaseContext
    Public _client As MongoClient
    Public _database As IMongoDatabase
    Public _collection As IMongoCollection(Of PlayerModel)

    Public Sub New()
        ConnectDatabase()
    End Sub

    Public Sub ConnectDatabase()
        Dim settings = Lyuze.Settings.Data
        _client = New MongoClient(settings.Database.MongoDb)
        _database = _client.GetDatabase("MyDatabase")
        _collection = _database.GetCollection(Of PlayerModel)("Players")
    End Sub

End Class
