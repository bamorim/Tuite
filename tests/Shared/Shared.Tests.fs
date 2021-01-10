module Shared.Tests

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open Shared

let shared =
    testList
        "Shared"
        [ testCase "Tweet needs username"
          <| fun _ ->
              let expected = false

              let actual =
                  Tweet.isValid
                      { Comment = "With Comment"
                        Username = "" }

              Expect.equal actual expected "Should be false" ]
