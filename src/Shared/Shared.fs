namespace Shared

open System

type Todo = { Id: Guid; Description: string }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }

type Tweet = { Comment: string; Username: string }

module Tweet =
    let isValid (tweet: Tweet): bool =
        let isNotEmpty = String.IsNullOrWhiteSpace >> not

        (isNotEmpty tweet.Comment)
        && (isNotEmpty tweet.Username)

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo> }

type ITweetsApi =
    { getTweets: unit -> Async<Tweet list>
      postTweet: Tweet -> Async<Tweet> }
