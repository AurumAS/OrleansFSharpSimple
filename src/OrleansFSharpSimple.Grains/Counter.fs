namespace OrleansFSharpSimple.Grains

open System.Threading.Tasks
open Orleans

type ICounterGrain =
    inherit IGrainWithStringKey
    abstract member CurrentCount: unit -> Task<int>
    abstract member Increase: unit -> Task<int>

type CounterGrain() =

    let mutable count = 0

    interface ICounterGrain with

        // Implement the CurrentCount method
        member _.CurrentCount() = Task.FromResult(count)

        // Implement the Increase method
        member _.Increase() =
            count <- count + 1
            Task.FromResult(count)
