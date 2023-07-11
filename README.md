**How to start the application?**
----------------

It is required to run the application on the docker due to the creating PostgreSQL database on docker to make it too convenient to use.

You can easily run the application using [Docker](https://www.docker.com). 

 1.Install the Docker

 2.Clone the repository

3. Enter `docker-compose build` to the terminal

4. Enter `docker-compose up`  to the terminal

5. Application will be avialable under http://localhost:3030/swagger/index.html.


# Endpoints
```PM

#Account

GET       /api/Account/profile
POST      /api/Account/register
POST      /api/Account/login
POST      /api/Account/GetUser

#Blog

GET  	  /api/Blogs
POST 	  /api/Blogs
GET  	  /api/Blogs/{blogId}
DELETE    /api/Blogs/{blogId}
PUT       /api/Blogs/{blogId}

#Post

GET  	  /api/blogs/{blogId}/Posts
POST 	  /api/blogs/{blogId}/Posts
DELETE    /api/blogs/{blogId}/Posts/{postId}
PUT       /api/blogs/{blogId}/Posts/{postId}
GET       /api/blogs/{blogId}/Posts/{postId}

#Comment

GET       /api/blogs/{blogId}/Posts/{postId}/comments
POST      /api/blogs/{blogId}/Posts/{postId}/comments
PUT       /api/blogs/{blogId}/Posts/{postId}/comments/{commentId}
DELATE    /api/blogs/{blogId}/Posts/{postId}/comments/{commentId}
