# Division42.NetworkTools

This is a library written against .NET 4.0 which offers basic network tools like: ping, traceroute, whois, and port scanning.

See this blog post for details: http://blog.robseder.com/2015/02/28/new-github-and-nuget-package-division42-networktools/

##What is this?
This is the Visual Studio 2013 / C# / Class Library source code for the "Division42.NetworkTools" NuGet package found [here](https://www.nuget.org/packages/Division42.NetworkTools/).

##What does it do?
Currently, this gives you the ability to do an ICMP "ping" to see if a host is up, port scanning to see which ports are open on a host, traceroute to show all of the stops along the way between you and another host, and whois to lookup the whois information for a domain.

##Background
I've needed this functionality for a second project, so I thought I would take the time to break this out into it's own project, and also create a NuGet package for it, so one might easily add this functionality to their application.
