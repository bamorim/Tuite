module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Giraffe.Core

open Shared

type Storage() =
    let tweets = ResizeArray<_>()

    member __.GetTweets() = List.ofSeq tweets

    member __.AddTweet(tweet: Tweet) =
        if Tweet.isValid tweet then
            tweets.Add tweet
            Ok()
        else
            Error "Invalid tweet"

let storage = Storage()

storage.AddTweet(
    { Comment = "Hello tuite"
      Username = "BernardoDCGA" }
)
|> ignore

let tweetsApi: ITweetsApi =
    { getTweets = fun () -> async { return List.rev (storage.GetTweets()) }
      postTweet =
          fun t ->
              async {
                  match storage.AddTweet t with
                  | Ok _ -> return t
                  // Todo: Proper error handling maybe?
                  | Error e -> return failwith e
              } }

let tweetsApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue tweetsApi
    |> Remoting.buildHttpHandler

let appRouter = tweetsApp

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router appRouter
        memory_cache
        use_static "public"
        use_gzip
    }

run app
