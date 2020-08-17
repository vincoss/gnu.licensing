
### Usage - Windows Container

Note: When running Windows Containers, please ensure you have Docker set to use Windows Containers.

When mounting a volume, please ensure the folder exists on the host machine before creating the container.

Double underscore is a standard way to encode colon in ASP.NET Core:

Run the following to create a Gnu.License Server Windows Container:

```
docker run -it --rm -p 1999:80 --name gnulicensesvr -e ConnectionStrings__EfDbContext="Data Source=/var/appdata/Gnu.Licensing.db3;Password=Pass@word1" -e URLS="http://+" -v c:/var/appdata:c:/var/appdata vincoss/gnulicensesvr:1.0.0-windows
```

Once the container is ready, Gnu.License Server can be accessed on port http://localhost:1999/api/license.

### Usage - Linux Container

Note: When using Linux containers on a Windows machine, please ensure you have switched to Linux Containers.

Run the following to create a Gnu.License Server Linux Container:

```
docker run -it --rm -p 1999:80 --name gnulicensesvr -e ConnectionStrings__EfDbContext="Data Source=/var/appdata/Gnu.Licensing.db3;Password=Pass@word1" -e URLS="http://+" -v c:/var/appdata:/var/appdata vincoss/gnulicensesvr:1.0.0-bionic
```

Once the container is ready, Gnu.License Server can be accessed on port http://localhost:1999/api/license.


### How to Engage, Contribute, and Give Feedback

Some of the best ways to contribute are to try things out, file issues, join in design conversations, and make pull-requests.

### Reporting bugs

https://github.com/vincoss/gnu.licensing/issues