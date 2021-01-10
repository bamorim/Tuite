module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model =
    { Tweets: Tweet list
      TweetComment: string }

type Msg =
    | PostTweet
    | TweetPosted of Tweet
    | GotTweets of Tweet list
    | TweetCommentChanged of string

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITweetsApi>

let init (): Model * Cmd<Msg> =
    let model =
        { Tweets = []
          TweetComment = "" }

    let cmd =
        Cmd.OfAsync.perform todosApi.getTweets () GotTweets

    model, cmd

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | PostTweet ->
        let tweet =
            { Comment = model.TweetComment
              Username = "Frontender" }

        let cmd =
            Cmd.OfAsync.perform todosApi.postTweet tweet TweetPosted

        { model with TweetComment = "" }, cmd
    | GotTweets tweets -> { model with Tweets = tweets }, Cmd.none
    | TweetPosted tweet ->
        { model with
              Tweets = tweet :: model.Tweets },
        Cmd.none
    | TweetCommentChanged newComment -> { model with TweetComment = newComment }, Cmd.none

open Fable.React
open Fable.React.Props

let view (model: Model) (dispatch: Msg -> unit) =
    div [] [
        ul
            []
            (List.map
                (fun tweet ->
                    li [] [
                        span [] [
                            str (sprintf "@%s " tweet.Username)
                        ]
                        span [] [ str tweet.Comment ]
                    ])
                model.Tweets)
        div [] [   
            textarea [ Value model.TweetComment
                       OnChange(fun evt -> dispatch (TweetCommentChanged evt.Value))
                       Placeholder "Say something..."] []
            button [ OnClick(fun _ -> dispatch PostTweet) ] [
                str "Tweet something"
            ]
        ]
    ]
