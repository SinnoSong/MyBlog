﻿namespace MyBlog.Application.HelloWorld.Impl
{
    public class HelloWorldService : ServiceBase, IHelloWorldService
    {
        public string HelloWorld()
        {
            return "Hello World!";
        }
    }
}