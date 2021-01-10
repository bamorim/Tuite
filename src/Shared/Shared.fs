namespace Shared

open System

type Tweet = { Comment: string; Username: string }

module Tweet =
    let isValid (tweet: Tweet): bool =
        let isNotEmpty = String.IsNullOrWhiteSpace >> not

        (isNotEmpty tweet.Comment)
        && (isNotEmpty tweet.Username)

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

type ITweetsApi =
    { getTweets: unit -> Async<Tweet list>
      postTweet: Tweet -> Async<Tweet> }
