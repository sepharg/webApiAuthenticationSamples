Web Api Authentication Samples
===================


When implementing authentication in web api, I found that even though there are many examples of how to do things on the web, there isn´t a comprehensive full working sample for the different authentication mechanisms.

Therefore, I decided to create this project to showcase the authentication mechanisms in web api and made it so it´s easy to reuse the code for your own projects.


I´ve added unit tests to showcase all of the Api´s methods.


Provided as part of the solution are:

 - **BasicAuthenticationFilterAttribute**: Authorization filter that can be added to Controllers or Actions to authenticate using [Basic Authentication](https://en.wikipedia.org/wiki/Basic_access_authentication) - http://localhost:40598/api/authenticationTest/testbasicauth
 - **DigestAuthenticationFilterAttribute**: Authorization filter that can be added to Controllers or Actions to authenticate using [Digest Authentication](https://en.wikipedia.org/wiki/Digest_access_authentication) - http://localhost:40598/api/authenticationTest/testdigestauth

Note: To run the unit tests, start visual studio as administrator, otherwise it won´t let you create the web server.
