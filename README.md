# Fashion Advice
A tool to help you find your style! Get it?

## But seriously
Ever had the problem where you see some style defined in a stylesheet but you don't know where it's being used across your codebase? So maybe you look at it and think "I don't know if I should delete that because it might end up breaking something somewhere in the site and then I'll be in trouble."

Fear not! Fashin Advice is here to help you find your style! 

TL;DR: FA will crawl your site looking for a particular CSS selector and will return the pages that has the selector on it.


# Roadmap / features
- Add caching - a site that gets crawled multiple times need not make a request for each page every time.
- Ability to point to a CSS file and say "find all of that" and generate a report afterwards, or even provide a CSS file with all the properties that weren't found removed.
- Create a web service to do this instead of relying on a local .exe

version 0.1


