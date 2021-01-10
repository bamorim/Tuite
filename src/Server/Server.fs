module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn
open Giraffe.Core

open Shared

type Storage() =
    let todos = ResizeArray<_>()
    let tweets = ResizeArray<_>()

    member __.GetTodos() = List.ofSeq todos

    member __.AddTodo(todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok()
        else
            Error "Invalid todo"

    member __.GetTweets() = List.ofSeq tweets

    member __.AddTweet(tweet: Tweet) =
        if Tweet.isValid tweet then
            tweets.Add tweet
            Ok()
        else
            Error "Invalid tweet"

let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project")
|> ignore

storage.AddTodo(Todo.create "Write your app")
|> ignore

storage.AddTodo(Todo.create "Ship it !!!")
|> ignore

storage.AddTweet(
    { Comment = "Hello tuite"
      Username = "BernardoDCGA" }
)
|> ignore

let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }
      addTodo =
          fun todo ->
              async {
                  match storage.AddTodo todo with
                  | Ok () -> return todo
                  | Error e -> return failwith e
              } }

let todosApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue todosApi
    |> Remoting.buildHttpHandler

let tweetsApi: ITweetsApi =
    { getTweets = fun () -> async { return storage.GetTweets() }
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

let appRouter = choose [ tweetsApp; todosApp ]

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router appRouter
        memory_cache
        use_static "public"
        use_gzip
    }

run app
