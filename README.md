# PogBot
Discord bot that sends Overwatch and League of Legends artworks

## Commands
- "Pog" sends a random Overwatch or League of Legends artwork. You can add a custom search phrase appended to the commnad to get a specific result.
- "NASA" sends the [Astronomy Picture of the Day](https://apod.nasa.gov/apod/) from NASA.

## Setup
- Set up an application with a bot on [Discord](https://discord.com/developers/applications) and get your bot token
- Set up a [Custom Search Engine](https://cse.google.com/) with Google
- Find your Google API key and context https://developers.google.com/custom-search/v1/introduction
- Get a [NASA api key](https://api.nasa.gov/) to be able to use the APOD(Astronomy Picture of the Day) feature

Create the keys file PogBot/src/keys.txt. It should look like this with all the fields filled in:
```
token=[YOUR_DISCORD_BOT_TOKEN]
googlekey=[YOUR_API_KEY]
googlecx=[YOUR_CONTEXT]
nasakey=[NASA_API_KEY]
```
## Running build on linux(Raspberry Pi)
```
sudo chmod a+x PogBot
./PogBot
```
