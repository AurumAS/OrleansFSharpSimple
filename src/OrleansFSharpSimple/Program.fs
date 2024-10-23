namespace OrleansFSharpSimple

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Orleans
open Orleans.Hosting

open OrleansFSharpSimple.Grains

module Load =
    [<assembly: Orleans.ApplicationPartAttribute("OrleansFSharpSimple.Grains")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Core.Abstractions")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Core")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Runtime")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Reminders")>]
    [<assembly: Orleans.ApplicationPartAttribute("OrleansDashboard.Core")>]
    [<assembly: Orleans.ApplicationPartAttribute("OrleansDashboard")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization.Abstractions")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization")>]
    ()


module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Host.UseOrleans(fun siloBuilder ->
            siloBuilder.UseLocalhostClustering().UseDashboard(fun x -> x.HostSelf <- true)
            |> ignore)
        |> ignore


        let app = builder.Build()

        app.MapGet(
            "/",
            Func<IGrainFactory, Task<IResult>>(fun grains ->
                task {
                    let counterGrain = grains.GetGrain<ICounterGrain>("counter")
                    let! currentCount = counterGrain.CurrentCount()
                    return currentCount |> Results.Ok
                })
        )
        |> ignore

        app.MapPut(
            "/",
            Func<IGrainFactory, Task<IResult>>(fun grains ->
                task {
                    let counterGrain = grains.GetGrain<ICounterGrain>("counter")
                    let! currentCount = counterGrain.Increase()
                    return currentCount |> Results.Ok
                })
        )
        |> ignore

        app.Map("/dashboard", Action<IApplicationBuilder>(fun map -> map.UseOrleansDashboard() |> ignore))
        |> ignore

        app.Run()

        0
