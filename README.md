# Simple NPM Registry Prototype
Simply trying to recreate an NPM registry in C# mostly using the docs, and the client tool it self.
## Current Functionality
- [x] Publish
- [x] Install
- [ ] Login
- [ ] Orgs
- [ ] Everything else

## This tool
This tool should only be used internally, this is partly due to it's current development.
When a package is requested that the tool does not have, it will forward it to the public repository.
And of cource, I'm not and neither is the tool affiliated with NPM Inc.
This is all for fun, and just trying to recreate it so it might be usefull for other projects like stated before.

## How To Use(Will change slowly over time)
See the Docker-Compose section.
Once it's up and running, it will allow you to publish locally, and install from it(globally).
A local publish means it at the moment only allows to be published to it selv, and it replaces the url such that it can be downloaded also.
Once you ask for an install it will check it's internal DB, and check if it has the package, if not, it will then forward your call to the official Repo, and in the process cache it for future calls, this is to speed up delivery.
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
