Imports MongoDB.Driver

Public Class DatabaseContext
    Public client As MongoClient
    Public database As IMongoDatabase
    Public playerCollection As IMongoCollection(Of PlayerModel)

    Public Sub New()
        Dim settings = Lyuze.Settings.Data
        client = New MongoClient(settings.Database.MongoDb)
        database = client.GetDatabase("MyDatabase")
        playerCollection = database.GetCollection(Of PlayerModel)("Players")
    End Sub

End Class
