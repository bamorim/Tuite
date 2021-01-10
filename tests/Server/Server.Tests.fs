module Server.Tests

open Expecto

let server = testList "Server" []

let all =
    testList "All" [ Shared.Tests.shared; server ]

[<EntryPoint>]
let main _ = runTests defaultConfig all
