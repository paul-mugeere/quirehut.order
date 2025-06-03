namespace quirehut.order.domain

type Result<'TSuccess, 'TFailure> =
        | Ok of 'TSuccess
        | Error of 'TFailure

module Result =
    
    let bind switchFn twoTrackFn =
        match twoTrackFn with
        | Ok value -> switchFn value
        | Error error -> Error error
    
    let mapError errorFn twoTrackFn =
        match twoTrackFn with
        | Ok value -> Ok value
        | Error error -> Error (errorFn error)
    
    let map oneTrackFn twoTrackFn =
        match twoTrackFn with
        | Ok value -> Ok (oneTrackFn value)
        | Error error -> Error error
                

type ResultBuilder() =
    member this.Bind(x, f) =
        Result.bind f x
    
    member this.Return(x) =
        Ok x
    
    member this.ReturnFrom(x) =
        x
    
    member this.MergeSome(oneTrackFn, twoTrackFn) =
        Result.map oneTrackFn twoTrackFn
    
    member this.MergeSources(t1: Result<'T,'TError>, t2: Result<'U,'TError>) : Result<'T * 'U, 'TError> =
        match t1, t2 with
        | Ok v1, Ok v2 -> Ok(v1, v2)
        | Error e, _ -> Error e
        | _, Error e -> Error e
    

module ResultBuilder =
    let result = ResultBuilder()