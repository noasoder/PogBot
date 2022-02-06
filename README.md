# PogBot
Discord bot that sends overwatch artworks

## Calling the bot
Invoke the bot by sending "pog" in a server with PogBot invited.

PogBot will return a random overwatch character artwork. 

## Custom Search
"pog" followed by anything will make a custom search and return an overwatch artwork based on your message.

## Setup
- Set up an application with a bot on Discord https://discord.com/developers/applications and get your bot token
- Set up a Custom Search Engine with Google https://cse.google.com/
- Find your Google API key and context https://developers.google.com/custom-search/v1/introduction

Create the keys file PogBot/src/keys.txt. It should look like this with all the fields filled in:
```
token=[YOUR_TOKEN]
googlekey=[YOUR_API_KEY]
googlecx=[YOUR_CONTEXT]
```
