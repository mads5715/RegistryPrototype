# Simple Internal NPM Registry
Simply trying to recreate an NPM registry in C# mostly using the docs, and the client tool it self.
## Current Functionality
- [x] Publish
- [x] Install
- [ ] Login
- [ ] Orgs
- [ ] Everything else
### Hopefull future Functionality
- [ ] Security Audit, manual, and automatic
- [ ] Orginisational control over packages
- [ ] Scanning of packages for code exploits(Like: SQL-Injection and such)
And many more potential features one could wish for in such a tool
## How To Use(Will change slowly over time)
See the Docker-Compose section.
Once running, you'll want to set the registry of your local NPM client like so:
```bash
npm config set registry http://ip-address:port/
```
Once it's up and running, it will allow you to publish locally, and install from it(globally).
A local publish means it at the moment only allows to be published to it selv, and it replaces the url such that it can be downloaded also.
Once you ask for an install it will check it's internal DB, and check if it has the package, if not, it will then forward your call to the official Repo, and in the process cache it for future calls, this is to speed up delivery. First time it loads external data, it will be a bit slower while it forwards the call and caches the result.
## Requirements
There's a few requirements for this to work
   * Docker
   * Docker-Compose
   * NPM client

## Docker-Compose
This is at the moment safest way to run the code, and the registry
```bash
docker-compose up --build
```
## This tool
This tool should only be used internally, this is partly due to it's current development.
When a package is requested that the tool does not have, it will forward it to the public repository as explained earlier.
And of cource, I'm not and neither is the tool affiliated with NPM Inc.
This is all for fun, and just trying to recreate it so it might be usefull for other projects like stated before. There also seem to be a lack of open-souce NPM repos, that is in languages many people use, therefor this tool is made in C#, and not Clojure, or something different.

# What this tool is not!
This tool is not meant to compete with NPMJS or infringe on their official registry, this is simply for people who want to use one for internal security audits, while using a piece open-source, free software, such that they can change and modify to their use.