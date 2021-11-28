Imports System.Reflection
Imports Discord.Commands

NotInheritable Class commandHandler

    Private Shared ReadOnly cmdService = serviceHandler.getService(Of CommandService)

    Public Shared Async Function loadCommandsAsync() As Task
        Await cmdService.AddModulesAsync(Assembly.GetEntryAssembly, serviceHandler.provider)
    End Function

End Class
