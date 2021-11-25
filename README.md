# PokedexApi
A Pokedex built in .NET 5 Web API framework that retrieves a Pokemon's basic information using public APIs.
## How to Run
First, clone the repo in to the desired folder using CLI `git clone https://github.com/sanjayh94/PokedexApi/` or your favourite IDE/GUI tool.

*You will need git CLI installed for this command to work, Find instructions [here](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)*
### Docker
1. Install Docker if you haven't already. Find instructions [here](https://docs.docker.com/get-docker/). _Note: Verify Docker is running on Linux containers mode._
2. Open your preferred terminal and Navigate to the directory where you cloned the repo earlier. For example `cd ~/repos/PokedexApi`
3. Build Image `docker build -f PokedexApi/Dockerfile -t pokedexapi .`
4. Run Image `docker run --rm -p 5000:80 pokedexapi`. 

**Note: the first port denoted after the `-p` parameter is the host port where the application is exposed i.e. port 5000 in this case. Your endpoint port will change depending on the port you specified here.** For example `http://localhost:5000/pokemon/{pokemonName}`. Ignore the `Now listening on: http://[::]:80` in the logs. 
### Visual Studio (or VSCode)
### dotnet CLI
1. Install .NET SDK (CLI tools are bundled) if you haven't already. Find instructions [here](https://dotnet.microsoft.com/download). If you installed Visual Studio in the previous step, .NET CLI should already be installed.
## Running Tests
## Design Decisions and Code Structure
## Approach for Production
