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